namespace Cau3_KeHoachDaNgoai
{
    partial class FormChuyenDi
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

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblNgonNgu = new System.Windows.Forms.Label();
            this.cboNgonNgu = new System.Windows.Forms.ComboBox();
            this.grpThongTin = new System.Windows.Forms.GroupBox();
            this.lblTenChuyen = new System.Windows.Forms.Label();
            this.txtTenChuyen = new System.Windows.Forms.TextBox();
            this.lblDiaDiem = new System.Windows.Forms.Label();
            this.txtDiaDiem = new System.Windows.Forms.TextBox();
            this.lblNgayDi = new System.Windows.Forms.Label();
            this.dtpNgayDi = new System.Windows.Forms.DateTimePicker();
            this.lblSoNguoi = new System.Windows.Forms.Label();
            this.numSoNguoi = new System.Windows.Forms.NumericUpDown();
            this.lblNganSach = new System.Windows.Forms.Label();
            this.numNganSach = new System.Windows.Forms.NumericUpDown();
            this.lblPhuongTien = new System.Windows.Forms.Label();
            this.cboPhuongTien = new System.Windows.Forms.ComboBox();
            this.chkHoanThanh = new System.Windows.Forms.CheckBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.grpThoiTiet = new System.Windows.Forms.GroupBox();
            this.btnXemThoiTiet = new System.Windows.Forms.Button();
            this.prgLoading = new System.Windows.Forms.ProgressBar();
            this.pnlThoiTiet = new System.Windows.Forms.Panel();
            this.lblThoiTietTitle = new System.Windows.Forms.Label();
            this.lblWeatherIcon = new System.Windows.Forms.Label();
            this.lblWeatherDesc = new System.Windows.Forms.Label();
            this.lblNhietDo = new System.Windows.Forms.Label();
            this.lblKhaNangMua = new System.Windows.Forms.Label();
            this.pnlCanhBao = new System.Windows.Forms.Panel();
            this.lblCanhBao = new System.Windows.Forms.Label();
            this.pnlLoc = new System.Windows.Forms.Panel();
            this.txtTimKiem = new System.Windows.Forms.TextBox();
            this.chkLocChuaHoanThanh = new System.Windows.Forms.CheckBox();
            this.lblTuNgay = new System.Windows.Forms.Label();
            this.dtpTuNgay = new System.Windows.Forms.DateTimePicker();
            this.lblDenNgay = new System.Windows.Forms.Label();
            this.dtpDenNgay = new System.Windows.Forms.DateTimePicker();
            this.btnTim = new System.Windows.Forms.Button();
            this.btnBoLoc = new System.Windows.Forms.Button();
            this.dgvChuyenDi = new System.Windows.Forms.DataGridView();
            this.colMa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenChuyen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDiaDiem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgayDi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoNguoi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNganSach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhuongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoanThanh = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.pnlTop.SuspendLayout();
            this.grpThongTin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNganSach)).BeginInit();
            this.grpThoiTiet.SuspendLayout();
            this.pnlThoiTiet.SuspendLayout();
            this.pnlCanhBao.SuspendLayout();
            this.pnlLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChuyenDi)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.lblNgonNgu);
            this.pnlTop.Controls.Add(this.cboNgonNgu);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(984, 45);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(12, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(232, 21);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Kế hoạch dã ngoại cuối tuần";
            // 
            // lblNgonNgu
            // 
            this.lblNgonNgu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNgonNgu.AutoSize = true;
            this.lblNgonNgu.Location = new System.Drawing.Point(740, 15);
            this.lblNgonNgu.Name = "lblNgonNgu";
            this.lblNgonNgu.Size = new System.Drawing.Size(65, 15);
            this.lblNgonNgu.TabIndex = 1;
            this.lblNgonNgu.Text = "Ngôn ngữ:";
            // 
            // cboNgonNgu
            // 
            this.cboNgonNgu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNgonNgu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNgonNgu.FormattingEnabled = true;
            this.cboNgonNgu.Items.AddRange(new object[] {
            "Tiếng Việt",
            "English"});
            this.cboNgonNgu.Location = new System.Drawing.Point(815, 12);
            this.cboNgonNgu.Name = "cboNgonNgu";
            this.cboNgonNgu.Size = new System.Drawing.Size(155, 23);
            this.cboNgonNgu.TabIndex = 2;
            this.cboNgonNgu.SelectedIndexChanged += new System.EventHandler(this.cboNgonNgu_SelectedIndexChanged);
            // 
            // grpThongTin
            // 
            this.grpThongTin.Controls.Add(this.lblTenChuyen);
            this.grpThongTin.Controls.Add(this.txtTenChuyen);
            this.grpThongTin.Controls.Add(this.lblDiaDiem);
            this.grpThongTin.Controls.Add(this.txtDiaDiem);
            this.grpThongTin.Controls.Add(this.lblNgayDi);
            this.grpThongTin.Controls.Add(this.dtpNgayDi);
            this.grpThongTin.Controls.Add(this.lblSoNguoi);
            this.grpThongTin.Controls.Add(this.numSoNguoi);
            this.grpThongTin.Controls.Add(this.lblNganSach);
            this.grpThongTin.Controls.Add(this.numNganSach);
            this.grpThongTin.Controls.Add(this.lblPhuongTien);
            this.grpThongTin.Controls.Add(this.cboPhuongTien);
            this.grpThongTin.Controls.Add(this.chkHoanThanh);
            this.grpThongTin.Controls.Add(this.btnThem);
            this.grpThongTin.Controls.Add(this.btnSua);
            this.grpThongTin.Controls.Add(this.btnXoa);
            this.grpThongTin.Controls.Add(this.btnLamMoi);
            this.grpThongTin.Location = new System.Drawing.Point(12, 51);
            this.grpThongTin.Name = "grpThongTin";
            this.grpThongTin.Size = new System.Drawing.Size(530, 220);
            this.grpThongTin.TabIndex = 1;
            this.grpThongTin.TabStop = false;
            this.grpThongTin.Text = "Thông tin chuyến đi";
            // 
            // lblTenChuyen
            // 
            this.lblTenChuyen.AutoSize = true;
            this.lblTenChuyen.Location = new System.Drawing.Point(15, 25);
            this.lblTenChuyen.Name = "lblTenChuyen";
            this.lblTenChuyen.Size = new System.Drawing.Size(71, 15);
            this.lblTenChuyen.TabIndex = 0;
            this.lblTenChuyen.Text = "Tên chuyến:";
            // 
            // txtTenChuyen
            // 
            this.txtTenChuyen.Location = new System.Drawing.Point(95, 22);
            this.txtTenChuyen.Name = "txtTenChuyen";
            this.txtTenChuyen.Size = new System.Drawing.Size(420, 23);
            this.txtTenChuyen.TabIndex = 1;
            // 
            // lblDiaDiem
            // 
            this.lblDiaDiem.AutoSize = true;
            this.lblDiaDiem.Location = new System.Drawing.Point(15, 55);
            this.lblDiaDiem.Name = "lblDiaDiem";
            this.lblDiaDiem.Size = new System.Drawing.Size(57, 15);
            this.lblDiaDiem.TabIndex = 2;
            this.lblDiaDiem.Text = "Địa điểm:";
            // 
            // txtDiaDiem
            // 
            this.txtDiaDiem.Location = new System.Drawing.Point(95, 52);
            this.txtDiaDiem.Name = "txtDiaDiem";
            this.txtDiaDiem.Size = new System.Drawing.Size(420, 23);
            this.txtDiaDiem.TabIndex = 3;
            // 
            // lblNgayDi
            // 
            this.lblNgayDi.AutoSize = true;
            this.lblNgayDi.Location = new System.Drawing.Point(15, 88);
            this.lblNgayDi.Name = "lblNgayDi";
            this.lblNgayDi.Size = new System.Drawing.Size(51, 15);
            this.lblNgayDi.TabIndex = 4;
            this.lblNgayDi.Text = "Ngày đi:";
            // 
            // dtpNgayDi
            // 
            this.dtpNgayDi.CustomFormat = "dd/MM/yyyy";
            this.dtpNgayDi.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayDi.Location = new System.Drawing.Point(95, 83);
            this.dtpNgayDi.Name = "dtpNgayDi";
            this.dtpNgayDi.Size = new System.Drawing.Size(150, 23);
            this.dtpNgayDi.TabIndex = 5;
            // 
            // lblSoNguoi
            // 
            this.lblSoNguoi.AutoSize = true;
            this.lblSoNguoi.Location = new System.Drawing.Point(280, 88);
            this.lblSoNguoi.Name = "lblSoNguoi";
            this.lblSoNguoi.Size = new System.Drawing.Size(57, 15);
            this.lblSoNguoi.TabIndex = 6;
            this.lblSoNguoi.Text = "Số người:";
            // 
            // numSoNguoi
            // 
            this.numSoNguoi.Location = new System.Drawing.Point(365, 83);
            this.numSoNguoi.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSoNguoi.Name = "numSoNguoi";
            this.numSoNguoi.Size = new System.Drawing.Size(150, 23);
            this.numSoNguoi.TabIndex = 7;
            this.numSoNguoi.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // lblNganSach
            // 
            this.lblNganSach.AutoSize = true;
            this.lblNganSach.Location = new System.Drawing.Point(15, 120);
            this.lblNganSach.Name = "lblNganSach";
            this.lblNganSach.Size = new System.Drawing.Size(66, 15);
            this.lblNganSach.TabIndex = 8;
            this.lblNganSach.Text = "Ngân sách:";
            // 
            // numNganSach
            // 
            this.numNganSach.Increment = new decimal(new int[] {
            500000,
            0,
            0,
            0});
            this.numNganSach.Location = new System.Drawing.Point(95, 115);
            this.numNganSach.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numNganSach.Name = "numNganSach";
            this.numNganSach.Size = new System.Drawing.Size(150, 23);
            this.numNganSach.TabIndex = 9;
            this.numNganSach.ThousandsSeparator = true;
            // 
            // lblPhuongTien
            // 
            this.lblPhuongTien.AutoSize = true;
            this.lblPhuongTien.Location = new System.Drawing.Point(280, 120);
            this.lblPhuongTien.Name = "lblPhuongTien";
            this.lblPhuongTien.Size = new System.Drawing.Size(75, 15);
            this.lblPhuongTien.TabIndex = 10;
            this.lblPhuongTien.Text = "Phương tiện:";
            // 
            // cboPhuongTien
            // 
            this.cboPhuongTien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPhuongTien.FormattingEnabled = true;
            this.cboPhuongTien.Items.AddRange(new object[] {
            "Xe máy",
            "Ô tô",
            "Xe khách",
            "Tàu hỏa"});
            this.cboPhuongTien.Location = new System.Drawing.Point(365, 115);
            this.cboPhuongTien.Name = "cboPhuongTien";
            this.cboPhuongTien.Size = new System.Drawing.Size(150, 23);
            this.cboPhuongTien.TabIndex = 11;
            // 
            // chkHoanThanh
            // 
            this.chkHoanThanh.AutoSize = true;
            this.chkHoanThanh.Location = new System.Drawing.Point(95, 148);
            this.chkHoanThanh.Name = "chkHoanThanh";
            this.chkHoanThanh.Size = new System.Drawing.Size(107, 19);
            this.chkHoanThanh.TabIndex = 12;
            this.chkHoanThanh.Text = "Đã hoàn thành";
            this.chkHoanThanh.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(95, 178);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(80, 28);
            this.btnThem.TabIndex = 13;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(185, 178);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(80, 28);
            this.btnSua.TabIndex = 14;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(275, 178);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(80, 28);
            this.btnXoa.TabIndex = 15;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(365, 178);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(80, 28);
            this.btnLamMoi.TabIndex = 16;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // grpThoiTiet
            // 
            this.grpThoiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpThoiTiet.Controls.Add(this.btnXemThoiTiet);
            this.grpThoiTiet.Controls.Add(this.prgLoading);
            this.grpThoiTiet.Controls.Add(this.pnlThoiTiet);
            this.grpThoiTiet.Location = new System.Drawing.Point(550, 51);
            this.grpThongTin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.grpThoiTiet.Name = "grpThoiTiet";
            this.grpThoiTiet.Size = new System.Drawing.Size(422, 220);
            this.grpThoiTiet.TabIndex = 2;
            this.grpThoiTiet.TabStop = false;
            this.grpThoiTiet.Text = "Dự báo thời tiết";
            // 
            // btnXemThoiTiet
            // 
            this.btnXemThoiTiet.Location = new System.Drawing.Point(15, 20);
            this.btnXemThoiTiet.Name = "btnXemThoiTiet";
            this.btnXemThoiTiet.Size = new System.Drawing.Size(120, 28);
            this.btnXemThoiTiet.TabIndex = 0;
            this.btnXemThoiTiet.Text = "Xem thời tiết";
            this.btnXemThoiTiet.UseVisualStyleBackColor = true;
            this.btnXemThoiTiet.Click += new System.EventHandler(this.btnXemThoiTiet_Click);
            // 
            // prgLoading
            // 
            this.prgLoading.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgLoading.Location = new System.Drawing.Point(145, 24);
            this.prgLoading.MarqueeAnimationSpeed = 30;
            this.prgLoading.Name = "prgLoading";
            this.prgLoading.Size = new System.Drawing.Size(265, 20);
            this.prgLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.prgLoading.TabIndex = 1;
            this.prgLoading.Visible = false;
            // 
            // pnlThoiTiet
            // 
            this.pnlThoiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlThoiTiet.BackColor = System.Drawing.Color.White;
            this.pnlThoiTiet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlThoiTiet.Controls.Add(this.lblThoiTietTitle);
            this.pnlThoiTiet.Controls.Add(this.lblWeatherIcon);
            this.pnlThoiTiet.Controls.Add(this.lblWeatherDesc);
            this.pnlThoiTiet.Controls.Add(this.lblNhietDo);
            this.pnlThoiTiet.Controls.Add(this.lblKhaNangMua);
            this.pnlThoiTiet.Controls.Add(this.pnlCanhBao);
            this.pnlThoiTiet.Location = new System.Drawing.Point(15, 55);
            this.pnlThoiTiet.Name = "pnlThoiTiet";
            this.pnlThoiTiet.Size = new System.Drawing.Size(395, 155);
            this.pnlThoiTiet.TabIndex = 2;
            // 
            // lblThoiTietTitle
            // 
            this.lblThoiTietTitle.AutoSize = true;
            this.lblThoiTietTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblThoiTietTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblThoiTietTitle.Location = new System.Drawing.Point(10, 5);
            this.lblThoiTietTitle.Name = "lblThoiTietTitle";
            this.lblThoiTietTitle.Size = new System.Drawing.Size(188, 15);
            this.lblThoiTietTitle.TabIndex = 0;
            this.lblThoiTietTitle.Text = "Chọn chuyến đi để xem dự báo";
            // 
            // lblWeatherIcon
            // 
            this.lblWeatherIcon.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblWeatherIcon.Location = new System.Drawing.Point(10, 25);
            this.lblWeatherIcon.Name = "lblWeatherIcon";
            this.lblWeatherIcon.Size = new System.Drawing.Size(65, 55);
            this.lblWeatherIcon.TabIndex = 1;
            this.lblWeatherIcon.Text = "🌤️";
            this.lblWeatherIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWeatherDesc
            // 
            this.lblWeatherDesc.AutoSize = true;
            this.lblWeatherDesc.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblWeatherDesc.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblWeatherDesc.Location = new System.Drawing.Point(85, 25);
            this.lblWeatherDesc.Name = "lblWeatherDesc";
            this.lblWeatherDesc.Size = new System.Drawing.Size(130, 20);
            this.lblWeatherDesc.TabIndex = 2;
            this.lblWeatherDesc.Text = "Chưa có thông tin";
            // 
            // lblNhietDo
            // 
            this.lblNhietDo.AutoSize = true;
            this.lblNhietDo.Location = new System.Drawing.Point(85, 48);
            this.lblNhietDo.Name = "lblNhietDo";
            this.lblNhietDo.Size = new System.Drawing.Size(56, 15);
            this.lblNhietDo.TabIndex = 3;
            this.lblNhietDo.Text = "Nhiệt độ:";
            // 
            // lblKhaNangMua
            // 
            this.lblKhaNangMua.AutoSize = true;
            this.lblKhaNangMua.Location = new System.Drawing.Point(85, 68);
            this.lblKhaNangMua.Name = "lblKhaNangMua";
            this.lblKhaNangMua.Size = new System.Drawing.Size(86, 15);
            this.lblKhaNangMua.TabIndex = 4;
            this.lblKhaNangMua.Text = "Khả năng mưa:";
            // 
            // pnlCanhBao
            // 
            this.pnlCanhBao.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCanhBao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(165)))), ((int)(((byte)(0)))));
            this.pnlCanhBao.Controls.Add(this.lblCanhBao);
            this.pnlCanhBao.Location = new System.Drawing.Point(5, 115);
            this.pnlCanhBao.Name = "pnlCanhBao";
            this.pnlCanhBao.Size = new System.Drawing.Size(383, 30);
            this.pnlCanhBao.TabIndex = 5;
            this.pnlCanhBao.Visible = false;
            // 
            // lblCanhBao
            // 
            this.lblCanhBao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCanhBao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCanhBao.ForeColor = System.Drawing.Color.DarkRed;
            this.lblCanhBao.Location = new System.Drawing.Point(0, 0);
            this.lblCanhBao.Name = "lblCanhBao";
            this.lblCanhBao.Size = new System.Drawing.Size(383, 30);
            this.lblCanhBao.TabIndex = 0;
            this.lblCanhBao.Text = "⚠️ Nên mang áo mưa hoặc cân nhắc dời lịch";
            this.lblCanhBao.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlLoc
            // 
            this.pnlLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlLoc.Controls.Add(this.txtTimKiem);
            this.pnlLoc.Controls.Add(this.chkLocChuaHoanThanh);
            this.pnlLoc.Controls.Add(this.lblTuNgay);
            this.pnlLoc.Controls.Add(this.dtpTuNgay);
            this.pnlLoc.Controls.Add(this.lblDenNgay);
            this.pnlLoc.Controls.Add(this.dtpDenNgay);
            this.pnlLoc.Controls.Add(this.btnTim);
            this.pnlLoc.Controls.Add(this.btnBoLoc);
            this.pnlLoc.Location = new System.Drawing.Point(12, 280);
            this.pnlLoc.Name = "pnlLoc";
            this.pnlLoc.Size = new System.Drawing.Size(960, 40);
            this.pnlLoc.TabIndex = 3;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.Location = new System.Drawing.Point(0, 8);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Tìm theo tên hoặc địa điểm...";
            this.txtTimKiem.Size = new System.Drawing.Size(180, 23);
            this.txtTimKiem.TabIndex = 0;
            // 
            // chkLocChuaHoanThanh
            // 
            this.chkLocChuaHoanThanh.AutoSize = true;
            this.chkLocChuaHoanThanh.Location = new System.Drawing.Point(195, 10);
            this.chkLocChuaHoanThanh.Name = "chkLocChuaHoanThanh";
            this.chkLocChuaHoanThanh.Size = new System.Drawing.Size(180, 19);
            this.chkLocChuaHoanThanh.TabIndex = 1;
            this.chkLocChuaHoanThanh.Text = "Chỉ hiện chuyến chưa hoàn thành";
            this.chkLocChuaHoanThanh.UseVisualStyleBackColor = true;
            // 
            // lblTuNgay
            // 
            this.lblTuNgay.AutoSize = true;
            this.lblTuNgay.Location = new System.Drawing.Point(390, 12);
            this.lblTuNgay.Name = "lblTuNgay";
            this.lblTuNgay.Size = new System.Drawing.Size(52, 15);
            this.lblTuNgay.TabIndex = 2;
            this.lblTuNgay.Text = "Từ ngày:";
            // 
            // dtpTuNgay
            // 
            this.dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpTuNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTuNgay.Location = new System.Drawing.Point(448, 8);
            this.dtpTuNgay.Name = "dtpTuNgay";
            this.dtpTuNgay.Size = new System.Drawing.Size(115, 23);
            this.dtpTuNgay.TabIndex = 3;
            // 
            // lblDenNgay
            // 
            this.lblDenNgay.AutoSize = true;
            this.lblDenNgay.Location = new System.Drawing.Point(575, 12);
            this.lblDenNgay.Name = "lblDenNgay";
            this.lblDenNgay.Size = new System.Drawing.Size(60, 15);
            this.lblDenNgay.TabIndex = 4;
            this.lblDenNgay.Text = "Đến ngày:";
            // 
            // dtpDenNgay
            // 
            this.dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            this.dtpDenNgay.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDenNgay.Location = new System.Drawing.Point(640, 8);
            this.dtpDenNgay.Name = "dtpDenNgay";
            this.dtpDenNgay.Size = new System.Drawing.Size(115, 23);
            this.dtpDenNgay.TabIndex = 5;
            // 
            // btnTim
            // 
            this.btnTim.Location = new System.Drawing.Point(770, 6);
            this.btnTim.Name = "btnTim";
            this.btnTim.Size = new System.Drawing.Size(70, 26);
            this.btnTim.TabIndex = 6;
            this.btnTim.Text = "Tìm kiếm";
            this.btnTim.UseVisualStyleBackColor = true;
            this.btnTim.Click += new System.EventHandler(this.btnTim_Click);
            // 
            // btnBoLoc
            // 
            this.btnBoLoc.Location = new System.Drawing.Point(850, 6);
            this.btnBoLoc.Name = "btnBoLoc";
            this.btnBoLoc.Size = new System.Drawing.Size(70, 26);
            this.btnBoLoc.TabIndex = 7;
            this.btnBoLoc.Text = "Bỏ lọc";
            this.btnBoLoc.UseVisualStyleBackColor = true;
            this.btnBoLoc.Click += new System.EventHandler(this.btnBoLoc_Click);
            // 
            // dgvChuyenDi
            // 
            this.dgvChuyenDi.AllowUserToAddRows = false;
            this.dgvChuyenDi.AllowUserToDeleteRows = false;
            this.dgvChuyenDi.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvChuyenDi.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvChuyenDi.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvChuyenDi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChuyenDi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMa,
            this.colTenChuyen,
            this.colDiaDiem,
            this.colNgayDi,
            this.colSoNguoi,
            this.colNganSach,
            this.colPhuongTien,
            this.colHoanThanh});
            this.dgvChuyenDi.Location = new System.Drawing.Point(12, 330);
            this.dgvChuyenDi.MultiSelect = false;
            this.dgvChuyenDi.Name = "dgvChuyenDi";
            this.dgvChuyenDi.ReadOnly = true;
            this.dgvChuyenDi.RowHeadersWidth = 30;
            this.dgvChuyenDi.RowTemplate.Height = 25;
            this.dgvChuyenDi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChuyenDi.Size = new System.Drawing.Size(960, 255);
            this.dgvChuyenDi.TabIndex = 4;
            this.dgvChuyenDi.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChuyenDi_CellClick);
            // 
            // colMa
            // 
            this.colMa.DataPropertyName = "MaChuyen";
            this.colMa.FillWeight = 30F;
            this.colMa.HeaderText = "Mã";
            this.colMa.Name = "colMa";
            this.colMa.ReadOnly = true;
            // 
            // colTenChuyen
            // 
            this.colTenChuyen.DataPropertyName = "TenChuyen";
            this.colTenChuyen.FillWeight = 110F;
            this.colTenChuyen.HeaderText = "Tên chuyến";
            this.colTenChuyen.Name = "colTenChuyen";
            this.colTenChuyen.ReadOnly = true;
            // 
            // colDiaDiem
            // 
            this.colDiaDiem.DataPropertyName = "DiaDiem";
            this.colDiaDiem.FillWeight = 80F;
            this.colDiaDiem.HeaderText = "Địa điểm";
            this.colDiaDiem.Name = "colDiaDiem";
            this.colDiaDiem.ReadOnly = true;
            // 
            // colNgayDi
            // 
            this.colNgayDi.DataPropertyName = "NgayDiStr";
            this.colNgayDi.FillWeight = 60F;
            this.colNgayDi.HeaderText = "Ngày đi";
            this.colNgayDi.Name = "colNgayDi";
            this.colNgayDi.ReadOnly = true;
            // 
            // colSoNguoi
            // 
            this.colSoNguoi.DataPropertyName = "SoNguoi";
            this.colSoNguoi.FillWeight = 40F;
            this.colSoNguoi.HeaderText = "Số người";
            this.colSoNguoi.Name = "colSoNguoi";
            this.colSoNguoi.ReadOnly = true;
            // 
            // colNganSach
            // 
            this.colNganSach.DataPropertyName = "NganSachFormatted";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colNganSach.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNganSach.FillWeight = 70F;
            this.colNganSach.HeaderText = "Ngân sách";
            this.colNganSach.Name = "colNganSach";
            this.colNganSach.ReadOnly = true;
            // 
            // colPhuongTien
            // 
            this.colPhuongTien.DataPropertyName = "PhuongTien";
            this.colPhuongTien.FillWeight = 60F;
            this.colPhuongTien.HeaderText = "Phương tiện";
            this.colPhuongTien.Name = "colPhuongTien";
            this.colPhuongTien.ReadOnly = true;
            // 
            // colHoanThanh
            // 
            this.colHoanThanh.DataPropertyName = "DaHoanThanh";
            this.colHoanThanh.FillWeight = 50F;
            this.colHoanThanh.HeaderText = "Hoàn thành";
            this.colHoanThanh.Name = "colHoanThanh";
            this.colHoanThanh.ReadOnly = true;
            // 
            // FormChuyenDi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 597);
            this.Controls.Add(this.dgvChuyenDi);
            this.Controls.Add(this.pnlLoc);
            this.Controls.Add(this.grpThoiTiet);
            this.Controls.Add(this.grpThongTin);
            this.Controls.Add(this.pnlTop);
            this.MinimumSize = new System.Drawing.Size(1000, 630);
            this.Name = "FormChuyenDi";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kế hoạch dã ngoại cuối tuần";
            this.Load += new System.EventHandler(this.FormChuyenDi_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.grpThongTin.ResumeLayout(false);
            this.grpThongTin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSoNguoi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numNganSach)).EndInit();
            this.grpThoiTiet.ResumeLayout(false);
            this.pnlThoiTiet.ResumeLayout(false);
            this.pnlThoiTiet.PerformLayout();
            this.pnlCanhBao.ResumeLayout(false);
            this.pnlLoc.ResumeLayout(false);
            this.pnlLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChuyenDi)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblNgonNgu;
        private System.Windows.Forms.ComboBox cboNgonNgu;
        private System.Windows.Forms.GroupBox grpThongTin;
        private System.Windows.Forms.Label lblTenChuyen;
        private System.Windows.Forms.TextBox txtTenChuyen;
        private System.Windows.Forms.Label lblDiaDiem;
        private System.Windows.Forms.TextBox txtDiaDiem;
        private System.Windows.Forms.Label lblNgayDi;
        private System.Windows.Forms.DateTimePicker dtpNgayDi;
        private System.Windows.Forms.Label lblSoNguoi;
        private System.Windows.Forms.NumericUpDown numSoNguoi;
        private System.Windows.Forms.Label lblNganSach;
        private System.Windows.Forms.NumericUpDown numNganSach;
        private System.Windows.Forms.Label lblPhuongTien;
        private System.Windows.Forms.ComboBox cboPhuongTien;
        private System.Windows.Forms.CheckBox chkHoanThanh;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.GroupBox grpThoiTiet;
        private System.Windows.Forms.Button btnXemThoiTiet;
        private System.Windows.Forms.ProgressBar prgLoading;
        private System.Windows.Forms.Panel pnlThoiTiet;
        private System.Windows.Forms.Label lblThoiTietTitle;
        private System.Windows.Forms.Label lblWeatherIcon;
        private System.Windows.Forms.Label lblWeatherDesc;
        private System.Windows.Forms.Label lblNhietDo;
        private System.Windows.Forms.Label lblKhaNangMua;
        private System.Windows.Forms.Panel pnlCanhBao;
        private System.Windows.Forms.Label lblCanhBao;
        private System.Windows.Forms.Panel pnlLoc;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.CheckBox chkLocChuaHoanThanh;
        private System.Windows.Forms.Label lblTuNgay;
        private System.Windows.Forms.DateTimePicker dtpTuNgay;
        private System.Windows.Forms.Label lblDenNgay;
        private System.Windows.Forms.DateTimePicker dtpDenNgay;
        private System.Windows.Forms.Button btnTim;
        private System.Windows.Forms.Button btnBoLoc;
        private System.Windows.Forms.DataGridView dgvChuyenDi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMa;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenChuyen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiaDiem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayDi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoNguoi;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNganSach;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhuongTien;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHoanThanh;
    }
}
