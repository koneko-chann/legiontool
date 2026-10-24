"""
Republic Legion CC - Automated Raid Bot with Anti-Detect
Tự động làm nhiệm vụ Raid trên nền tảng republic.legion.cc sử dụng Chrome profile người dùng.
Tích hợp thư viện Antidetect (playwright-stealth + browser evasions) để vượt qua mọi kiểm tra bot.
Hỗ trợ Fetch danh sách dưới dạng bảng có STT 1, 2, 3, 4..., chọn 1 raid cụ thể và lặp nhiều lần.
"""

import os
import sys
import json
import time
import argparse
import subprocess
import sqlite3
import shutil
import tempfile
import queue
import concurrent.futures
from datetime import datetime
from playwright.sync_api import sync_playwright

# Đảm bảo UTF-8 cho stdout/stderr trên Windows console
try:
    if hasattr(sys.stdout, "reconfigure"):
        sys.stdout.reconfigure(encoding="utf-8")
    if hasattr(sys.stderr, "reconfigure"):
        sys.stderr.reconfigure(encoding="utf-8")
except Exception:
    pass

# Anti-detect imports
try:
    from playwright_stealth.stealth import Stealth
    STEALTH_AVAILABLE = True
except ImportError:
    STEALTH_AVAILABLE = False

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
SCREENSHOTS_DIR = os.path.join(BASE_DIR, "screenshots")
os.makedirs(SCREENSHOTS_DIR, exist_ok=True)

CHROME_BIN = r"C:\Program Files\Google\Chrome\Application\chrome.exe"
REAL_USER_DATA = r"C:\Users\Administrator\AppData\Local\Google\Chrome\User Data"
JUNCTION_USER_DATA = r"C:\Users\Administrator\.chrome_full_junction"
CDP_PORT = 9222
RUNNERS_BASE = r"C:\Users\Administrator\.chrome_runners"
os.makedirs(RUNNERS_BASE, exist_ok=True)

def parse_proxy(proxy_str):
    """
    Phân tích chuỗi proxy theo các định dạng phổ biến:
    - ip:port
    - http://ip:port / socks5://ip:port
    - ip:port:user:pass
    - http://user:pass@ip:port
    Trả về (server, username, password)
    """
    if not proxy_str or not str(proxy_str).strip():
        return None, None, None
    proxy_str = str(proxy_str).strip()
    scheme = "http"
    if "://" in proxy_str:
        scheme, rest = proxy_str.split("://", 1)
    else:
        rest = proxy_str

    username, password = None, None
    if "@" in rest:
        auth_part, host_part = rest.split("@", 1)
        if ":" in auth_part:
            username, password = auth_part.split(":", 1)
        else:
            username = auth_part
        server = f"{scheme}://{host_part}"
    elif rest.count(":") == 3:
        parts = rest.split(":")
        server = f"{scheme}://{parts[0]}:{parts[1]}"
        username = parts[2]
        password = parts[3]
    else:
        server = f"{scheme}://{rest}"

    return server, username, password

def setup_runner_env(profile_name):
    """
    Tạo môi trường runner biệt lập cho từng profile để chạy đa luồng đồng thời mà không xung đột file lock.
    """
    if not profile_name:
        profile_name = "Default"
    clean_name = profile_name.replace(" ", "_").replace("/", "_").replace("\\", "_")
    runner_dir = os.path.join(RUNNERS_BASE, f"runner_{clean_name}")
    os.makedirs(runner_dir, exist_ok=True)

    # 1. Copy file Local State
    real_ls = os.path.join(REAL_USER_DATA, "Local State")
    target_ls = os.path.join(runner_dir, "Local State")
    if os.path.exists(real_ls):
        try:
            shutil.copy2(real_ls, target_ls)
        except Exception:
            pass

    # 2. Tạo directory junction cho profile tương ứng
    junc_target = os.path.join(runner_dir, profile_name)
    real_prof_dir = os.path.join(REAL_USER_DATA, profile_name)
    if not os.path.exists(junc_target) and os.path.exists(real_prof_dir):
        cmd = f'cmd.exe /c "mklink /J "{junc_target}" "{real_prof_dir}""'
        subprocess.run(cmd, shell=True, capture_output=True, text=True)

    return runner_dir

def log(msg, level="INFO"):
    ts = datetime.now().strftime("%Y-%m-%d %H:%M:%S")
    print(f"[{ts}] [{level}] {msg}")
    sys.stdout.flush()

def ensure_profile_junction():
    """Tạo directory junction từ Chrome User Data để mở remote debugging mà không làm mất cookie/session."""
    if not os.path.exists(JUNCTION_USER_DATA):
        log("Đang tạo junction cho Chrome User Data...")
        cmd = f'cmd.exe /c "mklink /J \"{JUNCTION_USER_DATA}\" \"{REAL_USER_DATA}\""'
        res = subprocess.run(cmd, shell=True, capture_output=True, text=True)
        log(f"Kết quả tạo junction: {res.stdout.strip()}")

def kill_running_chrome():
    """Tắt các tiến trình Chrome đang chạy để giải phóng cổng 9222 và khóa User Data."""
    log("Dừng các tiến trình Chrome cũ (nếu có)...")
    subprocess.run(["powershell", "-Command", "Stop-Process -Name chrome -Force -ErrorAction SilentlyContinue"])
    time.sleep(1.5)

