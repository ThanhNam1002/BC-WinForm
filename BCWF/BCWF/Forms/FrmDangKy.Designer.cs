namespace BCWF.Forms
{
    partial class FrmDangKy
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabMonHoc = new TabPage();
            lvMonHoc = new ListView();
            colMaMon = new ColumnHeader();
            colTenMon = new ColumnHeader();
            colSoTC = new ColumnHeader();
            colTienQuyet = new ColumnHeader();
            pnlMonEdit = new Panel();
            lblMaMon = new Label();
            txtMaMon = new TextBox();
            lblTenMon = new Label();
            txtTenMon = new TextBox();
            lblSoTC = new Label();
            nudSoTC = new NumericUpDown();
            lblTienQuyet = new Label();
            cboTienQuyet = new ComboBox();
            btnThemMon = new Button();
            btnSuaMon = new Button();
            btnXoaMon = new Button();
            lblMonGhiChu = new Label();
            tabDangKy = new TabPage();
            pnlDkCenter = new Panel();
            grpDaDangKy = new GroupBox();
            lstDaDangKy = new ListBox();
            pnlChuyen = new Panel();
            btnChonMon = new Button();
            btnBoMon = new Button();
            grpChuaDangKy = new GroupBox();
            lstChuaDangKy = new ListBox();
            pnlDkTop = new Panel();
            lblLop = new Label();
            cboLop = new ComboBox();
            lblSinhVien = new Label();
            cboSinhVien = new ComboBox();
            lblHocKy = new Label();
            cboHocKy = new ComboBox();
            pnlDkBottom = new Panel();
            lblTongTinChi = new Label();
            lblGioiHan = new Label();
            btnLuuDangKy = new Button();
            btnLamMoiDangKy = new Button();
            tabControl1.SuspendLayout();
            tabMonHoc.SuspendLayout();
            pnlMonEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoTC).BeginInit();
            tabDangKy.SuspendLayout();
            pnlDkCenter.SuspendLayout();
            grpDaDangKy.SuspendLayout();
            pnlChuyen.SuspendLayout();
            grpChuaDangKy.SuspendLayout();
            pnlDkTop.SuspendLayout();
            pnlDkBottom.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabMonHoc);
            tabControl1.Controls.Add(tabDangKy);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1000, 620);
            tabControl1.TabIndex = 0;
            // 
            // tabMonHoc
            // 
            tabMonHoc.Controls.Add(lvMonHoc);
            tabMonHoc.Controls.Add(pnlMonEdit);
            tabMonHoc.Location = new Point(4, 29);
            tabMonHoc.Name = "tabMonHoc";
            tabMonHoc.Padding = new Padding(3);
            tabMonHoc.Size = new Size(992, 587);
            tabMonHoc.TabIndex = 0;
            tabMonHoc.Text = "Quản lý môn học";
            tabMonHoc.UseVisualStyleBackColor = true;
            // 
            // lvMonHoc
            // 
            lvMonHoc.Columns.AddRange(new ColumnHeader[] { colMaMon, colTenMon, colSoTC, colTienQuyet });
            lvMonHoc.Dock = DockStyle.Fill;
            lvMonHoc.FullRowSelect = true;
            lvMonHoc.GridLines = true;
            lvMonHoc.Location = new Point(303, 3);
            lvMonHoc.MultiSelect = false;
            lvMonHoc.Name = "lvMonHoc";
            lvMonHoc.Size = new Size(686, 581);
            lvMonHoc.TabIndex = 1;
            lvMonHoc.UseCompatibleStateImageBehavior = false;
            lvMonHoc.View = View.Details;
            lvMonHoc.SelectedIndexChanged += lvMonHoc_SelectedIndexChanged;
            // 
            // colMaMon
            // 
            colMaMon.Text = "Mã môn";
            colMaMon.Width = 100;
            // 
            // colTenMon
            // 
            colTenMon.Text = "Tên môn";
            colTenMon.Width = 260;
            // 
            // colSoTC
            // 
            colSoTC.Text = "Số TC";
            colSoTC.Width = 70;
            // 
            // colTienQuyet
            // 
            colTienQuyet.Text = "Môn tiên quyết";
            colTienQuyet.Width = 130;
            // 
            // pnlMonEdit
            // 
            pnlMonEdit.Controls.Add(lblMaMon);
            pnlMonEdit.Controls.Add(txtMaMon);
            pnlMonEdit.Controls.Add(lblTenMon);
            pnlMonEdit.Controls.Add(txtTenMon);
            pnlMonEdit.Controls.Add(lblSoTC);
            pnlMonEdit.Controls.Add(nudSoTC);
            pnlMonEdit.Controls.Add(lblTienQuyet);
            pnlMonEdit.Controls.Add(cboTienQuyet);
            pnlMonEdit.Controls.Add(btnThemMon);
            pnlMonEdit.Controls.Add(btnSuaMon);
            pnlMonEdit.Controls.Add(btnXoaMon);
            pnlMonEdit.Controls.Add(lblMonGhiChu);
            pnlMonEdit.Dock = DockStyle.Left;
            pnlMonEdit.Location = new Point(3, 3);
            pnlMonEdit.Name = "pnlMonEdit";
            pnlMonEdit.Size = new Size(300, 581);
            pnlMonEdit.TabIndex = 0;
            // 
            // lblMaMon
            // 
            lblMaMon.AutoSize = true;
            lblMaMon.Location = new Point(16, 20);
            lblMaMon.Name = "lblMaMon";
            lblMaMon.Size = new Size(65, 20);
            lblMaMon.TabIndex = 0;
            lblMaMon.Text = "Mã môn:";
            // 
            // txtMaMon
            // 
            txtMaMon.Location = new Point(16, 44);
            txtMaMon.Name = "txtMaMon";
            txtMaMon.Size = new Size(250, 27);
            txtMaMon.TabIndex = 1;
            // 
            // lblTenMon
            // 
            lblTenMon.AutoSize = true;
            lblTenMon.Location = new Point(16, 82);
            lblTenMon.Name = "lblTenMon";
            lblTenMon.Size = new Size(70, 20);
            lblTenMon.TabIndex = 2;
            lblTenMon.Text = "Tên môn:";
            // 
            // txtTenMon
            // 
            txtTenMon.Location = new Point(16, 106);
            txtTenMon.Name = "txtTenMon";
            txtTenMon.Size = new Size(250, 27);
            txtTenMon.TabIndex = 3;
            // 
            // lblSoTC
            // 
            lblSoTC.AutoSize = true;
            lblSoTC.Location = new Point(16, 144);
            lblSoTC.Name = "lblSoTC";
            lblSoTC.Size = new Size(82, 20);
            lblSoTC.TabIndex = 4;
            lblSoTC.Text = "Số tín chỉ:";
            // 
            // nudSoTC
            // 
            nudSoTC.Location = new Point(16, 168);
            nudSoTC.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            nudSoTC.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudSoTC.Name = "nudSoTC";
            nudSoTC.Size = new Size(80, 27);
            nudSoTC.TabIndex = 5;
            nudSoTC.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // lblTienQuyet
            // 
            lblTienQuyet.AutoSize = true;
            lblTienQuyet.Location = new Point(16, 206);
            lblTienQuyet.Name = "lblTienQuyet";
            lblTienQuyet.Size = new Size(110, 20);
            lblTienQuyet.TabIndex = 6;
            lblTienQuyet.Text = "Môn tiên quyết:";
            // 
            // cboTienQuyet
            // 
            cboTienQuyet.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTienQuyet.FormattingEnabled = true;
            cboTienQuyet.Location = new Point(16, 230);
            cboTienQuyet.Name = "cboTienQuyet";
            cboTienQuyet.Size = new Size(250, 28);
            cboTienQuyet.TabIndex = 7;
            // 
            // btnThemMon
            // 
            btnThemMon.Location = new Point(16, 274);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(75, 32);
            btnThemMon.TabIndex = 8;
            btnThemMon.Text = "Thêm";
            btnThemMon.UseVisualStyleBackColor = true;
            btnThemMon.Click += btnThemMon_Click;
            // 
            // btnSuaMon
            // 
            btnSuaMon.Location = new Point(99, 274);
            btnSuaMon.Name = "btnSuaMon";
            btnSuaMon.Size = new Size(75, 32);
            btnSuaMon.TabIndex = 9;
            btnSuaMon.Text = "Sửa";
            btnSuaMon.UseVisualStyleBackColor = true;
            btnSuaMon.Click += btnSuaMon_Click;
            // 
            // btnXoaMon
            // 
            btnXoaMon.Location = new Point(182, 274);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(75, 32);
            btnXoaMon.TabIndex = 10;
            btnXoaMon.Text = "Xóa";
            btnXoaMon.UseVisualStyleBackColor = true;
            btnXoaMon.Click += btnXoaMon_Click;
            // 
            // lblMonGhiChu
            // 
            lblMonGhiChu.ForeColor = SystemColors.GrayText;
            lblMonGhiChu.Location = new Point(16, 324);
            lblMonGhiChu.Name = "lblMonGhiChu";
            lblMonGhiChu.Size = new Size(260, 80);
            lblMonGhiChu.TabIndex = 11;
            lblMonGhiChu.Text = "Chọn một môn trong danh sách bên phải để đưa lên vùng nhập liệu khi sửa hoặc xóa.";
            // 
            // tabDangKy
            // 
            tabDangKy.Controls.Add(pnlDkCenter);
            tabDangKy.Controls.Add(pnlDkTop);
            tabDangKy.Controls.Add(pnlDkBottom);
            tabDangKy.Location = new Point(4, 29);
            tabDangKy.Name = "tabDangKy";
            tabDangKy.Padding = new Padding(3);
            tabDangKy.Size = new Size(992, 587);
            tabDangKy.TabIndex = 1;
            tabDangKy.Text = "Đăng ký học phần";
            tabDangKy.UseVisualStyleBackColor = true;
            // 
            // pnlDkCenter
            // 
            pnlDkCenter.Controls.Add(grpDaDangKy);
            pnlDkCenter.Controls.Add(pnlChuyen);
            pnlDkCenter.Controls.Add(grpChuaDangKy);
            pnlDkCenter.Dock = DockStyle.Fill;
            pnlDkCenter.Location = new Point(3, 63);
            pnlDkCenter.Name = "pnlDkCenter";
            pnlDkCenter.Size = new Size(986, 456);
            pnlDkCenter.TabIndex = 1;
            // 
            // grpDaDangKy
            // 
            grpDaDangKy.Controls.Add(lstDaDangKy);
            grpDaDangKy.Dock = DockStyle.Fill;
            grpDaDangKy.Location = new Point(420, 0);
            grpDaDangKy.Name = "grpDaDangKy";
            grpDaDangKy.Size = new Size(566, 456);
            grpDaDangKy.TabIndex = 2;
            grpDaDangKy.TabStop = false;
            grpDaDangKy.Text = "Môn đã chọn";
            // 
            // lstDaDangKy
            // 
            lstDaDangKy.Dock = DockStyle.Fill;
            lstDaDangKy.ItemHeight = 20;
            lstDaDangKy.Location = new Point(3, 23);
            lstDaDangKy.Name = "lstDaDangKy";
            lstDaDangKy.SelectionMode = SelectionMode.MultiExtended;
            lstDaDangKy.Size = new Size(560, 430);
            lstDaDangKy.TabIndex = 0;
            lstDaDangKy.DoubleClick += lstDaDangKy_DoubleClick;
            // 
            // pnlChuyen
            // 
            pnlChuyen.Controls.Add(btnChonMon);
            pnlChuyen.Controls.Add(btnBoMon);
            pnlChuyen.Dock = DockStyle.Left;
            pnlChuyen.Location = new Point(340, 0);
            pnlChuyen.Name = "pnlChuyen";
            pnlChuyen.Size = new Size(80, 456);
            pnlChuyen.TabIndex = 1;
            // 
            // btnChonMon
            // 
            btnChonMon.Location = new Point(10, 130);
            btnChonMon.Name = "btnChonMon";
            btnChonMon.Size = new Size(60, 34);
            btnChonMon.TabIndex = 0;
            btnChonMon.Text = ">>";
            btnChonMon.UseVisualStyleBackColor = true;
            btnChonMon.Click += btnChonMon_Click;
            // 
            // btnBoMon
            // 
            btnBoMon.Location = new Point(10, 180);
            btnBoMon.Name = "btnBoMon";
            btnBoMon.Size = new Size(60, 34);
            btnBoMon.TabIndex = 1;
            btnBoMon.Text = "<<";
            btnBoMon.UseVisualStyleBackColor = true;
            btnBoMon.Click += btnBoMon_Click;
            // 
            // grpChuaDangKy
            // 
            grpChuaDangKy.Controls.Add(lstChuaDangKy);
            grpChuaDangKy.Dock = DockStyle.Left;
            grpChuaDangKy.Location = new Point(0, 0);
            grpChuaDangKy.Name = "grpChuaDangKy";
            grpChuaDangKy.Size = new Size(340, 456);
            grpChuaDangKy.TabIndex = 0;
            grpChuaDangKy.TabStop = false;
            grpChuaDangKy.Text = "Môn chưa đăng ký";
            // 
            // lstChuaDangKy
            // 
            lstChuaDangKy.Dock = DockStyle.Fill;
            lstChuaDangKy.ItemHeight = 20;
            lstChuaDangKy.Location = new Point(3, 23);
            lstChuaDangKy.Name = "lstChuaDangKy";
            lstChuaDangKy.SelectionMode = SelectionMode.MultiExtended;
            lstChuaDangKy.Size = new Size(334, 430);
            lstChuaDangKy.TabIndex = 0;
            lstChuaDangKy.DoubleClick += lstChuaDangKy_DoubleClick;
            // 
            // pnlDkTop
            // 
            pnlDkTop.Controls.Add(lblLop);
            pnlDkTop.Controls.Add(cboLop);
            pnlDkTop.Controls.Add(lblSinhVien);
            pnlDkTop.Controls.Add(cboSinhVien);
            pnlDkTop.Controls.Add(lblHocKy);
            pnlDkTop.Controls.Add(cboHocKy);
            pnlDkTop.Dock = DockStyle.Top;
            pnlDkTop.Location = new Point(3, 3);
            pnlDkTop.Name = "pnlDkTop";
            pnlDkTop.Size = new Size(986, 60);
            pnlDkTop.TabIndex = 0;
            // 
            // lblLop
            // 
            lblLop.AutoSize = true;
            lblLop.Location = new Point(14, 18);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(40, 20);
            lblLop.TabIndex = 0;
            lblLop.Text = "Lớp:";
            // 
            // cboLop
            // 
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.FormattingEnabled = true;
            cboLop.Location = new Point(60, 14);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(130, 28);
            cboLop.TabIndex = 1;
            cboLop.SelectedIndexChanged += cboLop_SelectedIndexChanged;
            // 
            // lblSinhVien
            // 
            lblSinhVien.AutoSize = true;
            lblSinhVien.Location = new Point(206, 18);
            lblSinhVien.Name = "lblSinhVien";
            lblSinhVien.Size = new Size(74, 20);
            lblSinhVien.TabIndex = 2;
            lblSinhVien.Text = "Sinh viên:";
            // 
            // cboSinhVien
            // 
            cboSinhVien.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSinhVien.FormattingEnabled = true;
            cboSinhVien.Location = new Point(286, 14);
            cboSinhVien.Name = "cboSinhVien";
            cboSinhVien.Size = new Size(260, 28);
            cboSinhVien.TabIndex = 3;
            cboSinhVien.SelectedIndexChanged += cboSinhVien_SelectedIndexChanged;
            // 
            // lblHocKy
            // 
            lblHocKy.AutoSize = true;
            lblHocKy.Location = new Point(566, 18);
            lblHocKy.Name = "lblHocKy";
            lblHocKy.Size = new Size(58, 20);
            lblHocKy.TabIndex = 4;
            lblHocKy.Text = "Học kỳ:";
            // 
            // cboHocKy
            // 
            cboHocKy.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHocKy.FormattingEnabled = true;
            cboHocKy.Items.AddRange(new object[] { "HK1", "HK2" });
            cboHocKy.Location = new Point(630, 14);
            cboHocKy.Name = "cboHocKy";
            cboHocKy.Size = new Size(90, 28);
            cboHocKy.TabIndex = 5;
            cboHocKy.SelectedIndexChanged += cboHocKy_SelectedIndexChanged;
            // 
            // pnlDkBottom
            // 
            pnlDkBottom.Controls.Add(lblTongTinChi);
            pnlDkBottom.Controls.Add(lblGioiHan);
            pnlDkBottom.Controls.Add(btnLuuDangKy);
            pnlDkBottom.Controls.Add(btnLamMoiDangKy);
            pnlDkBottom.Dock = DockStyle.Bottom;
            pnlDkBottom.Location = new Point(3, 519);
            pnlDkBottom.Name = "pnlDkBottom";
            pnlDkBottom.Size = new Size(986, 65);
            pnlDkBottom.TabIndex = 2;
            // 
            // lblTongTinChi
            // 
            lblTongTinChi.AutoSize = true;
            lblTongTinChi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTongTinChi.Location = new Point(14, 12);
            lblTongTinChi.Name = "lblTongTinChi";
            lblTongTinChi.Size = new Size(149, 20);
            lblTongTinChi.TabIndex = 0;
            lblTongTinChi.Text = "Tổng tín chỉ: 0";
            // 
            // lblGioiHan
            // 
            lblGioiHan.AutoSize = true;
            lblGioiHan.ForeColor = SystemColors.GrayText;
            lblGioiHan.Location = new Point(14, 36);
            lblGioiHan.Name = "lblGioiHan";
            lblGioiHan.Size = new Size(247, 20);
            lblGioiHan.TabIndex = 1;
            lblGioiHan.Text = "Giới hạn tín chỉ đọc từ App.config";
            // 
            // btnLuuDangKy
            // 
            btnLuuDangKy.Location = new Point(380, 14);
            btnLuuDangKy.Name = "btnLuuDangKy";
            btnLuuDangKy.Size = new Size(130, 34);
            btnLuuDangKy.TabIndex = 2;
            btnLuuDangKy.Text = "Lưu đăng ký";
            btnLuuDangKy.UseVisualStyleBackColor = true;
            btnLuuDangKy.Click += btnLuuDangKy_Click;
            // 
            // btnLamMoiDangKy
            // 
            btnLamMoiDangKy.Location = new Point(520, 14);
            btnLamMoiDangKy.Name = "btnLamMoiDangKy";
            btnLamMoiDangKy.Size = new Size(130, 34);
            btnLamMoiDangKy.TabIndex = 3;
            btnLamMoiDangKy.Text = "Làm mới";
            btnLamMoiDangKy.UseVisualStyleBackColor = true;
            btnLamMoiDangKy.Click += btnLamMoiDangKy_Click;
            // 
            // FrmDangKy
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 620);
            Controls.Add(tabControl1);
            Name = "FrmDangKy";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Đăng ký Học phần (SV2)";
            Load += FrmDangKy_Load;
            tabControl1.ResumeLayout(false);
            tabMonHoc.ResumeLayout(false);
            pnlMonEdit.ResumeLayout(false);
            pnlMonEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudSoTC).EndInit();
            tabDangKy.ResumeLayout(false);
            pnlDkCenter.ResumeLayout(false);
            grpDaDangKy.ResumeLayout(false);
            pnlChuyen.ResumeLayout(false);
            grpChuaDangKy.ResumeLayout(false);
            pnlDkTop.ResumeLayout(false);
            pnlDkTop.PerformLayout();
            pnlDkBottom.ResumeLayout(false);
            pnlDkBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabMonHoc;
        private TabPage tabDangKy;
        private ListView lvMonHoc;
        private ColumnHeader colMaMon;
        private ColumnHeader colTenMon;
        private ColumnHeader colSoTC;
        private ColumnHeader colTienQuyet;
        private Panel pnlMonEdit;
        private Label lblMaMon;
        private TextBox txtMaMon;
        private Label lblTenMon;
        private TextBox txtTenMon;
        private Label lblSoTC;
        private NumericUpDown nudSoTC;
        private Label lblTienQuyet;
        private ComboBox cboTienQuyet;
        private Button btnThemMon;
        private Button btnSuaMon;
        private Button btnXoaMon;
        private Label lblMonGhiChu;
        private Panel pnlDkCenter;
        private GroupBox grpDaDangKy;
        private ListBox lstDaDangKy;
        private Panel pnlChuyen;
        private Button btnChonMon;
        private Button btnBoMon;
        private GroupBox grpChuaDangKy;
        private ListBox lstChuaDangKy;
        private Panel pnlDkTop;
        private Label lblLop;
        private ComboBox cboLop;
        private Label lblSinhVien;
        private ComboBox cboSinhVien;
        private Label lblHocKy;
        private ComboBox cboHocKy;
        private Panel pnlDkBottom;
        private Label lblTongTinChi;
        private Label lblGioiHan;
        private Button btnLuuDangKy;
        private Button btnLamMoiDangKy;
    }
}
