using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RepublicRaidUI;

public partial class Form1 : Form
{
    private string pythonPath = @"C:\Python313\python.exe";
    private string scriptPath = "";
    private string screenshotsDir = "";
    private Process? activeProcess = null;
    private FileSystemWatcher? fileWatcher = null;

    private int? selectedSTT = null;
    private int? selectedCardIndex = null;
    private string? selectedTarget = null;
    private List<ChromeProfileInfo> detectedProfiles = new();

    public Form1()
    {
        InitializeComponent();
    }

    private async void Form1_Load(object sender, EventArgs e)
    {
        // Xác định đường dẫn file script và thư mục ảnh
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string candidate1 = Path.Combine(baseDir, "..", "..", "..", "..", "republic_raid_bot.py");
        string candidate2 = Path.Combine(baseDir, "republic_raid_bot.py");
        string candidate3 = @"d:\region tool\republic_raid_bot.py";

        if (File.Exists(candidate3))
            scriptPath = Path.GetFullPath(candidate3);
        else if (File.Exists(candidate1))
            scriptPath = Path.GetFullPath(candidate1);
        else if (File.Exists(candidate2))
            scriptPath = Path.GetFullPath(candidate2);
        else
            scriptPath = "republic_raid_bot.py";

        string projectDir = Path.GetDirectoryName(scriptPath) ?? @"d:\region tool";
        screenshotsDir = Path.Combine(projectDir, "screenshots");
        if (!Directory.Exists(screenshotsDir))
        {
            Directory.CreateDirectory(screenshotsDir);
        }

        // Kiểm tra python
        if (!File.Exists(pythonPath))
        {
            pythonPath = "python";
        }

        AppendLog($"[HỆ THỐNG] Khởi tạo giao diện Đa Luồng thành công.", Color.LightGreen);
        AppendLog($"[HỆ THỐNG] Bot script: {scriptPath}", Color.LightSteelBlue);
        AppendLog($"[HỆ THỐNG] Thư mục ảnh minh chứng: {screenshotsDir}", Color.LightSteelBlue);
        AppendLog($"[HỆ THỐNG] Thư viện Anti-Detect: Sẵn sàng (playwright-stealth active).", Color.LightGreen);

        InitScreenshotWatcher();
        LoadLatestScreenshot();

        // Tự động quét các Chrome Profile khi mở ứng dụng
        await ScanChromeProfilesAsync();
    }