def launch_chrome(profile_directory="Default"):
    ensure_profile_junction()
    kill_running_chrome()

    cmd = [
        CHROME_BIN,
        f"--user-data-dir={JUNCTION_USER_DATA}",
        f"--profile-directory={profile_directory}",
        f"--remote-debugging-port={CDP_PORT}",
        "--no-first-run",
        "--no-default-browser-check",
        "--disable-blink-features=AutomationControlled",
        "--excludeSwitches=enable-automation",
        "--useAutomationExtension=false"
    ]
    log(f"Khởi chạy Chrome (Anti-Detect Mode) profile '{profile_directory}' qua CDP port {CDP_PORT}...")
    proc = subprocess.Popen(cmd)
    time.sleep(3)
    return proc

def scan_chrome_profiles(user_data_dir=REAL_USER_DATA):
    """
    Quét tất cả Chrome profiles trong User Data để kiểm tra cookie của:
    - Republic Legion (republic.legion.cc / legion.cc)
    - X (x.com / twitter.com)
    Trả về danh sách các profile kèm phân tích và gợi ý profile tốt nhất.
    """
    local_state_path = os.path.join(user_data_dir, "Local State")
    profile_info = {}
    if os.path.exists(local_state_path):
        try:
            with open(local_state_path, "r", encoding="utf-8-sig") as f:
                ls = json.load(f)
                info_cache = ls.get("profile", {}).get("info_cache", {})
                for k, v in info_cache.items():
                    name = v.get("name") or k
                    user_name = v.get("user_name") or ""
                    profile_info[k] = {"name": name, "user_name": user_name}
        except Exception:
            pass

    results = []
    if not os.path.exists(user_data_dir):
        return results

    dirs = [d for d in os.listdir(user_data_dir) if os.path.isdir(os.path.join(user_data_dir, d))]

    for d in sorted(dirs):
        p_path = os.path.join(user_data_dir, d)
        cookie_path = os.path.join(p_path, "Network", "Cookies")
        if not os.path.exists(cookie_path):
            cookie_path = os.path.join(p_path, "Cookies")
        if not os.path.exists(cookie_path):
            continue

        meta = profile_info.get(d, {"name": d, "user_name": ""})
        has_republic = False
        has_x = False
        republic_cookies = []
        x_cookies = []

        tmp_name = os.path.join(tempfile.gettempdir(), f"chk_{abs(hash(d))}.db")
        try:
            shutil.copy2(cookie_path, tmp_name)
            conn = sqlite3.connect(tmp_name)
            c = conn.cursor()
            c.execute("SELECT host_key FROM cookies")
            for row in c.fetchall():
                h = row[0].lower()
                if "legion.cc" in h:
                    has_republic = True
                    republic_cookies.append(h)
                if "x.com" in h or "twitter.com" in h:
                    has_x = True
                    x_cookies.append(h)
            conn.close()
        except Exception:
            pass
        finally:
            if os.path.exists(tmp_name):
                try:
                    os.remove(tmp_name)
                except Exception:
                    pass

        has_republic_hist = False
        has_x_hist = False
        hist_path = os.path.join(p_path, "History")
        if os.path.exists(hist_path):
            tmp_hist = os.path.join(tempfile.gettempdir(), f"hist_{abs(hash(d))}.db")
            try:
                shutil.copy2(hist_path, tmp_hist)
                conn = sqlite3.connect(tmp_hist)
                c = conn.cursor()
                c.execute("SELECT url FROM urls WHERE url LIKE '%legion.cc%' OR url LIKE '%x.com%' OR url LIKE '%twitter.com%' LIMIT 20")
                for row in c.fetchall():
                    u = row[0].lower()
                    if "legion.cc" in u:
                        has_republic_hist = True
                    if "x.com" in u or "twitter.com" in u:
                        has_x_hist = True
                conn.close()
            except Exception:
                pass
            finally:
                if os.path.exists(tmp_hist):
                    try:
                        os.remove(tmp_hist)
                    except Exception:
                        pass

        is_recommended = has_republic and has_x

        if has_republic and has_x:
            status_text = "⭐ Khuyên dùng (Đã có cả Republic Legion & X)"
        elif has_republic:
            status_text = "🟡 Có Republic Legion (Chưa có cookie X)"
        elif has_x:
            status_text = "🟡 Có X (Chưa có cookie Republic Legion)"
        elif has_republic_hist or has_x_hist:
            status_text = "⚪ Có lịch sử duyệt web"
        else:
            status_text = "⚪ Chưa đăng nhập"

        results.append({
            "directory": d,
            "name": meta["name"],
            "user_name": meta["user_name"],
            "has_republic": has_republic,
            "has_x": has_x,
            "has_republic_hist": has_republic_hist,
            "has_x_hist": has_x_hist,
            "is_recommended": is_recommended,
            "status_text": status_text,
            "republic_cookies": list(set(republic_cookies)),
            "x_cookies": list(set(x_cookies))
        })

    return results

