namespace BCWF.Forms
{
    partial class FrmSinhVien
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
            pnlRight = new Panel();
            lvSinhVien = new ListView();
            colMSSV = new ColumnHeader();
            colHoTen = new ColumnHeader();
            colNgaySinh = new ColumnHeader();
            colGioiTinh = new ColumnHeader();
            colMaLop = new ColumnHeader();
            colAnh = new ColumnHeader();
            pnlEdit = new Panel();
            lblMSSV = new Label();
            txtMSSV = new BCWF.Common.TextBoxIme();
            lblHoTen = new Label();
            txtHoTen = new BCWF.Common.TextBoxIme();
            lblNgaySinh = new Label();
            dtpNgaySinh = new DateTimePicker();
            lblGioiTinh = new Label();
            radNam = new RadioButton();
            radNu = new RadioButton();
            lblMaLop = new Label();
            cboLop = new ComboBox();
            picAnh = new PictureBox();
            btnChonAnh = new Button();
            btnThem = new Button();
            btnSua = new Button();
            btnHuy = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnNhapCsv = new Button();
            pnlLeft = new Panel();
            treeKhoaLop = new TreeView();
            lblTreeTitle = new Label();
            pnlRight.SuspendLayout();
            pnlEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picAnh).BeginInit();
            pnlLeft.SuspendLayout();
            SuspendLayout();
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(lvSinhVien);
            pnlRight.Controls.Add(pnlEdit);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(236, 0);
            pnlRight.Margin = new Padding(3, 2, 3, 2);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(770, 495);
            pnlRight.TabIndex = 1;
            // 
            // lvSinhVien
            // 
            lvSinhVien.Columns.AddRange(new ColumnHeader[] { colMSSV, colHoTen, colNgaySinh, colGioiTinh, colMaLop, colAnh });
            lvSinhVien.Dock = DockStyle.Fill;
            lvSinhVien.FullRowSelect = true;
            lvSinhVien.GridLines = true;
            lvSinhVien.Location = new Point(0, 161);
            lvSinhVien.Margin = new Padding(3, 2, 3, 2);
            lvSinhVien.MultiSelect = false;
            lvSinhVien.Name = "lvSinhVien";
            lvSinhVien.Size = new Size(770, 334);
            lvSinhVien.TabIndex = 1;
            lvSinhVien.UseCompatibleStateImageBehavior = false;
            lvSinhVien.View = View.Details;
            lvSinhVien.SelectedIndexChanged += lvSinhVien_SelectedIndexChanged;
            // 
            // colMSSV
            // 
            colMSSV.Text = "MSSV";
            colMSSV.Width = 140;
            // 
            // colHoTen
            // 
            colHoTen.Text = "Họ tên";
            colHoTen.Width = 210;
            // 
            // colNgaySinh
            // 
            colNgaySinh.Text = "Ngày sinh";
            colNgaySinh.Width = 150;
            // 
            // colGioiTinh
            // 
            colGioiTinh.Text = "Giới tính";
            colGioiTinh.Width = 100;
            // 
            // colMaLop
            // 
            colMaLop.Text = "Lớp";
            colMaLop.Width = 120;
            // 
            // colAnh
            // 
            colAnh.Text = "Ảnh thẻ";
            colAnh.Width = 160;
            // 
            // pnlEdit
            // 
            pnlEdit.Controls.Add(lblMSSV);
            pnlEdit.Controls.Add(txtMSSV);
            pnlEdit.Controls.Add(lblHoTen);
            pnlEdit.Controls.Add(txtHoTen);
            pnlEdit.Controls.Add(lblNgaySinh);
            pnlEdit.Controls.Add(dtpNgaySinh);
            pnlEdit.Controls.Add(lblGioiTinh);
            pnlEdit.Controls.Add(radNam);
            pnlEdit.Controls.Add(radNu);
            pnlEdit.Controls.Add(lblMaLop);
            pnlEdit.Controls.Add(cboLop);
            pnlEdit.Controls.Add(picAnh);
            pnlEdit.Controls.Add(btnChonAnh);
            pnlEdit.Controls.Add(btnThem);
            pnlEdit.Controls.Add(btnSua);
            pnlEdit.Controls.Add(btnHuy);
            pnlEdit.Controls.Add(btnXoa);
            pnlEdit.Controls.Add(btnLamMoi);
            pnlEdit.Controls.Add(btnNhapCsv);
            pnlEdit.Dock = DockStyle.Top;
            pnlEdit.Location = new Point(0, 0);
            pnlEdit.Margin = new Padding(3, 2, 3, 2);
            pnlEdit.Name = "pnlEdit";
            pnlEdit.Size = new Size(770, 161);
            pnlEdit.TabIndex = 0;
            // 
            // lblMSSV
            // 
            lblMSSV.AutoSize = true;
            lblMSSV.Location = new Point(12, 14);
            lblMSSV.Name = "lblMSSV";
            lblMSSV.Size = new Size(40, 15);
            lblMSSV.TabIndex = 0;
            lblMSSV.Text = "MSSV:";
            // 
            // txtMSSV
            // 
            txtMSSV.Location = new Point(86, 11);
            txtMSSV.Margin = new Padding(3, 2, 3, 2);
            txtMSSV.Name = "txtMSSV";
            txtMSSV.Size = new Size(132, 23);
            txtMSSV.TabIndex = 1;
            // 
            // lblHoTen
            // 
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new Point(12, 41);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(46, 15);
            lblHoTen.TabIndex = 2;
            lblHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(86, 38);
            txtHoTen.Margin = new Padding(3, 2, 3, 2);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(219, 23);
            txtHoTen.TabIndex = 3;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new Point(12, 68);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(63, 15);
            lblNgaySinh.TabIndex = 4;
            lblNgaySinh.Text = "Ngày sinh:";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(86, 65);
            dtpNgaySinh.Margin = new Padding(3, 2, 3, 2);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(132, 23);
            dtpNgaySinh.TabIndex = 5;
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.AutoSize = true;
            lblGioiTinh.Location = new Point(12, 95);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new Size(55, 15);
            lblGioiTinh.TabIndex = 6;
            lblGioiTinh.Text = "Giới tính:";
            // 
            // radNam
            // 
            radNam.AutoSize = true;
            radNam.Checked = true;
            radNam.Location = new Point(86, 94);
            radNam.Margin = new Padding(3, 2, 3, 2);
            radNam.Name = "radNam";
            radNam.Size = new Size(51, 19);
            radNam.TabIndex = 7;
            radNam.TabStop = true;
            radNam.Text = "Nam";
            radNam.UseVisualStyleBackColor = true;
            // 
            // radNu
            // 
            radNu.AutoSize = true;
            radNu.Location = new Point(150, 94);
            radNu.Margin = new Padding(3, 2, 3, 2);
            radNu.Name = "radNu";
            radNu.Size = new Size(41, 19);
            radNu.TabIndex = 8;
            radNu.Text = "Nữ";
            radNu.UseVisualStyleBackColor = true;
            // 
            // lblMaLop
            // 
            lblMaLop.AutoSize = true;
            lblMaLop.Location = new Point(319, 14);
            lblMaLop.Name = "lblMaLop";
            lblMaLop.Size = new Size(30, 15);
            lblMaLop.TabIndex = 9;
            lblMaLop.Text = "Lớp:";
            // 
            // cboLop
            // 
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.FormattingEnabled = true;
            cboLop.Location = new Point(360, 11);
            cboLop.Margin = new Padding(3, 2, 3, 2);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(132, 23);
            cboLop.TabIndex = 10;
            // 
            // picAnh
            // 
            picAnh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picAnh.BorderStyle = BorderStyle.FixedSingle;
            picAnh.Location = new Point(542, 9);
            picAnh.Margin = new Padding(3, 2, 3, 2);
            picAnh.Name = "picAnh";
            picAnh.Size = new Size(88, 90);
            picAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            picAnh.TabIndex = 11;
            picAnh.TabStop = false;
            // 
            // btnChonAnh
            // 
            btnChonAnh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnChonAnh.Location = new Point(639, 9);
            btnChonAnh.Margin = new Padding(3, 2, 3, 2);
            btnChonAnh.Name = "btnChonAnh";
            btnChonAnh.Size = new Size(114, 24);
            btnChonAnh.TabIndex = 12;
            btnChonAnh.Text = "Chọn ảnh thẻ...";
            btnChonAnh.UseVisualStyleBackColor = true;
            btnChonAnh.Click += btnChonAnh_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(80, 123);
            btnThem.Margin = new Padding(3, 2, 3, 2);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(83, 24);
            btnThem.TabIndex = 13;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(172, 123);
            btnSua.Margin = new Padding(3, 2, 3, 2);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(83, 24);
            btnSua.TabIndex = 14;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnHuy
            // 
            btnHuy.Enabled = false;
            btnHuy.Location = new Point(265, 123);
            btnHuy.Margin = new Padding(3, 2, 3, 2);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(83, 24);
            btnHuy.TabIndex = 15;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            btnHuy.Click += btnHuy_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(358, 123);
            btnXoa.Margin = new Padding(3, 2, 3, 2);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(83, 24);
            btnXoa.TabIndex = 16;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(451, 123);
            btnLamMoi.Margin = new Padding(3, 2, 3, 2);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(83, 24);
            btnLamMoi.TabIndex = 17;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnNhapCsv
            // 
            btnNhapCsv.Location = new Point(543, 123);
            btnNhapCsv.Margin = new Padding(3, 2, 3, 2);
            btnNhapCsv.Name = "btnNhapCsv";
            btnNhapCsv.Size = new Size(140, 24);
            btnNhapCsv.TabIndex = 18;
            btnNhapCsv.Text = "Nhập từ CSV...";
            btnNhapCsv.UseVisualStyleBackColor = true;
            btnNhapCsv.Click += btnNhapCsv_Click;
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(treeKhoaLop);
            pnlLeft.Controls.Add(lblTreeTitle);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Margin = new Padding(3, 2, 3, 2);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(236, 495);
            pnlLeft.TabIndex = 0;
            // 
            // treeKhoaLop
            // 
            treeKhoaLop.Dock = DockStyle.Fill;
            treeKhoaLop.FullRowSelect = true;
            treeKhoaLop.HideSelection = false;
            treeKhoaLop.Location = new Point(0, 27);
            treeKhoaLop.Margin = new Padding(3, 2, 3, 2);
            treeKhoaLop.Name = "treeKhoaLop";
            treeKhoaLop.Size = new Size(236, 468);
            treeKhoaLop.TabIndex = 1;
            treeKhoaLop.AfterSelect += treeKhoaLop_AfterSelect;
            // 
            // lblTreeTitle
            // 
            lblTreeTitle.Dock = DockStyle.Top;
            lblTreeTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTreeTitle.Location = new Point(0, 0);
            lblTreeTitle.Name = "lblTreeTitle";
            lblTreeTitle.Size = new Size(236, 27);
            lblTreeTitle.TabIndex = 0;
            lblTreeTitle.Text = "KHOA - LỚP";
            lblTreeTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmSinhVien
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 495);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FrmSinhVien";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Quản lý Sinh viên - Lớp (SV1)";
            Load += FrmSinhVien_Load;
            pnlRight.ResumeLayout(false);
            pnlEdit.ResumeLayout(false);
            pnlEdit.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picAnh).EndInit();
            pnlLeft.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlRight;
        private Panel pnlLeft;
        private Panel pnlEdit;
        private Label lblTreeTitle;
        private TreeView treeKhoaLop;
        private ListView lvSinhVien;
        private ColumnHeader colMSSV;
        private ColumnHeader colHoTen;
        private ColumnHeader colNgaySinh;
        private ColumnHeader colGioiTinh;
        private ColumnHeader colMaLop;
        private ColumnHeader colAnh;
        private Label lblMSSV;
        private BCWF.Common.TextBoxIme txtMSSV;
        private Label lblHoTen;
        private BCWF.Common.TextBoxIme txtHoTen;
        private Label lblNgaySinh;
        private DateTimePicker dtpNgaySinh;
        private Label lblGioiTinh;
        private RadioButton radNam;
        private RadioButton radNu;
        private Label lblMaLop;
        private ComboBox cboLop;
        private PictureBox picAnh;
        private Button btnChonAnh;
        private Button btnThem;
        private Button btnSua;
        private Button btnHuy;
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnNhapCsv;
    }
}
