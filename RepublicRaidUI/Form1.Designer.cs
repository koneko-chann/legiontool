namespace RepublicRaidUI;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        System.Windows.Forms.DataGridViewCellStyle dgvStyleAlt1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dgvStyleHeader1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dgvStyleRow1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dgvStyleAlt2 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dgvStyleHeader2 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dgvStyleRow2 = new System.Windows.Forms.DataGridViewCellStyle();

        panelHeader = new System.Windows.Forms.Panel();
        lblAntiDetect = new System.Windows.Forms.Label();
        lblSubtitle = new System.Windows.Forms.Label();
        lblTitle = new System.Windows.Forms.Label();

        splitVertical = new System.Windows.Forms.SplitContainer();
        panelUpper = new System.Windows.Forms.Panel();
        grpRaids = new System.Windows.Forms.GroupBox();
        dgvRaids = new System.Windows.Forms.DataGridView();
        colSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colTarget = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colTimeLeft = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colRaiders = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colCardIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colRaw = new System.Windows.Forms.DataGridViewTextBoxColumn();

        panelActionControls = new System.Windows.Forms.Panel();
        lblSelectedTarget = new System.Windows.Forms.Label();
        btnFetch = new System.Windows.Forms.Button();
        lblThreads = new System.Windows.Forms.Label();
        numThreads = new System.Windows.Forms.NumericUpDown();
        lblLoop = new System.Windows.Forms.Label();
        numLoop = new System.Windows.Forms.NumericUpDown();
        lblDelay = new System.Windows.Forms.Label();
        numDelay = new System.Windows.Forms.NumericUpDown();
        btnStart = new System.Windows.Forms.Button();
        btnStop = new System.Windows.Forms.Button();
        btnOpenScreenshots = new System.Windows.Forms.Button();

        grpProfiles = new System.Windows.Forms.GroupBox();
        dgvProfiles = new System.Windows.Forms.DataGridView();
        colProfSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
        colProfDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colProfName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colProfGoogle = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colProfCookies = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colProfProxy = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colProfStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();

        panelProfileToolbar = new System.Windows.Forms.Panel();
        lblProfileSuggestion = new System.Windows.Forms.Label();
        lblProfileSummary = new System.Windows.Forms.Label();
        btnScanProfiles = new System.Windows.Forms.Button();
        btnImportProxies = new System.Windows.Forms.Button();
        btnTestProxies = new System.Windows.Forms.Button();
        btnUnselectAll = new System.Windows.Forms.Button();
        btnSelectAll = new System.Windows.Forms.Button();

        splitBottom = new System.Windows.Forms.SplitContainer();
        grpLogs = new System.Windows.Forms.GroupBox();
        btnClearLogs = new System.Windows.Forms.Button();
        rtbLogs = new System.Windows.Forms.RichTextBox();
        grpPreview = new System.Windows.Forms.GroupBox();
        lblPreviewName = new System.Windows.Forms.Label();
        picPreview = new System.Windows.Forms.PictureBox();

        statusStrip1 = new System.Windows.Forms.StatusStrip();
        statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
        statusProgress = new System.Windows.Forms.ToolStripProgressBar();
        statusProfile = new System.Windows.Forms.ToolStripStatusLabel();

        panelHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitVertical).BeginInit();
        splitVertical.Panel1.SuspendLayout();
        splitVertical.Panel2.SuspendLayout();
        splitVertical.SuspendLayout();
        panelUpper.SuspendLayout();
        grpRaids.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvRaids).BeginInit();
        panelActionControls.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numThreads).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numLoop).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDelay).BeginInit();
        grpProfiles.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvProfiles).BeginInit();
        panelProfileToolbar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitBottom).BeginInit();
        splitBottom.Panel1.SuspendLayout();
        splitBottom.Panel2.SuspendLayout();
        splitBottom.SuspendLayout();
        grpLogs.SuspendLayout();
        grpPreview.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picPreview).BeginInit();
        statusStrip1.SuspendLayout();
        SuspendLayout();

        // 
        // panelHeader
        // 
        panelHeader.BackColor = System.Drawing.Color.FromArgb(24, 30, 42);
        panelHeader.Controls.Add(lblAntiDetect);
        panelHeader.Controls.Add(lblSubtitle);
        panelHeader.Controls.Add(lblTitle);
        panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
        panelHeader.Location = new System.Drawing.Point(0, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new System.Drawing.Size(1264, 65);
        panelHeader.TabIndex = 0;

        // 
        // lblAntiDetect
        // 
        lblAntiDetect.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        lblAntiDetect.BackColor = System.Drawing.Color.FromArgb(16, 75, 42);
        lblAntiDetect.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        lblAntiDetect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblAntiDetect.ForeColor = System.Drawing.Color.LightGreen;
        lblAntiDetect.Location = new System.Drawing.Point(1065, 16);
        lblAntiDetect.Name = "lblAntiDetect";
        lblAntiDetect.Size = new System.Drawing.Size(185, 32);
        lblAntiDetect.TabIndex = 2;
        lblAntiDetect.Text = "🛡️ Anti-Detect Active";
        lblAntiDetect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoSize = true;
        lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblSubtitle.ForeColor = System.Drawing.Color.LightSteelBlue;
        lblSubtitle.Location = new System.Drawing.Point(16, 38);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new System.Drawing.Size(760, 15);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Hỗ trợ Chọn Nhiều Profile | Chạy Đa Luồng Độc Lập | Cấu hình Proxy riêng biệt | Chung 1 Target Raid | Chụp màn hình đầy đủ";

        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblTitle.ForeColor = System.Drawing.Color.White;
        lblTitle.Location = new System.Drawing.Point(14, 10);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new System.Drawing.Size(560, 25);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "REPUBLIC LEGION RAID TOOL - MULTI-PROFILE & THREADS";

        // 
        // splitVertical
        // 
        splitVertical.Dock = System.Windows.Forms.DockStyle.Fill;
        splitVertical.Location = new System.Drawing.Point(0, 65);
        splitVertical.Name = "splitVertical";
        splitVertical.Orientation = System.Windows.Forms.Orientation.Horizontal;

        // 
        // splitVertical.Panel1
        // 
        splitVertical.Panel1.Controls.Add(panelUpper);
        splitVertical.Panel1.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);

        // 
        // splitVertical.Panel2
        // 
        splitVertical.Panel2.Controls.Add(splitBottom);
        splitVertical.Panel2.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
        splitVertical.Size = new System.Drawing.Size(1264, 750);
        splitVertical.SplitterDistance = 470;
        splitVertical.TabIndex = 1;

        // 
        // panelUpper
        // 
        panelUpper.Controls.Add(grpRaids);
        panelUpper.Controls.Add(panelActionControls);
        panelUpper.Controls.Add(grpProfiles);
        panelUpper.Dock = System.Windows.Forms.DockStyle.Fill;
        panelUpper.Location = new System.Drawing.Point(8, 4);
        panelUpper.Name = "panelUpper";
        panelUpper.Size = new System.Drawing.Size(1248, 462);
        panelUpper.TabIndex = 0;

        // 
        // grpProfiles
        // 
        grpProfiles.Controls.Add(dgvProfiles);
        grpProfiles.Controls.Add(panelProfileToolbar);
        grpProfiles.Dock = System.Windows.Forms.DockStyle.Top;
        grpProfiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        grpProfiles.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        grpProfiles.Location = new System.Drawing.Point(0, 0);
        grpProfiles.Name = "grpProfiles";
        grpProfiles.Padding = new System.Windows.Forms.Padding(6);
        grpProfiles.Size = new System.Drawing.Size(1248, 195);
        grpProfiles.TabIndex = 0;
        grpProfiles.TabStop = false;
        grpProfiles.Text = "1. Danh sách Profile Chrome & Cấu hình Proxy (Tích chọn các profile cần chạy):";

        // 
        // panelProfileToolbar
        // 
        panelProfileToolbar.BackColor = System.Drawing.Color.FromArgb(241, 245, 249);
        panelProfileToolbar.Controls.Add(lblProfileSuggestion);
        panelProfileToolbar.Controls.Add(lblProfileSummary);
        panelProfileToolbar.Controls.Add(btnTestProxies);
        panelProfileToolbar.Controls.Add(btnImportProxies);
        panelProfileToolbar.Controls.Add(btnScanProfiles);
        panelProfileToolbar.Controls.Add(btnUnselectAll);
        panelProfileToolbar.Controls.Add(btnSelectAll);
        panelProfileToolbar.Dock = System.Windows.Forms.DockStyle.Top;
        panelProfileToolbar.Location = new System.Drawing.Point(6, 22);
        panelProfileToolbar.Name = "panelProfileToolbar";
        panelProfileToolbar.Size = new System.Drawing.Size(1236, 32);
        panelProfileToolbar.TabIndex = 0;

        // 
        // btnSelectAll
        // 
        btnSelectAll.BackColor = System.Drawing.Color.White;
        btnSelectAll.Cursor = System.Windows.Forms.Cursors.Hand;
        btnSelectAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
        btnSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnSelectAll.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        btnSelectAll.Location = new System.Drawing.Point(4, 4);
        btnSelectAll.Name = "btnSelectAll";
        btnSelectAll.Size = new System.Drawing.Size(90, 24);
        btnSelectAll.TabIndex = 0;
        btnSelectAll.Text = "☑ Chọn hết";
        btnSelectAll.UseVisualStyleBackColor = false;
        btnSelectAll.Click += btnSelectAll_Click;

        // 
        // btnUnselectAll
        // 
        btnUnselectAll.BackColor = System.Drawing.Color.White;
        btnUnselectAll.Cursor = System.Windows.Forms.Cursors.Hand;
        btnUnselectAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(203, 213, 225);
        btnUnselectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnUnselectAll.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        btnUnselectAll.Location = new System.Drawing.Point(98, 4);
        btnUnselectAll.Name = "btnUnselectAll";
        btnUnselectAll.Size = new System.Drawing.Size(80, 24);
        btnUnselectAll.TabIndex = 1;
        btnUnselectAll.Text = "☐ Bỏ chọn";
        btnUnselectAll.UseVisualStyleBackColor = false;
        btnUnselectAll.Click += btnUnselectAll_Click;

        // 
        // btnScanProfiles
        // 
        btnScanProfiles.BackColor = System.Drawing.Color.FromArgb(14, 116, 144);
        btnScanProfiles.Cursor = System.Windows.Forms.Cursors.Hand;
        btnScanProfiles.FlatAppearance.BorderSize = 0;
        btnScanProfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnScanProfiles.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        btnScanProfiles.ForeColor = System.Drawing.Color.White;
        btnScanProfiles.Location = new System.Drawing.Point(182, 4);
        btnScanProfiles.Name = "btnScanProfiles";
        btnScanProfiles.Size = new System.Drawing.Size(115, 24);
        btnScanProfiles.TabIndex = 2;
        btnScanProfiles.Text = "🔍 Quét Profile";
        btnScanProfiles.UseVisualStyleBackColor = false;
        btnScanProfiles.Click += btnScanProfiles_Click;

        // 
        // btnImportProxies
        // 
        btnImportProxies.BackColor = System.Drawing.Color.FromArgb(79, 70, 229);
        btnImportProxies.Cursor = System.Windows.Forms.Cursors.Hand;
        btnImportProxies.FlatAppearance.BorderSize = 0;
        btnImportProxies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnImportProxies.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        btnImportProxies.ForeColor = System.Drawing.Color.White;
        btnImportProxies.Location = new System.Drawing.Point(303, 4);
        btnImportProxies.Name = "btnImportProxies";
        btnImportProxies.Size = new System.Drawing.Size(145, 24);
        btnImportProxies.TabIndex = 3;
        btnImportProxies.Text = "📂 Nạp file Proxy";
        btnImportProxies.UseVisualStyleBackColor = false;
        btnImportProxies.Click += btnImportProxies_Click;

        // 
        // btnTestProxies
        // 
        btnTestProxies.BackColor = System.Drawing.Color.FromArgb(16, 122, 60);
        btnTestProxies.Cursor = System.Windows.Forms.Cursors.Hand;
        btnTestProxies.FlatAppearance.BorderSize = 0;
        btnTestProxies.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnTestProxies.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        btnTestProxies.ForeColor = System.Drawing.Color.White;
        btnTestProxies.Location = new System.Drawing.Point(454, 4);
        btnTestProxies.Name = "btnTestProxies";
        btnTestProxies.Size = new System.Drawing.Size(100, 24);
        btnTestProxies.TabIndex = 4;
        btnTestProxies.Text = "⚡ Test Proxy";
        btnTestProxies.UseVisualStyleBackColor = false;
        btnTestProxies.Click += btnTestProxies_Click;

        // 
        // lblProfileSummary
        // 
        lblProfileSummary.AutoSize = true;
        lblProfileSummary.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblProfileSummary.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        lblProfileSummary.Location = new System.Drawing.Point(560, 9);
        lblProfileSummary.Name = "lblProfileSummary";
        lblProfileSummary.Size = new System.Drawing.Size(105, 15);
        lblProfileSummary.TabIndex = 5;
        lblProfileSummary.Text = "Đã chọn: 0 profile";

        // 
        // lblProfileSuggestion
        // 
        lblProfileSuggestion.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        lblProfileSuggestion.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblProfileSuggestion.ForeColor = System.Drawing.Color.FromArgb(16, 75, 42);
        lblProfileSuggestion.Location = new System.Drawing.Point(675, 6);
        lblProfileSuggestion.Name = "lblProfileSuggestion";
        lblProfileSuggestion.Size = new System.Drawing.Size(555, 20);
        lblProfileSuggestion.TabIndex = 6;
        lblProfileSuggestion.Text = "Đang quét các Profile Chrome...";
        lblProfileSuggestion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

        // 
        // dgvProfiles
        // 
        dgvProfiles.AllowUserToAddRows = false;
        dgvProfiles.AllowUserToDeleteRows = false;
        dgvProfiles.AllowUserToResizeRows = false;
        dgvStyleAlt1.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
        dgvProfiles.AlternatingRowsDefaultCellStyle = dgvStyleAlt1;
        dgvProfiles.BackgroundColor = System.Drawing.Color.White;
        dgvProfiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
        dgvProfiles.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
        dgvStyleHeader1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dgvStyleHeader1.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
        dgvStyleHeader1.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        dgvStyleHeader1.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        dgvStyleHeader1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dgvStyleHeader1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dgvStyleHeader1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        dgvProfiles.ColumnHeadersDefaultCellStyle = dgvStyleHeader1;
        dgvProfiles.ColumnHeadersHeight = 28;
        dgvProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvProfiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colProfSelect,
            colProfDir,
            colProfName,
            colProfGoogle,
            colProfCookies,
            colProfProxy,
            colProfStatus
        });
        dgvStyleRow1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dgvStyleRow1.BackColor = System.Drawing.SystemColors.Window;
        dgvStyleRow1.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        dgvStyleRow1.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        dgvStyleRow1.SelectionBackColor = System.Drawing.Color.FromArgb(199, 210, 254);
        dgvStyleRow1.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
        dgvStyleRow1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        dgvProfiles.DefaultCellStyle = dgvStyleRow1;
        dgvProfiles.Dock = System.Windows.Forms.DockStyle.Fill;
        dgvProfiles.EnableHeadersVisualStyles = false;
        dgvProfiles.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
        dgvProfiles.Location = new System.Drawing.Point(6, 54);
        dgvProfiles.MultiSelect = false;
        dgvProfiles.Name = "dgvProfiles";
        dgvProfiles.RowHeadersVisible = false;
        dgvProfiles.RowTemplate.Height = 26;
        dgvProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvProfiles.Size = new System.Drawing.Size(1236, 135);
        dgvProfiles.TabIndex = 1;
        dgvProfiles.CellValueChanged += dgvProfiles_CellValueChanged;
        dgvProfiles.CurrentCellDirtyStateChanged += dgvProfiles_CurrentCellDirtyStateChanged;

        // 
        // colProfSelect
        // 
        colProfSelect.HeaderText = "Chọn";
        colProfSelect.Name = "colProfSelect";
        colProfSelect.Width = 50;

        // 
        // colProfDir
        // 
        colProfDir.HeaderText = "Thư mục";
        colProfDir.Name = "colProfDir";
        colProfDir.ReadOnly = true;
        colProfDir.Width = 100;

        // 
        // colProfName
        // 
        colProfName.HeaderText = "Tên Profile";
        colProfName.Name = "colProfName";
        colProfName.ReadOnly = true;
        colProfName.Width = 160;

        // 
        // colProfGoogle
        // 
        colProfGoogle.HeaderText = "Tài khoản Google";
        colProfGoogle.Name = "colProfGoogle";
        colProfGoogle.ReadOnly = true;
        colProfGoogle.Width = 180;

        // 
        // colProfCookies
        // 
        colProfCookies.HeaderText = "Cookie Republic / X";
        colProfCookies.Name = "colProfCookies";
        colProfCookies.ReadOnly = true;
        colProfCookies.Width = 200;

        // 
        // colProfProxy
        // 
        colProfProxy.HeaderText = "Proxy (ip:port hoặc user:pass@ip:port - Bỏ trống nếu không dùng)";
        colProfProxy.Name = "colProfProxy";
        colProfProxy.Width = 320;

        // 
        // colProfStatus
        // 
        colProfStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        colProfStatus.HeaderText = "Trạng thái";
        colProfStatus.Name = "colProfStatus";
        colProfStatus.ReadOnly = true;

        // 
        // panelActionControls
        // 
        panelActionControls.BackColor = System.Drawing.Color.FromArgb(242, 245, 249);
        panelActionControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        panelActionControls.Controls.Add(lblSelectedTarget);
        panelActionControls.Controls.Add(btnFetch);
        panelActionControls.Controls.Add(lblThreads);
        panelActionControls.Controls.Add(numThreads);
        panelActionControls.Controls.Add(lblLoop);
        panelActionControls.Controls.Add(numLoop);
        panelActionControls.Controls.Add(lblDelay);
        panelActionControls.Controls.Add(numDelay);
        panelActionControls.Controls.Add(btnStart);
        panelActionControls.Controls.Add(btnStop);
        panelActionControls.Controls.Add(btnOpenScreenshots);
        panelActionControls.Dock = System.Windows.Forms.DockStyle.Top;
        panelActionControls.Location = new System.Drawing.Point(0, 195);
        panelActionControls.Name = "panelActionControls";
        panelActionControls.Size = new System.Drawing.Size(1248, 72);
        panelActionControls.TabIndex = 1;

        // 
        // btnFetch
        // 
        btnFetch.BackColor = System.Drawing.Color.FromArgb(0, 102, 204);
        btnFetch.Cursor = System.Windows.Forms.Cursors.Hand;
        btnFetch.FlatAppearance.BorderSize = 0;
        btnFetch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnFetch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        btnFetch.ForeColor = System.Drawing.Color.White;
        btnFetch.Location = new System.Drawing.Point(8, 6);
        btnFetch.Name = "btnFetch";
        btnFetch.Size = new System.Drawing.Size(175, 30);
        btnFetch.TabIndex = 0;
        btnFetch.Text = "🔄 Tải danh sách Raid";
        btnFetch.UseVisualStyleBackColor = false;
        btnFetch.Click += btnFetch_Click;

        // 
        // lblSelectedTarget
        // 
        lblSelectedTarget.AutoSize = true;
        lblSelectedTarget.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblSelectedTarget.ForeColor = System.Drawing.Color.FromArgb(14, 116, 144);
        lblSelectedTarget.Location = new System.Drawing.Point(8, 44);
        lblSelectedTarget.Name = "lblSelectedTarget";
        lblSelectedTarget.Size = new System.Drawing.Size(535, 17);
        lblSelectedTarget.TabIndex = 1;
        lblSelectedTarget.Text = "🎯 TARGET CHUNG: Hãy bấm Tải danh sách Raid rồi chọn 1 hàng mục tiêu trong bảng dưới";

        // 
        // lblThreads
        // 
        lblThreads.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        lblThreads.AutoSize = true;
        lblThreads.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        lblThreads.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        lblThreads.Location = new System.Drawing.Point(620, 12);
        lblThreads.Name = "lblThreads";
        lblThreads.Size = new System.Drawing.Size(60, 15);
        lblThreads.TabIndex = 2;
        lblThreads.Text = "Số luồng:";

        // 
        // numThreads
        // 
        numThreads.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        numThreads.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        numThreads.Location = new System.Drawing.Point(685, 9);
        numThreads.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numThreads.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        numThreads.Name = "numThreads";
        numThreads.Size = new System.Drawing.Size(48, 23);
        numThreads.TabIndex = 3;
        numThreads.Value = new decimal(new int[] { 2, 0, 0, 0 });

        // 
        // lblLoop
        // 
        lblLoop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        lblLoop.AutoSize = true;
        lblLoop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblLoop.Location = new System.Drawing.Point(745, 12);
        lblLoop.Name = "lblLoop";
        lblLoop.Size = new System.Drawing.Size(61, 15);
        lblLoop.TabIndex = 4;
        lblLoop.Text = "Số lần lặp:";

        // 
        // numLoop
        // 
        numLoop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        numLoop.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        numLoop.Location = new System.Drawing.Point(810, 9);
        numLoop.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numLoop.Name = "numLoop";
        numLoop.Size = new System.Drawing.Size(48, 23);
        numLoop.TabIndex = 5;
        numLoop.Value = new decimal(new int[] { 1, 0, 0, 0 });

        // 
        // lblDelay
        // 
        lblDelay.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        lblDelay.AutoSize = true;
        lblDelay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblDelay.Location = new System.Drawing.Point(868, 12);
        lblDelay.Name = "lblDelay";
        lblDelay.Size = new System.Drawing.Size(56, 15);
        lblDelay.TabIndex = 6;
        lblDelay.Text = "Delay (s):";

        // 
        // numDelay
        // 
        numDelay.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        numDelay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        numDelay.Location = new System.Drawing.Point(928, 9);
        numDelay.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numDelay.Name = "numDelay";
        numDelay.Size = new System.Drawing.Size(48, 23);
        numDelay.TabIndex = 7;
        numDelay.Value = new decimal(new int[] { 5, 0, 0, 0 });

        // 
        // btnStart
        // 
        btnStart.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnStart.BackColor = System.Drawing.Color.FromArgb(22, 163, 74);
        btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
        btnStart.FlatAppearance.BorderSize = 0;
        btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnStart.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        btnStart.ForeColor = System.Drawing.Color.White;
        btnStart.Location = new System.Drawing.Point(988, 6);
        btnStart.Name = "btnStart";
        btnStart.Size = new System.Drawing.Size(250, 32);
        btnStart.TabIndex = 8;
        btnStart.Text = "🚀 BẮT ĐẦU CHẠY ĐA LUỒNG";
        btnStart.UseVisualStyleBackColor = false;
        btnStart.Click += btnStart_Click;

        // 
        // btnStop
        // 
        btnStop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnStop.BackColor = System.Drawing.Color.FromArgb(220, 38, 38);
        btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
        btnStop.Enabled = false;
        btnStop.FlatAppearance.BorderSize = 0;
        btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnStop.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        btnStop.ForeColor = System.Drawing.Color.White;
        btnStop.Location = new System.Drawing.Point(988, 40);
        btnStop.Name = "btnStop";
        btnStop.Size = new System.Drawing.Size(120, 27);
        btnStop.TabIndex = 9;
        btnStop.Text = "⏹ Dừng tất cả";
        btnStop.UseVisualStyleBackColor = false;
        btnStop.Click += btnStop_Click;

        // 
        // btnOpenScreenshots
        // 
        btnOpenScreenshots.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnOpenScreenshots.BackColor = System.Drawing.Color.FromArgb(71, 85, 105);
        btnOpenScreenshots.Cursor = System.Windows.Forms.Cursors.Hand;
        btnOpenScreenshots.FlatAppearance.BorderSize = 0;
        btnOpenScreenshots.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        btnOpenScreenshots.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        btnOpenScreenshots.ForeColor = System.Drawing.Color.White;
        btnOpenScreenshots.Location = new System.Drawing.Point(1118, 40);
        btnOpenScreenshots.Name = "btnOpenScreenshots";
        btnOpenScreenshots.Size = new System.Drawing.Size(120, 27);
        btnOpenScreenshots.TabIndex = 10;
        btnOpenScreenshots.Text = "📁 Xem ảnh";
        btnOpenScreenshots.UseVisualStyleBackColor = false;
        btnOpenScreenshots.Click += btnOpenScreenshots_Click;

        // 
        // grpRaids
        // 
        grpRaids.Controls.Add(dgvRaids);
        grpRaids.Dock = System.Windows.Forms.DockStyle.Fill;
        grpRaids.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        grpRaids.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        grpRaids.Location = new System.Drawing.Point(0, 267);
        grpRaids.Name = "grpRaids";
        grpRaids.Padding = new System.Windows.Forms.Padding(6);
        grpRaids.Size = new System.Drawing.Size(1248, 195);
        grpRaids.TabIndex = 2;
        grpRaids.TabStop = false;
        grpRaids.Text = "2. Chọn 1 Target Raid (Tất cả profiles đã chọn sẽ cùng thực hiện nhiệm vụ này):";

        // 
        // dgvRaids
        // 
        dgvRaids.AllowUserToAddRows = false;
        dgvRaids.AllowUserToDeleteRows = false;
        dgvRaids.AllowUserToResizeRows = false;
        dgvStyleAlt2.BackColor = System.Drawing.Color.FromArgb(248, 250, 252);
        dgvRaids.AlternatingRowsDefaultCellStyle = dgvStyleAlt2;
        dgvRaids.BackgroundColor = System.Drawing.Color.White;
        dgvRaids.BorderStyle = System.Windows.Forms.BorderStyle.None;
        dgvRaids.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
        dgvStyleHeader2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dgvStyleHeader2.BackColor = System.Drawing.Color.FromArgb(226, 232, 240);
        dgvStyleHeader2.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        dgvStyleHeader2.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        dgvStyleHeader2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dgvStyleHeader2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dgvStyleHeader2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        dgvRaids.ColumnHeadersDefaultCellStyle = dgvStyleHeader2;
        dgvRaids.ColumnHeadersHeight = 28;
        dgvRaids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgvRaids.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colSTT,
            colTarget,
            colStatus,
            colTimeLeft,
            colRaiders,
            colCardIndex,
            colRaw
        });
        dgvStyleRow2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dgvStyleRow2.BackColor = System.Drawing.SystemColors.Window;
        dgvStyleRow2.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        dgvStyleRow2.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        dgvStyleRow2.SelectionBackColor = System.Drawing.Color.FromArgb(199, 210, 254);
        dgvStyleRow2.SelectionForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
        dgvStyleRow2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        dgvRaids.DefaultCellStyle = dgvStyleRow2;
        dgvRaids.Dock = System.Windows.Forms.DockStyle.Fill;
        dgvRaids.EnableHeadersVisualStyles = false;
        dgvRaids.GridColor = System.Drawing.Color.FromArgb(226, 232, 240);
        dgvRaids.Location = new System.Drawing.Point(6, 22);
        dgvRaids.MultiSelect = false;
        dgvRaids.Name = "dgvRaids";
        dgvRaids.ReadOnly = true;
        dgvRaids.RowHeadersVisible = false;
        dgvRaids.RowTemplate.Height = 26;
        dgvRaids.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        dgvRaids.Size = new System.Drawing.Size(1236, 167);
        dgvRaids.TabIndex = 0;
        dgvRaids.SelectionChanged += dgvRaids_SelectionChanged;

        // 
        // colSTT
        // 
        colSTT.HeaderText = "STT";
        colSTT.Name = "colSTT";
        colSTT.ReadOnly = true;
        colSTT.Width = 55;

        // 
        // colTarget
        // 
        colTarget.HeaderText = "Mục tiêu (Target)";
        colTarget.Name = "colTarget";
        colTarget.ReadOnly = true;
        colTarget.Width = 180;

        // 
        // colStatus
        // 
        colStatus.HeaderText = "Trạng thái";
        colStatus.Name = "colStatus";
        colStatus.ReadOnly = true;
        colStatus.Width = 140;

        // 
        // colTimeLeft
        // 
        colTimeLeft.HeaderText = "Thời gian còn lại";
        colTimeLeft.Name = "colTimeLeft";
        colTimeLeft.ReadOnly = true;
        colTimeLeft.Width = 140;

        // 
        // colRaiders
        // 
        colRaiders.HeaderText = "Số người tham gia";
        colRaiders.Name = "colRaiders";
        colRaiders.ReadOnly = true;
        colRaiders.Width = 140;

        // 
        // colCardIndex
        // 
        colCardIndex.HeaderText = "Card Index";
        colCardIndex.Name = "colCardIndex";
        colCardIndex.ReadOnly = true;
        colCardIndex.Visible = false;

        // 
        // colRaw
        // 
        colRaw.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
        colRaw.HeaderText = "Chi tiết thẻ";
        colRaw.Name = "colRaw";
        colRaw.ReadOnly = true;

        // 
        // splitBottom
        // 
        splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
        splitBottom.Location = new System.Drawing.Point(8, 4);
        splitBottom.Name = "splitBottom";

        // 
        // splitBottom.Panel1
        // 
        splitBottom.Panel1.Controls.Add(grpLogs);

        // 
        // splitBottom.Panel2
        // 
        splitBottom.Panel2.Controls.Add(grpPreview);
        splitBottom.Size = new System.Drawing.Size(1248, 268);
        splitBottom.SplitterDistance = 750;
        splitBottom.TabIndex = 0;

        // 
        // grpLogs
        // 
        grpLogs.Controls.Add(btnClearLogs);
        grpLogs.Controls.Add(rtbLogs);
        grpLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        grpLogs.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        grpLogs.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        grpLogs.Location = new System.Drawing.Point(0, 0);
        grpLogs.Name = "grpLogs";
        grpLogs.Padding = new System.Windows.Forms.Padding(6);
        grpLogs.Size = new System.Drawing.Size(750, 268);
        grpLogs.TabIndex = 0;
        grpLogs.TabStop = false;
        grpLogs.Text = "Nhật ký tiến trình Đa Luồng (Live Logs):";

        // 
        // btnClearLogs
        // 
        btnClearLogs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        btnClearLogs.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        btnClearLogs.ForeColor = System.Drawing.Color.FromArgb(51, 65, 85);
        btnClearLogs.Location = new System.Drawing.Point(670, 14);
        btnClearLogs.Name = "btnClearLogs";
        btnClearLogs.Size = new System.Drawing.Size(74, 22);
        btnClearLogs.TabIndex = 1;
        btnClearLogs.Text = "Xóa log";
        btnClearLogs.UseVisualStyleBackColor = true;
        btnClearLogs.Click += btnClearLogs_Click;

        // 
        // rtbLogs
        // 
        rtbLogs.BackColor = System.Drawing.Color.FromArgb(15, 23, 42);
        rtbLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
        rtbLogs.Dock = System.Windows.Forms.DockStyle.Fill;
        rtbLogs.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        rtbLogs.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240);
        rtbLogs.Location = new System.Drawing.Point(6, 22);
        rtbLogs.Name = "rtbLogs";
        rtbLogs.ReadOnly = true;
        rtbLogs.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
        rtbLogs.Size = new System.Drawing.Size(738, 240);
        rtbLogs.TabIndex = 0;
        rtbLogs.Text = "";

        // 
        // grpPreview
        // 
        grpPreview.Controls.Add(lblPreviewName);
        grpPreview.Controls.Add(picPreview);
        grpPreview.Dock = System.Windows.Forms.DockStyle.Fill;
        grpPreview.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        grpPreview.ForeColor = System.Drawing.Color.FromArgb(30, 41, 59);
        grpPreview.Location = new System.Drawing.Point(0, 0);
        grpPreview.Name = "grpPreview";
        grpPreview.Padding = new System.Windows.Forms.Padding(6);
        grpPreview.Size = new System.Drawing.Size(494, 268);
        grpPreview.TabIndex = 0;
        grpPreview.TabStop = false;
        grpPreview.Text = "Ảnh chụp minh chứng mới nhất:";

        // 
        // lblPreviewName
        // 
        lblPreviewName.Dock = System.Windows.Forms.DockStyle.Bottom;
        lblPreviewName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        lblPreviewName.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
        lblPreviewName.Location = new System.Drawing.Point(6, 242);
        lblPreviewName.Name = "lblPreviewName";
        lblPreviewName.Size = new System.Drawing.Size(482, 20);
        lblPreviewName.TabIndex = 1;
        lblPreviewName.Text = "Chưa có ảnh";
        lblPreviewName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

        // 
        // picPreview
        // 
        picPreview.BackColor = System.Drawing.Color.FromArgb(2, 6, 23);
        picPreview.Dock = System.Windows.Forms.DockStyle.Fill;
        picPreview.Location = new System.Drawing.Point(6, 22);
        picPreview.Name = "picPreview";
        picPreview.Size = new System.Drawing.Size(482, 220);
        picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        picPreview.TabIndex = 0;
        picPreview.TabStop = false;

        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            statusLabel,
            statusProgress,
            statusProfile
        });
        statusStrip1.Location = new System.Drawing.Point(0, 818);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new System.Drawing.Size(1264, 22);
        statusStrip1.TabIndex = 2;
        statusStrip1.Text = "statusStrip1";

        // 
        // statusLabel
        // 
        statusLabel.Name = "statusLabel";
        statusLabel.Size = new System.Drawing.Size(54, 17);
        statusLabel.Text = "Sẵn sàng";

        // 
        // statusProgress
        // 
        statusProgress.Name = "statusProgress";
        statusProgress.Size = new System.Drawing.Size(120, 16);
        statusProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
        statusProgress.Visible = false;

        // 
        // statusProfile
        // 
        statusProfile.Name = "statusProfile";
        statusProfile.RightToLeft = System.Windows.Forms.RightToLeft.No;
        statusProfile.Size = new System.Drawing.Size(1075, 17);
        statusProfile.Spring = true;
        statusProfile.Text = "Chế độ: Đa luồng";
        statusProfile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

        // 
        // Form1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        ClientSize = new System.Drawing.Size(1264, 840);
        Controls.Add(splitVertical);
        Controls.Add(panelHeader);
        Controls.Add(statusStrip1);
        Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        MinimumSize = new System.Drawing.Size(1100, 700);
        Name = "Form1";
        StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        Text = "Republic Legion CC - Multi-Profile & Multi-Thread Raid Automation";
        FormClosing += Form1_FormClosing;
        Load += Form1_Load;
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        splitVertical.Panel1.ResumeLayout(false);
        splitVertical.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitVertical).EndInit();
        splitVertical.ResumeLayout(false);
        panelUpper.ResumeLayout(false);
        grpRaids.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvRaids).EndInit();
        panelActionControls.ResumeLayout(false);
        panelActionControls.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numThreads).EndInit();
        ((System.ComponentModel.ISupportInitialize)numLoop).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDelay).EndInit();
        grpProfiles.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvProfiles).EndInit();
        panelProfileToolbar.ResumeLayout(false);
        panelProfileToolbar.PerformLayout();
        splitBottom.Panel1.ResumeLayout(false);
        splitBottom.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitBottom).EndInit();
        splitBottom.ResumeLayout(false);
        grpLogs.ResumeLayout(false);
        grpPreview.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picPreview).EndInit();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Panel panelHeader;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblSubtitle;
    private System.Windows.Forms.Label lblAntiDetect;

    private System.Windows.Forms.SplitContainer splitVertical;
    private System.Windows.Forms.Panel panelUpper;
    private System.Windows.Forms.GroupBox grpProfiles;
    private System.Windows.Forms.Panel panelProfileToolbar;
    private System.Windows.Forms.Button btnSelectAll;
    private System.Windows.Forms.Button btnUnselectAll;
    private System.Windows.Forms.Button btnScanProfiles;
    private System.Windows.Forms.Button btnImportProxies;
    private System.Windows.Forms.Button btnTestProxies;
    private System.Windows.Forms.Label lblProfileSummary;
    private System.Windows.Forms.Label lblProfileSuggestion;
    private System.Windows.Forms.DataGridView dgvProfiles;
    private System.Windows.Forms.DataGridViewCheckBoxColumn colProfSelect;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProfDir;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProfName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProfGoogle;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProfCookies;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProfProxy;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProfStatus;

    private System.Windows.Forms.Panel panelActionControls;
    private System.Windows.Forms.Button btnFetch;
    private System.Windows.Forms.Label lblSelectedTarget;
    private System.Windows.Forms.Label lblThreads;
    private System.Windows.Forms.NumericUpDown numThreads;
    private System.Windows.Forms.Label lblLoop;
    private System.Windows.Forms.NumericUpDown numLoop;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.NumericUpDown numDelay;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnOpenScreenshots;

    private System.Windows.Forms.GroupBox grpRaids;
    private System.Windows.Forms.DataGridView dgvRaids;
    private System.Windows.Forms.DataGridViewTextBoxColumn colSTT;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTarget;
    private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTimeLeft;
    private System.Windows.Forms.DataGridViewTextBoxColumn colRaiders;
    private System.Windows.Forms.DataGridViewTextBoxColumn colCardIndex;
    private System.Windows.Forms.DataGridViewTextBoxColumn colRaw;

    private System.Windows.Forms.SplitContainer splitBottom;
    private System.Windows.Forms.GroupBox grpLogs;
    private System.Windows.Forms.RichTextBox rtbLogs;
    private System.Windows.Forms.Button btnClearLogs;
    private System.Windows.Forms.GroupBox grpPreview;
    private System.Windows.Forms.PictureBox picPreview;
    private System.Windows.Forms.Label lblPreviewName;

    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel statusLabel;
    private System.Windows.Forms.ToolStripProgressBar statusProgress;
    private System.Windows.Forms.ToolStripStatusLabel statusProfile;
}