def scan_and_print_profiles():
    log("Bắt đầu quét tất cả các Profile Chrome để tìm cookie Republic Legion và X...")
    profiles = scan_chrome_profiles()
    log(f"Đã tìm thấy {len(profiles)} Profile Chrome.")

    recommended = [p for p in profiles if p["is_recommended"]]
    rep_only = [p for p in profiles if p["has_republic"] and not p["has_x"]]

    suggestion = ""
    if recommended:
        best = recommended[0]
        suggestion = f"Gợi ý: Profile '{best['directory']}' ({best['name']}) có đầy đủ cookie của cả Republic Legion và X! Khuyên dùng profile này."
    elif rep_only:
        best = rep_only[0]
        suggestion = f"Gợi ý: Profile '{best['directory']}' ({best['name']}) đã có cookie Republic Legion. Hãy mở profile này đăng nhập thêm tài khoản X (Twitter) để hoàn tất 100%!"
    else:
        suggestion = "Chưa tìm thấy Profile nào có sẵn cookie. Hãy đăng nhập tài khoản trên Chrome trước."

    log(suggestion)

    output = {
        "suggestion": suggestion,
        "profiles": profiles
    }

    print("###PROFILES_JSON_START###")
    print(json.dumps(output, ensure_ascii=False, indent=2))
    print("###PROFILES_JSON_END###")
    return output

def apply_antidetect(page):
    """Áp dụng thư viện Anti-Detect lên page (giấu navigator.webdriver, mock plugins, codecs, permissions)."""
    if STEALTH_AVAILABLE:
        try:
            stealth = Stealth()
            stealth.apply_stealth_sync(page)
            log("Đã kích hoạt anti-detect (playwright-stealth) thành công cho tab.")
        except Exception as e:
            log(f"Lỗi khi áp dụng stealth: {e}", "WARN")

    # Bổ sung các evasions tùy biến cấp thấp
    try:
        page.add_init_script("""
            Object.defineProperty(navigator, 'webdriver', {
                get: () => undefined
            });
            window.chrome = {
                runtime: {},
                loadTimes: function() {},
                csi: function() {},
                app: {}
            };
        """)
    except Exception:
        pass

def save_screenshot(page, label, raid_id=None):
    ts = datetime.now().strftime("%Y%m%d_%H%M%S")
    prefix = f"{ts}_{raid_id}" if raid_id else ts
    fname = f"{prefix}_{label}.png"
    fpath = os.path.join(SCREENSHOTS_DIR, fname)
    try:
        page.screenshot(path=fpath, full_page=False)
        log(f"Đã chụp màn hình: {fname}")
        return fpath
    except Exception as e:
        log(f"Lỗi chụp ảnh: {e}", "WARN")
        return None

def check_and_authenticate(page):
    """Kiểm tra nếu phiên đăng nhập bị hết hạn và tự động đăng nhập lại qua Discord."""
    sign_in = page.locator("button:has-text('Sign in with Discord'), a:has-text('Sign in with Discord')")
    if sign_in.count() > 0 and sign_in.first.is_visible():
        log("Phát hiện nút 'Sign in with Discord', đang tiến hành xác thực...")
        sign_in.first.click()
        time.sleep(3)
        auth = page.locator("button:has-text('Authorize'), button:has-text('Phê duyệt')")
        if auth.count() > 0:
            log("Nhấn nút Phê duyệt trên Discord...")
            auth.first.click()
            time.sleep(4)
        if "raids" not in page.url:
            page.goto("https://republic.legion.cc/raids", wait_until="domcontentloaded")
            time.sleep(3)
        save_screenshot(page, "authenticated_session")

def wait_and_get_live_raid_cards(page, timeout=40, log_prefix=""):
    """
    Chờ máy chủ phản hồi và các thẻ raid hiển thị đầy đủ trên giao diện.
    Tự động xử lý:
    1. Kiểm tra xác thực (Sign in with Discord nếu cần)
    2. Tự động click nút 'View all' (.home-lobby-raids-view-all) qua JavaScript
    3. Chờ trạng thái skeleton loading kết thúc và API trả dữ liệu
    4. Trả về locator của các thẻ raid đã sẵn sàng (modal hoặc fallback trang chính)
    """
    pfx = f"{log_prefix} " if log_prefix else ""
    log(f"{pfx}Đang kết nối và chờ dữ liệu Live Raids từ máy chủ...")

    start_time = time.time()

    # 1. Kiểm tra đăng nhập
    check_and_authenticate(page)

    # 2. Vòng lặp tự động click View all và chờ cards
    view_all_clicked = False
    while time.time() - start_time < timeout:
        # Kiểm tra xem thẻ trong modal đã hiển thị đầy đủ chưa
        modal_cards = page.locator(".home-modal-overlay .home-lobby-raid-card")
        cnt = modal_cards.count()
        if cnt > 0:
            try:
                first_txt = modal_cards.first.inner_text().strip()
                if len(first_txt) > 3 and any(k in first_txt for k in ["@", "$", "LIVE", "PROOF", "Ends in", "Raider", "VP"]):
                    log(f"{pfx}Máy chủ đã phản hồi! Tải thành công {cnt} thẻ Live Raids trong modal.")
                    return modal_cards
            except Exception:
                pass

        # Tự động tìm và click nút 'View all' bằng JavaScript
        clicked = page.evaluate("""() => {
            const btn = document.querySelector('.home-lobby-raids-view-all') || 
                        Array.from(document.querySelectorAll('button')).find(b => b.innerText && b.innerText.includes('View all'));
            if (btn) {
                btn.scrollIntoView({ behavior: 'instant', block: 'center' });
                btn.click();
                return true;
            }
            return false;
        }""")
        if clicked and not view_all_clicked:
            log(f"{pfx}Đã tự động click nút 'View all' qua JavaScript để mở rộng Live Raids!")
            view_all_clicked = True

        time.sleep(1)

    # Fallback: kiểm tra trên toàn trang chính nếu modal không có
    page_cards = page.locator(".home-lobby-raid-card")
    if page_cards.count() > 0:
        log(f"{pfx}Tìm thấy {page_cards.count()} thẻ raid trên trang chính.")
        return page_cards

    log(f"{pfx}Hết thời gian chờ phản hồi ({timeout}s), không tìm thấy thẻ raid nào!", "WARNING")
    return page.locator(".home-modal-overlay .home-lobby-raid-card")

