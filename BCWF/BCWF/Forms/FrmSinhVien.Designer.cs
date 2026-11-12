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
            txtMSSV = new TextBox();
            lblHoTen = new Label();
            txtHoTen = new TextBox();
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
            pnlRight.Location = new Point(270, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(880, 660);
            pnlRight.TabIndex = 1;
            // 
            // lvSinhVien
            // 
            lvSinhVien.Columns.AddRange(new ColumnHeader[] { colMSSV, colHoTen, colNgaySinh, colGioiTinh, colMaLop, colAnh });
            lvSinhVien.Dock = DockStyle.Fill;
            lvSinhVien.FullRowSelect = true;
            lvSinhVien.GridLines = true;
            lvSinhVien.Location = new Point(0, 215);
            lvSinhVien.MultiSelect = false;
            lvSinhVien.Name = "lvSinhVien";
            lvSinhVien.Size = new Size(880, 445);
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
            pnlEdit.Controls.Add(btnXoa);
            pnlEdit.Controls.Add(btnLamMoi);
            pnlEdit.Controls.Add(btnNhapCsv);
            pnlEdit.Dock = DockStyle.Top;
            pnlEdit.Location = new Point(0, 0);
            pnlEdit.Name = "pnlEdit";
            pnlEdit.Size = new Size(880, 215);
            pnlEdit.TabIndex = 0;
            // 
            // lblMSSV
            // 
            lblMSSV.AutoSize = true;
            lblMSSV.Location = new Point(14, 19);
            lblMSSV.Name = "lblMSSV";
            lblMSSV.Size = new Size(50, 20);
            lblMSSV.TabIndex = 0;
            lblMSSV.Text = "MSSV:";
            // 
            // txtMSSV
            // 
            txtMSSV.Location = new Point(91, 15);
            txtMSSV.Name = "txtMSSV";
            txtMSSV.Size = new Size(150, 27);
            txtMSSV.TabIndex = 1;
            // 
            // lblHoTen
            // 
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new Point(14, 55);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(57, 20);
            lblHoTen.TabIndex = 2;
            lblHoTen.Text = "Họ tên:";
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(91, 51);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(250, 27);
            txtHoTen.TabIndex = 3;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.AutoSize = true;
            lblNgaySinh.Location = new Point(14, 91);
            lblNgaySinh.Name = "lblNgaySinh";
            lblNgaySinh.Size = new Size(77, 20);
            lblNgaySinh.TabIndex = 4;
            lblNgaySinh.Text = "Ngày sinh:";
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            dtpNgaySinh.Location = new Point(91, 87);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(150, 27);
            dtpNgaySinh.TabIndex = 5;
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.AutoSize = true;
            lblGioiTinh.Location = new Point(14, 127);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new Size(68, 20);
            lblGioiTinh.TabIndex = 6;
            lblGioiTinh.Text = "Giới tính:";
            // 
            // radNam
            // 
            radNam.AutoSize = true;
            radNam.Checked = true;
            radNam.Location = new Point(91, 125);
            radNam.Name = "radNam";
            radNam.Size = new Size(62, 24);
            radNam.TabIndex = 7;
            radNam.TabStop = true;
            radNam.Text = "Nam";
            radNam.UseVisualStyleBackColor = true;
            // 
            // radNu
            // 
            radNu.AutoSize = true;
            radNu.Location = new Point(165, 125);
            radNu.Name = "radNu";
            radNu.Size = new Size(50, 24);
            radNu.TabIndex = 8;
            radNu.Text = "Nữ";
            radNu.UseVisualStyleBackColor = true;
            // 
            // lblMaLop
            // 
            lblMaLop.AutoSize = true;
            lblMaLop.Location = new Point(365, 19);
            lblMaLop.Name = "lblMaLop";
            lblMaLop.Size = new Size(37, 20);
            lblMaLop.TabIndex = 9;
            lblMaLop.Text = "Lớp:";
            // 
            // cboLop
            // 
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.FormattingEnabled = true;
            cboLop.Location = new Point(411, 15);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(150, 28);
            cboLop.TabIndex = 10;
            // 
            // picAnh
            // 
            picAnh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picAnh.BorderStyle = BorderStyle.FixedSingle;
            picAnh.Location = new Point(619, 12);
            picAnh.Name = "picAnh";
            picAnh.Size = new Size(100, 119);
            picAnh.SizeMode = PictureBoxSizeMode.StretchImage;
            picAnh.TabIndex = 11;
            picAnh.TabStop = false;
            // 
            // btnChonAnh
            // 
            btnChonAnh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnChonAnh.Location = new Point(730, 12);
            btnChonAnh.Name = "btnChonAnh";
            btnChonAnh.Size = new Size(130, 32);
            btnChonAnh.TabIndex = 12;
            btnChonAnh.Text = "Chọn ảnh thẻ...";
            btnChonAnh.UseVisualStyleBackColor = true;
            btnChonAnh.Click += btnChonAnh_Click;
            // 
            // btnThem
            // 
            btnThem.Location = new Point(91, 164);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(95, 32);
            btnThem.TabIndex = 13;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            btnThem.Click += btnThem_Click;
            // 
            // btnSua
            // 
            btnSua.Location = new Point(197, 164);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(95, 32);
            btnSua.TabIndex = 14;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Location = new Point(302, 164);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(95, 32);
            btnXoa.TabIndex = 15;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            btnXoa.Click += btnXoa_Click;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(407, 164);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(95, 32);
            btnLamMoi.TabIndex = 16;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            btnLamMoi.Click += btnLamMoi_Click;
            // 
            // btnNhapCsv
            // 
            btnNhapCsv.Location = new Point(512, 164);
            btnNhapCsv.Name = "btnNhapCsv";
            btnNhapCsv.Size = new Size(160, 32);
            btnNhapCsv.TabIndex = 17;
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
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(270, 660);
            pnlLeft.TabIndex = 0;
            // 
            // treeKhoaLop
            // 
            treeKhoaLop.Dock = DockStyle.Fill;
            treeKhoaLop.FullRowSelect = true;
            treeKhoaLop.HideSelection = false;
            treeKhoaLop.Location = new Point(0, 36);
            treeKhoaLop.Name = "treeKhoaLop";
            treeKhoaLop.Size = new Size(270, 624);
            treeKhoaLop.TabIndex = 1;
            treeKhoaLop.AfterSelect += treeKhoaLop_AfterSelect;
            // 
            // lblTreeTitle
            // 
            lblTreeTitle.Dock = DockStyle.Top;
            lblTreeTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblTreeTitle.Location = new Point(0, 0);
            lblTreeTitle.Name = "lblTreeTitle";
            lblTreeTitle.Size = new Size(270, 36);
            lblTreeTitle.TabIndex = 0;
            lblTreeTitle.Text = "KHOA - LỚP";
            lblTreeTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmSinhVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 660);
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
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
        private TextBox txtMSSV;
        private Label lblHoTen;
        private TextBox txtHoTen;
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
        private Button btnXoa;
        private Button btnLamMoi;
        private Button btnNhapCsv;
    }
}