    public async Task ScanChromeProfilesAsync()
    {
        btnScanProfiles.Enabled = false;
        lblProfileSuggestion.Text = "⏳ Đang quét tất cả Profile Chrome để tìm cookie Republic Legion & X...";
        lblProfileSuggestion.ForeColor = Color.DarkOrange;
        AppendLog("\n[PROFILE SCAN] Đang quét các Profile Chrome trên máy tính...", Color.Cyan);

        try
        {
            string output = await Task.Run(() =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = $"\"{scriptPath}\" --scan-profiles",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using var proc = Process.Start(psi);
                if (proc == null) return "";

                StringBuilder sb = new StringBuilder();
                proc.OutputDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        sb.AppendLine(ev.Data);
                        AppendLog(ev.Data);
                    }
                };
                proc.ErrorDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        sb.AppendLine(ev.Data);
                        AppendLog(ev.Data, Color.LightCoral);
                    }
                };

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                return sb.ToString();
            });

            int startIdx = output.IndexOf("###PROFILES_JSON_START###");
            int endIdx = output.IndexOf("###PROFILES_JSON_END###");

            if (startIdx >= 0 && endIdx > startIdx)
            {
                startIdx += "###PROFILES_JSON_START###".Length;
                string jsonStr = output.Substring(startIdx, endIdx - startIdx).Trim();

                var scanResult = JsonSerializer.Deserialize<ScanProfilesResult>(jsonStr, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (scanResult != null && scanResult.profiles != null && scanResult.profiles.Count > 0)
                {
                    detectedProfiles = scanResult.profiles;
                    dgvProfiles.Rows.Clear();

                    int bestIndex = 0;
                    for (int i = 0; i < detectedProfiles.Count; i++)
                    {
                        var prof = detectedProfiles[i];
                        bool autoCheck = prof.is_recommended || prof.has_republic || (i == 0);

                        int rowIdx = dgvProfiles.Rows.Add(
                            autoCheck,
                            prof.directory,
                            prof.name,
                            prof.user_name,
                            prof.status_text,
                            "", // Ô nhập Proxy
                            "Sẵn sàng"
                        );

                        var row = dgvProfiles.Rows[rowIdx];
                        if (prof.is_recommended)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(240, 253, 244);
                            row.Cells[4].Style.ForeColor = Color.DarkGreen;
                            bestIndex = i;
                        }
                        else if (prof.has_republic)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(240, 249, 255);
                            row.Cells[4].Style.ForeColor = Color.DarkBlue;
                        }
                    }

                    UpdateProfileSummary();

                    var bestProf = detectedProfiles[bestIndex];
                    if (bestProf.is_recommended)
                    {
                        lblProfileSuggestion.Text = $"⭐ GỢI Ý: Profile '{bestProf.directory}' ({bestProf.name}) có đầy đủ cả Republic Legion & X! Khuyên dùng.";
                        lblProfileSuggestion.ForeColor = Color.FromArgb(22, 101, 52);
                    }
                    else if (bestProf.has_republic)
                    {
                        lblProfileSuggestion.Text = $"💡 GỢI Ý: Profile '{bestProf.directory}' đã có cookie Republic Legion. Hãy mở profile này đăng nhập thêm X!";
                        lblProfileSuggestion.ForeColor = Color.FromArgb(3, 105, 161);
                    }
                    else
                    {
                        lblProfileSuggestion.Text = scanResult.suggestion;
                        lblProfileSuggestion.ForeColor = Color.FromArgb(180, 83, 9);
                    }

                    AppendLog($"[QUÉT PROFILE HOÀN TẤT] Tìm thấy {detectedProfiles.Count} Profile Chrome.", Color.LightGreen);
                }
            }
            else
            {
                lblProfileSuggestion.Text = "Không đọc được dữ liệu profile từ bot.";
                lblProfileSuggestion.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblProfileSuggestion.Text = $"Lỗi khi quét profile: {ex.Message}";
            lblProfileSuggestion.ForeColor = Color.Red;
            AppendLog($"[LỖI QUÉT PROFILE] {ex.Message}", Color.Red);
        }
        finally
        {
            btnScanProfiles.Enabled = true;
        }
    }

    private void UpdateProfileSummary()
    {
        int checkedCount = 0;
        foreach (DataGridViewRow row in dgvProfiles.Rows)
        {
            if (Convert.ToBoolean(row.Cells[0].Value))
            {
                checkedCount++;
            }
        }
        lblProfileSummary.Text = $"Đã chọn: {checkedCount}/{dgvProfiles.Rows.Count} profile | Luồng: {numThreads.Value}";
        statusProfile.Text = $"Đã chọn {checkedCount} Profile | Chạy {numThreads.Value} Luồng đồng thời";
    }

    private void btnScanProfiles_Click(object sender, EventArgs e)
    {
        _ = ScanChromeProfilesAsync();
    }

    private void btnSelectAll_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dgvProfiles.Rows)
        {
            row.Cells[0].Value = true;
        }
        UpdateProfileSummary();
    }

    private void btnUnselectAll_Click(object sender, EventArgs e)
    {
        foreach (DataGridViewRow row in dgvProfiles.Rows)
        {
            row.Cells[0].Value = false;
        }
        UpdateProfileSummary();
    }

    private void btnImportProxies_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog
        {
            Title = "Chọn file danh sách Proxy (Webshare, Text file...)",
            Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\Downloads"
        };

        if (ofd.ShowDialog() == DialogResult.OK)
        {
            try
            {
                var lines = File.ReadAllLines(ofd.FileName)
                    .Select(l => l.Trim())
                    .Where(l => !string.IsNullOrEmpty(l) && !l.StartsWith("#"))
                    .ToList();

                if (lines.Count == 0)
                {
                    MessageBox.Show("File proxy không có dòng hợp lệ nào!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int assigned = 0;
                for (int i = 0; i < dgvProfiles.Rows.Count; i++)
                {
                    string p = lines[i % lines.Count];
                    dgvProfiles.Rows[i].Cells[5].Value = p;
                    assigned++;
                }

                AppendLog($"[PROXY] Đã nạp thành công {lines.Count} proxy từ file: {Path.GetFileName(ofd.FileName)}", Color.LightGreen);
                MessageBox.Show($"Đã nạp {lines.Count} proxy từ file và tự động gán vào các Profile trong bảng!\n\nVí dụ proxy đầu tiên: {lines[0]}", 
                    "Nạp Proxy thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đọc file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private async void btnTestProxies_Click(object sender, EventArgs e)
    {
        btnTestProxies.Enabled = false;
        AppendLog("\n[PROXY TEST] Đang kiểm tra kết nối các proxy trong bảng...", Color.Cyan);

        try
        {
            for (int i = 0; i < dgvProfiles.Rows.Count; i++)
            {
                var row = dgvProfiles.Rows[i];
                string prx = row.Cells[5].Value?.ToString()?.Trim() ?? "";
                string prof = row.Cells[1].Value?.ToString() ?? $"Profile {i + 1}";

                if (string.IsNullOrEmpty(prx))
                {
                    row.Cells[6].Value = "IP Máy (Không proxy)";
                    row.Cells[6].Style.ForeColor = Color.DarkSlateGray;
                    continue;
                }

                row.Cells[6].Value = "Đang kiểm tra...";
                row.Cells[6].Style.ForeColor = Color.DarkOrange;

                string resText = await Task.Run(() =>
                {
                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = pythonPath,
                            Arguments = $"\"{scriptPath}\" --test-proxy \"{prx}\"",
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true,
                            StandardOutputEncoding = Encoding.UTF8
                        };

                        using var proc = Process.Start(psi);
                        if (proc == null) return "Lỗi khởi chạy";
                        string outStr = proc.StandardOutput.ReadToEnd();
                        proc.WaitForExit();

                        foreach (var line in outStr.Split('\n'))
                        {
                            if (line.Contains("[PROXY TEST THÀNH CÔNG]"))
                            {
                                int idx = line.IndexOf("-> ");
                                return idx >= 0 ? line.Substring(idx + 3).Trim() : "Hoạt động tốt";
                            }
                            if (line.Contains("[PROXY TEST THẤT BẠI]") || line.Contains("[PROXY TEST LỖI]"))
                            {
                                int idx = line.IndexOf("-> ");
                                return idx >= 0 ? line.Substring(idx + 3).Trim() : "Lỗi kết nối";
                            }
                        }
                        return "Không xác định";
                    }
                    catch (Exception ex)
                    {
                        return $"Lỗi: {ex.Message}";
                    }
                });

                row.Cells[6].Value = resText;
                if (resText.Contains("Hoạt động tốt") || resText.Contains("IP:"))
                {
                    row.Cells[6].Style.ForeColor = Color.DarkGreen;
                    AppendLog($"[{prof}] Proxy OK: {resText}", Color.LightGreen);
                }
                else
                {
                    row.Cells[6].Style.ForeColor = Color.Red;
                    AppendLog($"[{prof}] Proxy Lỗi: {resText}", Color.Red);
                }
            }
        }
        finally
        {
            btnTestProxies.Enabled = true;
            AppendLog("[PROXY TEST] Kiểm tra hoàn tất.\n", Color.LightGreen);
        }
    }

    private void dgvProfiles_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
        if (dgvProfiles.IsCurrentCellDirty && dgvProfiles.CurrentCell is DataGridViewCheckBoxCell)
        {
            dgvProfiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }

    private void dgvProfiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex == 0)
        {
            UpdateProfileSummary();
        }
    }

    private void InitScreenshotWatcher()
    {
        try
        {
            if (Directory.Exists(screenshotsDir))
            {
                fileWatcher = new FileSystemWatcher(screenshotsDir, "*.png")
                {
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
                    EnableRaisingEvents = true
                };

                fileWatcher.Created += (s, ev) =>
                {
                    Task.Delay(300).ContinueWith(_ => UpdatePreviewImage(ev.FullPath));
                };

                fileWatcher.Changed += (s, ev) =>
                {
                    Task.Delay(300).ContinueWith(_ => UpdatePreviewImage(ev.FullPath));
                };
            }
        }
        catch (Exception ex)
        {
            AppendLog($"[CẢNH BÁO] Không thể khởi động FileSystemWatcher: {ex.Message}", Color.Orange);
        }
    }

    private void LoadLatestScreenshot()
    {
        try
        {
            if (Directory.Exists(screenshotsDir))
            {
                var dir = new DirectoryInfo(screenshotsDir);
                var latestFile = dir.GetFiles("*.png")
                    .OrderByDescending(f => f.LastWriteTime)
                    .FirstOrDefault();

                if (latestFile != null)
                {
                    UpdatePreviewImage(latestFile.FullName);
                }
            }
        }
        catch { }
    }

    private void UpdatePreviewImage(string filePath)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => UpdatePreviewImage(filePath)));
            return;
        }

        try
        {
            if (File.Exists(filePath))
            {
                byte[] bytes = File.ReadAllBytes(filePath);
                using (var ms = new MemoryStream(bytes))
                {
                    var oldImg = picPreview.Image;
                    picPreview.Image = Image.FromStream(ms);
                    oldImg?.Dispose();
                }
                lblPreviewName.Text = $"{Path.GetFileName(filePath)} ({DateTime.Now:HH:mm:ss})";
            }
        }
        catch { }
    }

    public void AppendLog(string message, Color? color = null)
    {
        if (InvokeRequired)
        {
            BeginInvoke(new Action(() => AppendLog(message, color)));
            return;
        }

        rtbLogs.SelectionStart = rtbLogs.TextLength;
        rtbLogs.SelectionLength = 0;
        rtbLogs.SelectionColor = color ?? Color.FromArgb(226, 232, 240);
        rtbLogs.AppendText(message + Environment.NewLine);
        rtbLogs.SelectionColor = rtbLogs.ForeColor;
        rtbLogs.ScrollToCaret();
    }

    private async void btnFetch_Click(object sender, EventArgs e)
    {
        // Lấy profile đầu tiên được chọn hoặc Default để fetch danh sách thẻ
        string fetchProfile = "Default";
        foreach (DataGridViewRow row in dgvProfiles.Rows)
        {
            if (Convert.ToBoolean(row.Cells[0].Value))
            {
                fetchProfile = row.Cells[1].Value?.ToString() ?? "Default";
                break;
            }
        }

        btnFetch.Enabled = false;
        btnStart.Enabled = false;
        statusProgress.Visible = true;
        statusLabel.Text = $"Đang tải danh sách Raids qua Profile '{fetchProfile}'...";
        AppendLog($"\n[FETCH] Khởi chạy bot lấy danh sách Raids với Profile: {fetchProfile}...", Color.Cyan);

        dgvRaids.Rows.Clear();

        try
        {
            string output = await Task.Run(() =>
            {
                var psi = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = $"\"{scriptPath}\" --fetch --profile \"{fetchProfile}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                using var proc = Process.Start(psi);
                if (proc == null) return "";

                StringBuilder sb = new StringBuilder();
                proc.OutputDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        sb.AppendLine(ev.Data);
                        AppendLog(ev.Data);
                    }
                };
                proc.ErrorDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        sb.AppendLine(ev.Data);
                        AppendLog(ev.Data, Color.LightCoral);
                    }
                };

                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
                proc.WaitForExit();
                return sb.ToString();
            });

            int startIdx = output.IndexOf("###RAIDS_JSON_START###");
            int endIdx = output.IndexOf("###RAIDS_JSON_END###");

            if (startIdx >= 0 && endIdx > startIdx)
            {
                startIdx += "###RAIDS_JSON_START###".Length;
                string jsonStr = output.Substring(startIdx, endIdx - startIdx).Trim();

                var raids = JsonSerializer.Deserialize<List<RaidModel>>(jsonStr, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (raids != null && raids.Count > 0)
                {
                    foreach (var r in raids)
                    {
                        int rowIdx = dgvRaids.Rows.Add(
                            r.stt,
                            r.target,
                            r.status,
                            r.time_left,
                            r.raiders,
                            r.card_index,
                            r.raw
                        );

                        var row = dgvRaids.Rows[rowIdx];
                        if (r.status == "COMPLETED")
                        {
                            row.DefaultCellStyle.ForeColor = Color.Gray;
                        }
                        else if (r.status == "LIVE")
                        {
                            row.DefaultCellStyle.ForeColor = Color.DarkGreen;
                            row.DefaultCellStyle.Font = new Font(dgvRaids.Font, FontStyle.Bold);
                        }
                        else if (r.status == "PROOF NEEDED" || r.status == "NEEDS UPDATES")
                        {
                            row.DefaultCellStyle.ForeColor = Color.DarkOrange;
                            row.DefaultCellStyle.Font = new Font(dgvRaids.Font, FontStyle.Bold);
                        }
                    }

                    AppendLog($"[FETCH THÀNH CÔNG] Đã nạp {raids.Count} Raid vào bảng theo số thứ tự 1, 2, 3...!", Color.LightGreen);
                    statusLabel.Text = $"Đã tải xong {raids.Count} Raid.";

                    for (int i = 0; i < dgvRaids.Rows.Count; i++)
                    {
                        string st = dgvRaids.Rows[i].Cells[2].Value?.ToString() ?? "";
                        if (st != "COMPLETED" && st != "MISSED")
                        {
                            dgvRaids.ClearSelection();
                            dgvRaids.Rows[i].Selected = true;
                            break;
                        }
                    }
                }
                else
                {
                    AppendLog("[CẢNH BÁO] Không tìm thấy dữ liệu thẻ raid nào từ kết quả fetch.", Color.Yellow);
                    statusLabel.Text = "Không có raid nào khả dụng.";
                }
            }
            else
            {
                AppendLog("[LỖI] Không tìm thấy khối JSON dữ liệu trong kết quả trả về của bot.", Color.Red);
                statusLabel.Text = "Lỗi đọc danh sách raid.";
            }
        }
        catch (Exception ex)
        {
            AppendLog($"[LỖI FETCH] {ex.Message}", Color.Red);
            statusLabel.Text = "Lỗi khi tải danh sách.";
        }
        finally
        {
            btnFetch.Enabled = true;
            btnStart.Enabled = true;
            statusProgress.Visible = false;
            LoadLatestScreenshot();
        }
    }

    private void dgvRaids_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvRaids.SelectedRows.Count > 0)
        {
            var row = dgvRaids.SelectedRows[0];
            if (row.Cells[0].Value != null && int.TryParse(row.Cells[0].Value?.ToString(), out int stt))
            {
                selectedSTT = stt;
                selectedTarget = row.Cells[1].Value?.ToString() ?? "Unknown";
                string status = row.Cells[2].Value?.ToString() ?? "";
                if (int.TryParse(row.Cells[5].Value?.ToString(), out int cardIdx))
                {
                    selectedCardIndex = cardIdx;
                }

                lblSelectedTarget.Text = $"🎯 TARGET CHUNG: STT #{selectedSTT} - {selectedTarget} [{status}] (Tất cả profiles đã chọn sẽ cùng làm raid này)";
                lblSelectedTarget.ForeColor = Color.FromArgb(14, 116, 144);
            }
        }
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
        // 1. Kiểm tra profiles đã chọn
        var selectedProfiles = new List<object>();
        foreach (DataGridViewRow row in dgvProfiles.Rows)
        {
            bool isChecked = Convert.ToBoolean(row.Cells[0].Value);
            if (isChecked)
            {
                string dir = row.Cells[1].Value?.ToString() ?? "";
                string proxy = row.Cells[5].Value?.ToString()?.Trim() ?? "";
                selectedProfiles.Add(new { profile = dir, proxy = proxy });
            }
        }

        if (selectedProfiles.Count == 0)
        {
            MessageBox.Show("Vui lòng tích chọn ít nhất 1 Profile trong danh sách trước khi bắt đầu!", 
                "Chưa chọn Profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // 2. Kiểm tra Target Raid đã chọn
        if (selectedSTT == null)
        {
            MessageBox.Show("Vui lòng nhấn 'Tải danh sách Raid' và bấm chọn 1 dòng Target Raid trong bảng dưới trước khi bắt đầu!", 
                "Chưa chọn Target Raid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int threads = (int)numThreads.Value;
        int loop = (int)numLoop.Value;
        int delay = (int)numDelay.Value;

        // 3. Ghi file JSON cấu hình profiles & proxies
        string tempJsonPath = Path.Combine(Path.GetTempPath(), "republic_multithread_profiles.json");
        try
        {
            string jsonContent = JsonSerializer.Serialize(selectedProfiles, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(tempJsonPath, jsonContent, new UTF8Encoding(false));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể tạo file cấu hình tạm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        StringBuilder args = new StringBuilder();
        args.Append($"\"{scriptPath}\"");
        args.Append($" --profiles-json \"{tempJsonPath}\"");
        args.Append($" --stt {selectedSTT.Value}");
        if (!string.IsNullOrEmpty(selectedTarget))
        {
            args.Append($" --target-name \"{selectedTarget}\"");
        }
        args.Append($" --threads {threads}");
        args.Append($" --loop {loop}");
        args.Append($" --delay {delay}");

        AppendLog($"\n=======================================================", Color.LimeGreen);
        AppendLog($"[BẮT ĐẦU ĐA LUỒNG] Số Profile: {selectedProfiles.Count} | Luồng đồng thời: {threads}", Color.LimeGreen);
        AppendLog($"[TARGET CHUNG] STT #{selectedSTT.Value} ({selectedTarget}) | Lặp: {loop} lần | Delay: {delay}s", Color.LimeGreen);
        AppendLog($"=======================================================", Color.LimeGreen);

        btnStart.Enabled = false;
        btnFetch.Enabled = false;
        btnStop.Enabled = true;
        statusProgress.Visible = true;
        statusLabel.Text = $"Đang chạy Đa luồng ({selectedProfiles.Count} profiles, {threads} luồng)...";

        Task.Run(() =>
        {
            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName = pythonPath,
                    Arguments = args.ToString(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                };

                activeProcess = Process.Start(psi);
                if (activeProcess == null)
                {
                    AppendLog("[LỖI] Không thể khởi chạy tiến trình Python!", Color.Red);
                    return;
                }

                activeProcess.OutputDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        Color c = Color.FromArgb(226, 232, 240);
                        if (ev.Data.Contains("[ERROR]")) c = Color.Red;
                        else if (ev.Data.Contains("[WARN]")) c = Color.Orange;
                        else if (ev.Data.Contains("THÀNH CÔNG") || ev.Data.Contains("HOÀN TẤT") || ev.Data.Contains("COMPLETED")) c = Color.LightGreen;
                        else if (ev.Data.Contains("ĐA LUỒNG") || ev.Data.Contains("TARGET")) c = Color.Cyan;
                        else if (ev.Data.Contains("Proxy:")) c = Color.Yellow;
                        AppendLog(ev.Data, c);
                    }
                };

                activeProcess.ErrorDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        AppendLog(ev.Data, Color.LightCoral);
                    }
                };

                activeProcess.BeginOutputReadLine();
                activeProcess.BeginErrorReadLine();
                activeProcess.WaitForExit();
                int exitCode = activeProcess.ExitCode;

                BeginInvoke(new Action(() =>
                {
                    AppendLog($"\n[KẾT THÚC] Tiến trình đa luồng kết thúc với mã thoát: {exitCode}", exitCode == 0 ? Color.LightGreen : Color.Orange);
                    statusLabel.Text = exitCode == 0 ? "Hoàn tất nhiệm vụ đa luồng!" : "Tiến trình kết thúc có cảnh báo.";
                }));
            }
            catch (Exception ex)
            {
                AppendLog($"[LỖI THỰC THI] {ex.Message}", Color.Red);
                BeginInvoke(new Action(() =>
                {
                    statusLabel.Text = "Lỗi thực thi.";
                }));
            }
            finally
            {
                activeProcess = null;
                BeginInvoke(new Action(() =>
                {
                    btnStart.Enabled = true;
                    btnFetch.Enabled = true;
                    btnStop.Enabled = false;
                    statusProgress.Visible = false;
                    LoadLatestScreenshot();
                }));
            }
        });
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        try
        {
            if (activeProcess != null && !activeProcess.HasExited)
            {
                AppendLog("[HỆ THỐNG] Đang dừng tất cả các tiến trình theo lệnh người dùng...", Color.Yellow);
                activeProcess.Kill(true);
            }

            // Dừng tất cả Chrome
            Process.Start(new ProcessStartInfo
            {
                FileName = "powershell",
                Arguments = "-Command Stop-Process -Name chrome -Force -ErrorAction SilentlyContinue",
                CreateNoWindow = true,
                UseShellExecute = false
            })?.WaitForExit(2000);

            AppendLog("[HỆ THỐNG] Đã dừng toàn bộ luồng thành công.", Color.LightGreen);
            statusLabel.Text = "Đã dừng.";
        }
        catch (Exception ex)
        {
            AppendLog($"[LỖI KHI DỪNG] {ex.Message}", Color.Red);
        }
        finally
        {
            btnStart.Enabled = true;
            btnFetch.Enabled = true;
            btnStop.Enabled = false;
            statusProgress.Visible = false;
        }
    }

    private void btnClearLogs_Click(object sender, EventArgs e)
    {
        rtbLogs.Clear();
    }

    private void btnOpenScreenshots_Click(object sender, EventArgs e)
    {
        try
        {
            if (Directory.Exists(screenshotsDir))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer.exe",
                    Arguments = $"\"{screenshotsDir}\"",
                    UseShellExecute = true
                });
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Không thể mở thư mục: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            fileWatcher?.Dispose();
            if (activeProcess != null && !activeProcess.HasExited)
            {
                activeProcess.Kill(true);
            }
        }
        catch { }
    }
}

public class ScanProfilesResult
{
    public string suggestion { get; set; } = "";
    public List<ChromeProfileInfo> profiles { get; set; } = new();
}

public class ChromeProfileInfo
{
    public string directory { get; set; } = "";
    public string name { get; set; } = "";
    public string user_name { get; set; } = "";
    public bool has_republic { get; set; }
    public bool has_x { get; set; }
    public bool has_republic_hist { get; set; }
    public bool has_x_hist { get; set; }
    public bool is_recommended { get; set; }
    public string status_text { get; set; } = "";
    public List<string> republic_cookies { get; set; } = new();
    public List<string> x_cookies { get; set; } = new();
}

public class RaidModel
{
    public int stt { get; set; }
    public int card_index { get; set; }
    public string target { get; set; } = "";
    public string time_left { get; set; } = "";
    public string raiders { get; set; } = "";
    public string status { get; set; } = "";
    public bool can_run { get; set; }
    public string raw { get; set; } = "";
}