def fetch_raids_json(profile_directory="Default"):
    """Lấy danh sách tất cả các raid hiển thị dưới dạng bảng có số thứ tự 1, 2, 3..."""
    chrome_proc = launch_chrome(profile_directory)
    raids_list = []
    try:
        with sync_playwright() as p:
            browser = p.chromium.connect_over_cdp(f"http://127.0.0.1:{CDP_PORT}")
            context = browser.contexts[0]
            page = context.new_page()
            page.set_viewport_size({"width": 1440, "height": 900})
            apply_antidetect(page)

            log("Mở trang Live Raids để tải danh sách...")
            page.goto("https://republic.legion.cc/raids", wait_until="domcontentloaded", timeout=45000)

            modal_cards = wait_and_get_live_raid_cards(page, timeout=40, log_prefix="[Fetch]")
            card_count = modal_cards.count()
            log(f"Đã đọc được {card_count} thẻ raid từ trang.")

            stt = 1
            for i in range(card_count):
                c = modal_cards.nth(i)
                raw_text = c.inner_text().strip()
                lines = [line.strip() for line in raw_text.split("\n") if line.strip()]

                status = "LIVE"
                target = "Unknown"
                time_left = ""
                raiders = ""

                for line in lines:
                    if line in ["LIVE", "PROOF NEEDED", "COMPLETED", "MISSED", "NEEDS UPDATES"]:
                        status = line
                    elif line.startswith("@") or line.startswith("$") or "Guide" in line or "Republic" in line:
                        target = line
                    elif "Ends in" in line:
                        time_left = line
                    elif "Raider" in line:
                        raiders = line

                if target == "Unknown" and len(lines) > 1:
                    target = lines[1] if lines[0] in ["LIVE", "PROOF NEEDED", "NEEDS UPDATES"] else lines[0]

                can_run = status in ["LIVE", "PROOF NEEDED", "NEEDS UPDATES"]

                raids_list.append({
                    "stt": stt,
                    "card_index": i,
                    "target": target,
                    "time_left": time_left,
                    "raiders": raiders,
                    "status": status,
                    "can_run": can_run,
                    "raw": " | ".join(lines)
                })
                stt += 1
    finally:
        chrome_proc.terminate()
        log("Đã đóng trình duyệt sau khi fetch.")

    print("###RAIDS_JSON_START###")
    print(json.dumps(raids_list, ensure_ascii=False, indent=2))
    print("###RAIDS_JSON_END###")
    return raids_list

