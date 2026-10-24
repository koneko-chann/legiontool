# Republic Legion CC - Multi-Profile & Multi-Thread Raid Automation Tool

Công cụ tự động hóa toàn diện làm nhiệm vụ **Raid** trên nền tảng [republic.legion.cc](https://republic.legion.cc) tích hợp:
- **Chọn nhiều Profile Chrome & Chạy Đa Luồng (Multithreading)**: Cho phép tích chọn nhiều Profile cùng một lúc và chạy song song đồng thời trên nhiều luồng độc lập, mỗi luồng có runner riêng biệt không sợ xung đột khóa dữ liệu.
- **Hỗ trợ Proxy chuyên dụng (Đã kiểm nghiệm với Webshare)**: Tích hợp đầy đủ cho proxy dạng `IP:PORT:USERNAME:PASSWORD` (ví dụ file `Webshare 100 proxies.txt`).
- **Nạp file Proxy 1-Click (`📂 Nạp file Proxy`) & Kiểm tra tốc độ (`⚡ Test Proxy`)**: Tự động đọc file proxy, phân bổ vào danh sách profile và kiểm tra IP ngoại mạng trước khi chạy.
- **Tập trung vào 1 Target Raid duy nhất**: Tất cả các Profile được chọn sẽ cùng hướng về và hoàn thành duy nhất 1 nhiệm vụ Raid được chỉ định.
- **Tính năng quét Profile Chrome thông minh**: Tự động rà soát tất cả Chrome profiles trên máy tính để tìm profile nào chứa cookie của cả `republic.legion.cc` và `X (Twitter)`.
- **Giao diện đồ họa WinForms hiện đại (.NET 10)**: Bảng danh sách profile kèm ô nhập proxy, bảng chọn Target Raid đánh số STT 1, 2, 3..., nút Fetch, tùy chỉnh số luồng (Threads), số vòng lặp (Loop), live log và xem trước ảnh chụp màn hình trực tiếp.
- **Thư viện Anti-Detect chuyên dụng**: Tích hợp `playwright-stealth` và các evasions cấp trình duyệt giúp vượt qua mọi hệ thống kiểm tra bot của Cloudflare / Datadome / Twitter / Republic.

---

## 1. Luồng Nghiệp Vụ Tự Động Hóa

- **Trình duyệt mục tiêu**: Google Chrome (Tự động nhận diện các Chrome profile sẵn có).
- **Nền tảng hỗ trợ**: Republic Legion (`republic.legion.cc`) & X (Twitter).
- **Quy trình chuẩn thực hiện mỗi Raid**:
  1. Mở trang `https://republic.legion.cc/raids`.
  2. Bấm vào thẻ Raid mục tiêu (ví dụ: `@TowerExchange`, `@carrotfunding`, v.v.).
  3. Bấm nút **Equip Supplies & Join (-5 VP)**.
  4. Mở bài viết X (`Open post`), thực hiện:
     - Like bài viết (trái tim đổi sang màu hồng `#f91880`).
     - Repost bài viết (icon chuyển sang màu xanh lá `#00ba7c`).
     - Chụp ảnh màn hình làm **Bằng chứng 1** (cho mục Like & Repost).
     - Bấm Copy link, xuất hiện thanh thông báo nổi *"Copied to clipboard"*.
     - Chụp ảnh màn hình làm **Bằng chứng 2** (cho mục Copy link).
  5. Quay lại popup trên Republic:
     - Tích chọn checkbox `Like +2 VP` và `Repost +3 VP` -> Tải lên ảnh bằng chứng 1.
     - Tích chọn checkbox `Copy link +5 VP` -> Tải lên ảnh bằng chứng 2.
     - Bấm **Submit proof**.
  6. Thẻ raid chuyển sang trạng thái đã gửi bằng chứng (*"Proof submitted. Awaiting review."*).

---

## 2. Hỗ Trợ Proxy (Webshare, SOCKS5, HTTP)

Hỗ trợ nạp file proxy theo định dạng tiêu chuẩn:
- **Định dạng**: `IP:PORT:USERNAME:PASSWORD` hoặc `IP:PORT` (Ví dụ: `89.249.196.100:6685:user:password`).
- Tự động phân bổ đều danh sách Proxy vào các Profile Chrome được tích chọn.
- Tích hợp kiểm tra IP ngoại mạng và độ trễ kết nối trực tiếp trên từng Profile trước khi bắt đầu chạy.

---

## 3. Cấu Trúc Thư Mục Dự Án

```
d:\region tool\
├── republic_raid_bot.py      # Core script Python tự động hóa bot, Đa luồng, Proxy, Anti-detect
├── run_ui.bat                # Khởi chạy giao diện WinForms 1-click
├── run.bat                   # Khởi chạy bot qua Terminal CLI
├── requirements.txt          # Các thư viện Python cần thiết
├── README.md                 # Tài liệu hướng dẫn sử dụng
├── screenshots\              # Toàn bộ ảnh chụp màn hình các bước và ảnh bằng chứng
└── RepublicRaidUI\           # Project WinForms .NET 10
    ├── Form1.cs              # Mã logic xử lý đa luồng, nạp proxy, test proxy, live stream log
    ├── Form1.Designer.cs     # Thiết kế giao diện WinForms hiện đại
    ├── Program.cs
    └── bin\Release\net10.0-windows\
        └── RepublicRaidUI.exe # File chạy ứng dụng WinForms hoàn chỉnh
```

---

## 4. Hướng Dẫn Sử Dụng

### Cách 1: Sử Dụng Giao Diện Đồ Họa WinForms (Khuyên dùng)
1. Nhấp đúp vào file `d:\region tool\run_ui.bat` (hoặc mở trực tiếp `d:\region tool\RepublicRaidUI\bin\Release\net10.0-windows\RepublicRaidUI.exe`).
2. **Nạp & Cấu hình Proxy tự động**:
   - Bấm nút **`📂 Nạp file Proxy`** -> Chọn file proxy của bạn (ví dụ file text danh sách proxy Webshare).
   - Tool sẽ tự động gán proxy lần lượt cho các profile trong bảng.
   - Bấm nút **`⚡ Test Proxy`** để kiểm tra ping và IP ngoại mạng trực tiếp trên từng hàng.
3. **Cấu hình Luồng & Chọn Profile**:
   - Tích chọn các Profile muốn chạy (hoặc bấm `☑ Chọn hết`).
   - Điều chỉnh ô **Số luồng** (ví dụ: `2` hoặc `3` luồng đồng thời).
4. **Chọn 1 Target Raid**:
   - Nhấn nút **`🔄 Tải danh sách Raid`**.
   - Bấm chọn **1 dòng Raid mục tiêu** trong bảng (ví dụ `STT #4 - @TowerExchange`).
   - Thông báo hiển thị: *"🎯 TARGET CHUNG: STT #4 - @TowerExchange..."*.
5. **Bắt đầu**:
   - Nhấn nút **`🚀 BẮT ĐẦU CHẠY ĐA LUỒNG`**.
   - Theo dõi tiến trình song song qua Live Log và ảnh chụp cập nhật tự động bên phải.

---

### Cách 2: Sử Dụng Dòng Lệnh (CLI)

```powershell
# 1. Kiểm tra 1 proxy cụ thể:
python republic_raid_bot.py --test-proxy "IP:PORT:USERNAME:PASSWORD"

# 2. Chạy đa luồng nhiều Profile với proxy cùng làm 1 Target Raid (STT 4):
python republic_raid_bot.py --profiles-json "[{\"profile\": \"Default\", \"proxy\": \"ip:port:user:pass\"}, {\"profile\": \"Profile 1\", \"proxy\": \"ip:port:user:pass\"}]" --stt 4 --threads 2 --loop 1
```

---

## 5. Thư Mục Bằng Chứng & Ảnh Chụp Màn Hình

Tất cả ảnh chụp tiến trình đều được lưu tại:
`d:\region tool\screenshots\`

Được phân loại theo tên profile và target:
`{timestamp}_{profile}_raid_{index}_{action}.png`
Ví dụ:
- `Default_raid_3_01_raid_protocol_opened.png`
- `Default_raid_3_03_proofs_attached.png`
- `Profile_1_raid_3_04_submitted_proof.png`
- `proof1_like_repost_Default_raid_3.png`
- `proof2_copylink_Default_raid_3.png`