def capture_x_proofs(context, tweet_url, raid_key):
    """Mở bài viết X (Twitter), tạo giao diện Like/Repost và Copy Link rồi chụp ảnh làm bằng chứng."""
    proof1_path = os.path.join(SCREENSHOTS_DIR, f"proof1_like_repost_{raid_key}.png")
    proof2_path = os.path.join(SCREENSHOTS_DIR, f"proof2_copylink_{raid_key}.png")

    log(f"Đang mở bài viết X: {tweet_url}")
    tweet_page = context.new_page()
    tweet_page.set_viewport_size({"width": 1280, "height": 850})
    apply_antidetect(tweet_page)

    try:
        tweet_page.goto(tweet_url, wait_until="domcontentloaded", timeout=25000)
        
        # Chờ tweet article tải xong
        try:
            tweet_page.wait_for_selector("article", timeout=12000)
        except Exception:
            pass
        time.sleep(2)

        # Ẩn overlay đăng nhập và highlight nút Like/Repost
        tweet_page.evaluate("""() => {
            const layers = document.getElementById('layers');
            if (layers) layers.style.display = 'none';

            // Highlight Like (màu hồng)
            const likeBtn = document.querySelector('article [aria-label*="Like"]');
            if (likeBtn) {
                likeBtn.style.color = '#f91880';
                const svg = likeBtn.querySelector('svg');
                if (svg) {
                    svg.style.color = '#f91880';
                    svg.style.fill = '#f91880';
                }
            }

            // Highlight Repost (màu xanh lá)
            const repostBtn = document.querySelector('article [aria-label*="Repost"]');
            if (repostBtn) {
                repostBtn.style.color = '#00ba7c';
                const svg = repostBtn.querySelector('svg');
                if (svg) {
                    svg.style.color = '#00ba7c';
                    svg.style.fill = '#00ba7c';
                }
            }
        }""")
        time.sleep(1)

        # Chụp Proof 1 (Like + Repost)
        article = tweet_page.locator("article").first
        if article.count() > 0:
            article.screenshot(path=proof1_path)
        else:
            tweet_page.screenshot(path=proof1_path)
        log(f"Đã lưu Proof 1 (Like & Repost): {os.path.basename(proof1_path)}")

        # Tạo toast "Copied to clipboard" giống giao diện thật trên X
        tweet_page.evaluate("""() => {
            const toast = document.createElement('div');
            toast.id = 'copy-toast';
            toast.innerText = 'Copied to clipboard';
            toast.style.position = 'fixed';
            toast.style.bottom = '40px';
            toast.style.left = '50%';
            toast.style.transform = 'translateX(-50%)';
            toast.style.backgroundColor = 'rgb(29, 155, 240)';
            toast.style.color = 'white';
            toast.style.padding = '12px 24px';
            toast.style.borderRadius = '4px';
            toast.style.fontSize = '15px';
            toast.style.fontWeight = '500';
            toast.style.fontFamily = '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif';
            toast.style.zIndex = '999999';
            toast.style.boxShadow = '0 4px 12px rgba(0,0,0,0.2)';
            document.body.appendChild(toast);
        }""")
        time.sleep(1)

        # Chụp Proof 2 (Copy link)
        tweet_page.screenshot(path=proof2_path)
        log(f"Đã lưu Proof 2 (Copy Link Toast): {os.path.basename(proof2_path)}")

        return proof1_path, proof2_path
    except Exception as e:
        log(f"Lỗi khi xử lý bài viết X: {e}", "ERROR")
        return None, None
    finally:
        tweet_page.close()

def process_single_raid(page, context, card, raid_index, profile_name="Default", log_prefix=""):
    """Thực hiện quy trình hoàn chỉnh của một nhiệm vụ raid."""
    pfx = f"{log_prefix} " if log_prefix else ""
    card_text = card.inner_text().strip().replace("\n", " ")
    log(f"{pfx}--- BẮT ĐẦU RAID [Index {raid_index}]: {card_text[:50]} ---")

    try:
        card.scroll_into_view_if_needed(timeout=5000)
    except Exception:
        pass
    card.click(force=True)

    # Chờ giao diện chi tiết Protocol xuất hiện từ máy chủ
    log(f"{pfx}Đang mở chi tiết nhiệm vụ và chờ máy chủ phản hồi protocol...")
    start_proto = time.time()
    while time.time() - start_proto < 20:
        if page.locator("button:has-text('Equip Supplies'), a:has-text('Open post')").count() > 0:
            break
        time.sleep(1)

    save_screenshot(page, "01_raid_protocol_opened", f"{profile_name}_raid_{raid_index}")

    equip_btn = page.locator("button:has-text('Equip Supplies')")
    if equip_btn.count() > 0 and equip_btn.first.is_visible():
        log(f"{pfx}Đang trang bị supplies (-5 VP)...")
        equip_btn.first.click()
        
        # Chờ xử lý xong equip và link Open post xuất hiện
        start_eq = time.time()
        while time.time() - start_eq < 15:
            if page.locator("a:has-text('Open post')").count() > 0:
                break
            time.sleep(1)
        save_screenshot(page, "02_supplies_equipped", f"{profile_name}_raid_{raid_index}")

    # Chờ và lấy link Open post
    open_post = page.locator("a:has-text('Open post')")
    open_post_link = None
    start_op = time.time()
    while time.time() - start_op < 15:
        if open_post.count() > 0:
            open_post_link = open_post.first.get_attribute("href")
            if open_post_link:
                break
        time.sleep(1)

    if open_post_link:
        log(f"{pfx}Target X Post: {open_post_link}")
    else:
        log(f"{pfx}Không tìm thấy link Open post!", "WARN")
        return False

    proof1, proof2 = capture_x_proofs(context, open_post_link, f"{profile_name}_raid_{raid_index}")
    if not proof1 or not proof2:
        log(f"{pfx}Không tạo được ảnh bằng chứng, hủy raid này.", "ERROR")
        return False

    cb_like = page.locator("#raid-action-like-screenshot")
    cb_repost = page.locator("#raid-action-repost-screenshot")
    cb_copy = page.locator("#raid-action-copy-link-screenshot")

    if cb_like.count() > 0 and not cb_like.is_checked():
        cb_like.check(force=True)
    if cb_repost.count() > 0 and not cb_repost.is_checked():
        cb_repost.check(force=True)
    if cb_copy.count() > 0 and not cb_copy.is_checked():
        cb_copy.check(force=True)
    time.sleep(2)

    f_like_repost = page.locator("#raid-screenshot-like-repost")
    f_copylink = page.locator("#raid-image-copy-link-screenshot")

    if f_like_repost.count() > 0 and os.path.exists(proof1):
        log(f"{pfx}Đính kèm ảnh bằng chứng Like/Repost...")
        f_like_repost.set_input_files(proof1)
        time.sleep(2)

    if f_copylink.count() > 0 and os.path.exists(proof2):
        log(f"{pfx}Đính kèm ảnh bằng chứng Copy link...")
        f_copylink.set_input_files(proof2)
        time.sleep(2)

    save_screenshot(page, "03_proofs_attached", f"{profile_name}_raid_{raid_index}")

    submit_btn = page.locator("button:has-text('Submit proof')")
    result = False
    if submit_btn.count() > 0 and not submit_btn.first.is_disabled():
        log(f"{pfx}Nhấn nút 'Submit proof'...")
        submit_btn.first.click()
        
        # Chờ máy chủ xác nhận kết quả Submit proof
        log(f"{pfx}Đang chờ máy chủ xác nhận Submit proof...")
        start_sub = time.time()
        while time.time() - start_sub < 15:
            if page.locator("text='Successfully', text='submitted', text='Submitted', text='Under review'").count() > 0:
                break
            time.sleep(1)

        time.sleep(2)
        save_screenshot(page, "04_submitted_proof", f"{profile_name}_raid_{raid_index}")
        log(f"{pfx}==> ĐÃ GỬI BẰNG CHỨNG THÀNH CÔNG CHO RAID [Index {raid_index}]!")
        result = True
    else:
        log(f"{pfx}Nút 'Submit proof' không khả dụng hoặc bị vô hiệu hóa!", "WARN")
        result = False

    # Quay lại danh sách cards để sẵn sàng cho raid tiếp theo
    try:
        close_btn = page.locator("button:has-text('✕'), button[aria-label='Close'], button:has-text('Back')")
        if close_btn.count() > 0 and close_btn.first.is_visible():
            close_btn.first.click(force=True)
            time.sleep(2)
        else:
            page.goto("https://republic.legion.cc/raids", wait_until="domcontentloaded")
            time.sleep(3)
    except Exception:
        pass

    return result

def test_proxy_connection(proxy_str):
    """Kiểm tra kết nối và địa chỉ IP ngoại mạng qua proxy."""
    server, user, password = parse_proxy(proxy_str)
    if not server:
        msg = "Proxy không hợp lệ hoặc để trống."
        log(f"[PROXY TEST] {msg}", "WARN")
        return False, msg

    if user and password:
        scheme, host = server.split("://", 1)
        p_url = f"{scheme}://{user}:{password}@{host}"
    else:
        p_url = server

    proxies = {"http": p_url, "https": p_url}
    t0 = time.time()
    try:
        import requests
        r = requests.get("https://api64.ipify.org?format=json", proxies=proxies, timeout=10)
        ms = int((time.time() - t0) * 1000)
        if r.status_code == 200:
            ip = r.json().get("ip", "Unknown")
            res_msg = f"IP: {ip} ({ms}ms) - Hoạt động tốt!"
            log(f"[PROXY TEST THÀNH CÔNG] {server} -> {res_msg}")
            return True, res_msg
        else:
            res_msg = f"HTTP {r.status_code} ({ms}ms)"
            log(f"[PROXY TEST THẤT BẠI] {server} -> {res_msg}", "ERROR")
            return False, res_msg
    except Exception as e:
        res_msg = f"Lỗi: {str(e)[:60]}"
        log(f"[PROXY TEST LỖI] {server} -> {res_msg}", "ERROR")
        return False, res_msg

def run_single_profile_worker(profile_name, proxy_str="", target_stt=None, target_name=None, loop_count=1, delay=5):
    """
    Thực thi 1 luồng riêng biệt cho 1 Profile cụ thể, hỗ trợ Proxy (kể cả có Auth) qua launch_persistent_context.
    Tất cả các profile cùng hướng về 1 TARGET RAID duy nhất.
    """
    log_pfx = f"[{profile_name}]"
    log(f"{log_pfx} Khởi tạo môi trường runner độc lập...", "INFO")

    runner_dir = setup_runner_env(profile_name)
    proxy_server, proxy_user, proxy_pass = parse_proxy(proxy_str)

    proxy_cfg = None
    if proxy_server:
        proxy_cfg = {"server": proxy_server}
        if proxy_user and proxy_pass:
            proxy_cfg["username"] = proxy_user
            proxy_cfg["password"] = proxy_pass
        log(f"{log_pfx} Sử dụng Proxy: {proxy_server} (User: {proxy_user or 'None'})")

    for current_loop in range(1, loop_count + 1):
        log(f"{log_pfx} === BẮT ĐẦU VÒNG LẶP {current_loop}/{loop_count} ===")

        try:
            with sync_playwright() as p:
                context = p.chromium.launch_persistent_context(
                    user_data_dir=runner_dir,
                    executable_path=CHROME_BIN,
                    headless=False,
                    proxy=proxy_cfg,
                    viewport={"width": 1440, "height": 900},
                    args=[
                        f"--profile-directory={profile_name}",
                        "--no-first-run",
                        "--no-default-browser-check",
                        "--disable-blink-features=AutomationControlled",
                        "--excludeSwitches=enable-automation",
                        "--useAutomationExtension=false"
                    ]
                )
                page = context.pages[0] if context.pages else context.new_page()
                apply_antidetect(page)

                log(f"{log_pfx} Mở trang: https://republic.legion.cc/raids")
                page.goto("https://republic.legion.cc/raids", wait_until="domcontentloaded", timeout=45000)

                # Chờ máy chủ phản hồi và render danh sách cards (vượt qua skeleton loading)
                modal_cards = wait_and_get_live_raid_cards(page, timeout=40, log_prefix=log_pfx)
                card_count = modal_cards.count()
                log(f"{log_pfx} Tìm thấy {card_count} thẻ raid trên giao diện.")

                save_screenshot(page, f"loop_{current_loop}_lobby", f"{profile_name}")

                target_card = None
                target_idx = -1

                # 1. Tìm theo target_name nếu có
                if target_name and card_count > 0:
                    clean_target = target_name.strip().lstrip("@").lower()
                    candidates = []
                    for i in range(card_count):
                        txt = modal_cards.nth(i).inner_text().strip()
                        txt_l = txt.lower()
                        if clean_target in txt_l or target_name.strip().lower() in txt_l:
                            candidates.append((i, txt))

                    # Ưu tiên thẻ đang Active (không phải MISSED hoặc COMPLETED)
                    for i, txt in candidates:
                        if "MISSED" not in txt and "COMPLETED" not in txt:
                            target_card = modal_cards.nth(i)
                            target_idx = i
                            log(f"{log_pfx} Khớp mục tiêu '{target_name}' (đang Active) tại thẻ #{i+1}")
                            break

                    # Nếu không có thẻ active, lấy thẻ khớp đầu tiên
                    if not target_card and candidates:
                        target_idx, _ = candidates[0]
                        target_card = modal_cards.nth(target_idx)
                        log(f"{log_pfx} Khớp mục tiêu '{target_name}' tại thẻ #{target_idx+1}")

                # 2. Nếu không thấy target_name, tìm theo STT
                if not target_card and target_stt is not None and card_count > 0:
                    idx = target_stt - 1
                    if 0 <= idx < card_count:
                        target_card = modal_cards.nth(idx)
                        target_idx = idx
                        log(f"{log_pfx} Chọn mục tiêu theo STT {target_stt} (thẻ #{idx+1})")

                if target_card:
                    log(f"{log_pfx} Bắt đầu thực thi raid mục tiêu [{target_name or f'STT {target_stt}'}] tại Card #{target_idx+1}")
                    process_single_raid(page, context, target_card, target_idx, profile_name=profile_name, log_prefix=log_pfx)
                else:
                    log(f"{log_pfx} Không tìm thấy thẻ raid mục tiêu: {target_name or f'STT {target_stt}'}!", "ERROR")

                save_screenshot(page, f"loop_{current_loop}_finished", f"{profile_name}")
                context.close()
                log(f"{log_pfx} Đã đóng Chrome phiên làm việc.")
        except Exception as e:
            log(f"{log_pfx} Lỗi thực thi: {e}", "ERROR")

        if current_loop < loop_count:
            log(f"{log_pfx} Chờ {delay}s trước vòng tiếp theo...")
            time.sleep(delay)

    log(f"{log_pfx} ==> HOÀN TẤT PROFILE {profile_name}! <==")

def run_multithread_raids(profiles_config, target_stt=None, target_name=None, threads=2, loop_count=1, delay=5):
    """
    Chạy đa luồng nhiều Profile Chrome thực hiện CÙNG 1 TARGET RAID.
    profiles_config: List các dict [{"profile": "Default", "proxy": "..."}, ...]
    """
    total = len(profiles_config)
    log(f"\n=======================================================")
    log(f"=== BẮT ĐẦU ĐA LUỒNG: {total} Profiles | {threads} Threads | Target: {target_name or f'STT {target_stt}'} ===")
    log(f"=======================================================")

    def worker_wrapper(item):
        prof = item.get("profile") or item.get("directory") or item.get("ProfileDir") or item.get("dir") or item.get("Profile") or "Default"
        prx = item.get("proxy") or item.get("Proxy") or ""
        run_single_profile_worker(
            profile_name=prof,
            proxy_str=prx,
            target_stt=target_stt,
            target_name=target_name,
            loop_count=loop_count,
            delay=delay
        )

    with concurrent.futures.ThreadPoolExecutor(max_workers=threads) as executor:
        futures = [executor.submit(worker_wrapper, item) for item in profiles_config]
        for f in concurrent.futures.as_completed(futures):
            try:
                f.result()
            except Exception as e:
                log(f"Lỗi thread worker: {e}", "ERROR")

    log(f"\n===> HOÀN TẤT TẤT CẢ {total} PROFILES CHO TARGET NÀY! <===")

def run_loop(target_card_index=None, target_stt=None, loop_count=1, delay=5, profile_directory="Default"):
    """
    Chạy tự động hóa với số lần lặp lại (loop_count).
    Có thể chọn 1 raid cụ thể (target_card_index hoặc target_stt) hoặc chạy tất cả.
    """
    log(f"Bắt đầu phiên làm việc: Profile={profile_directory}, Loop={loop_count}, TargetCardIndex={target_card_index}, TargetSTT={target_stt}, Delay={delay}s")

    for current_loop in range(1, loop_count + 1):
        log(f"\n==========================================")
        log(f"=== BẮT ĐẦU VÒNG LẶP {current_loop}/{loop_count} ===")
        log(f"==========================================")

        chrome_proc = launch_chrome(profile_directory)
        try:
            with sync_playwright() as p:
                browser = p.chromium.connect_over_cdp(f"http://127.0.0.1:{CDP_PORT}")
                context = browser.contexts[0]
                page = context.new_page()
                page.set_viewport_size({"width": 1440, "height": 900})
                apply_antidetect(page)

                log("Mở trang Republic Raids: https://republic.legion.cc/raids")
                page.goto("https://republic.legion.cc/raids", wait_until="domcontentloaded", timeout=45000)

                modal_cards = wait_and_get_live_raid_cards(page, timeout=40, log_prefix=f"[Loop {current_loop}]")
                card_count = modal_cards.count()
                log(f"Tìm thấy {card_count} thẻ raid trong Live Raids modal")

                save_screenshot(page, f"loop_{current_loop}_lobby")

                selected_indices = []

                if target_card_index is not None:
                    if 0 <= target_card_index < card_count:
                        selected_indices.append(target_card_index)
                    else:
                        log(f"Lỗi: card_index {target_card_index} ngoài phạm vi (0-{card_count-1})!", "ERROR")
                elif target_stt is not None:
                    idx = target_stt - 1
                    if 0 <= idx < card_count:
                        selected_indices.append(idx)
                    else:
                        log(f"Lỗi: STT {target_stt} ngoài phạm vi (1-{card_count})!", "ERROR")
                else:
                    for i in range(card_count):
                        txt = modal_cards.nth(i).inner_text().strip().replace("\n", " ")
                        if "COMPLETED" in txt or "MISSED" in txt:
                            continue
                        selected_indices.append(i)

                log(f"Danh sách chỉ số raid được chọn thực thi trong vòng {current_loop}: {selected_indices}")

                for idx in selected_indices:
                    cards_current = page.locator(".home-modal-overlay .home-lobby-raid-card")
                    if idx < cards_current.count():
                        target_card = cards_current.nth(idx)
                        process_single_raid(page, context, target_card, idx)
                        time.sleep(delay)

                save_screenshot(page, f"loop_{current_loop}_finished")
        except Exception as e:
            log(f"Lỗi trong vòng lặp {current_loop}: {e}", "ERROR")
        finally:
            chrome_proc.terminate()
            log(f"Đã đóng Chrome sau vòng lặp {current_loop}.")

        if current_loop < loop_count:
            log(f"Đang chờ {delay} giây trước khi bắt đầu vòng tiếp theo...")
            time.sleep(delay)

    log(f"\n===> HOÀN TẤT TẤT CẢ {loop_count} VÒNG LẶP! <===")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Republic Legion Raid Automation Tool")
    parser.add_argument("--scan-profiles", action="store_true", help="Quét tất cả Profile Chrome để tìm cookie Republic Legion và X")
    parser.add_argument("--profile", type=str, default="Default", help="Chỉ định profile directory cần chạy đơn lẻ (ví dụ: Default, Profile 1...)")
    parser.add_argument("--profiles-json", type=str, default=None, help="JSON hoặc đường dẫn file JSON cấu hình danh sách profiles và proxy")
    parser.add_argument("--target-name", type=str, default=None, help="Tên mục tiêu Raid (ví dụ: @TowerExchange)")
    parser.add_argument("--threads", type=int, default=2, help="Số luồng chạy đồng thời (mặc định: 2)")
    parser.add_argument("--proxy", type=str, default=None, help="Proxy cho profile đơn lẻ")
    parser.add_argument("--fetch", action="store_true", help="Lấy danh sách các raid hiện có dưới dạng JSON")
    parser.add_argument("--card-index", type=int, default=None, help="Chỉ định card_index của raid cần làm")
    parser.add_argument("--stt", type=int, default=None, help="Chỉ định STT (1, 2, 3...) của raid cần làm")
    parser.add_argument("--loop", type=int, default=1, help="Số lần lặp lại nhiệm vụ (mặc định 1)")
    parser.add_argument("--test-proxy", type=str, default=None, help="Kiểm tra kết nối và địa chỉ IP ngoại mạng qua proxy")
    parser.add_argument("--delay", type=int, default=5, help="Thời gian chờ giữa các lần (giây)")

    args = parser.parse_args()

    if args.test_proxy:
        test_proxy_connection(args.test_proxy)
    elif args.scan_profiles:
        scan_and_print_profiles()
    elif args.fetch:
        fetch_raids_json(profile_directory=args.profile)
    elif args.profiles_json:
        p_data = args.profiles_json.strip()
        if os.path.exists(p_data):
            with open(p_data, "r", encoding="utf-8-sig") as f:
                profiles_config = json.load(f)
        else:
            profiles_config = json.loads(p_data.lstrip('\ufeff'))

        run_multithread_raids(
            profiles_config=profiles_config,
            target_stt=args.stt,
            target_name=args.target_name,
            threads=args.threads,
            loop_count=args.loop,
            delay=args.delay
        )
    else:
        run_loop(
            target_card_index=args.card_index,
            target_stt=args.stt,
            loop_count=args.loop,
            delay=args.delay,
            profile_directory=args.profile
        )
