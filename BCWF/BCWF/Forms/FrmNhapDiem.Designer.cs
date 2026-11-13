namespace BCWF.Forms
{
    partial class FrmNhapDiem
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
            pnlTop = new Panel();
            lblLop = new Label();
            cboLop = new ComboBox();
            lblMon = new Label();
            cboMon = new ComboBox();
            lblHocKy = new Label();
            cboHocKy = new ComboBox();
            btnTaiDanhSach = new Button();
            lblHuongDan = new Label();
            lvDiem = new ListView();
            colMSSV = new ColumnHeader();
            colHoTen = new ColumnHeader();
            colDiemQT = new ColumnHeader();
            colDiemGK = new ColumnHeader();
            colDiemCK = new ColumnHeader();
            colTongKet = new ColumnHeader();
            colXepLoai = new ColumnHeader();
            colDtbTichLuy = new ColumnHeader();
            colGhiChu = new ColumnHeader();
            pnlBottom = new Panel();
            lblTongKet = new Label();
            btnLuuDiem = new Button();
            btnXuatCsv = new Button();
            btnTaiLai = new Button();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblLop);
            pnlTop.Controls.Add(cboLop);
            pnlTop.Controls.Add(lblMon);
            pnlTop.Controls.Add(cboMon);
            pnlTop.Controls.Add(lblHocKy);
            pnlTop.Controls.Add(cboHocKy);
            pnlTop.Controls.Add(btnTaiDanhSach);
            pnlTop.Controls.Add(lblHuongDan);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1000, 95);
            pnlTop.TabIndex = 0;
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
            // lblMon
            // 
            lblMon.AutoSize = true;
            lblMon.Location = new Point(206, 18);
            lblMon.Name = "lblMon";
            lblMon.Size = new Size(65, 20);
            lblMon.TabIndex = 2;
            lblMon.Text = "Môn học:";
            // 
            // cboMon
            // 
            cboMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboMon.FormattingEnabled = true;
            cboMon.Location = new Point(280, 14);
            cboMon.Name = "cboMon";
            cboMon.Size = new Size(250, 28);
            cboMon.TabIndex = 3;
            cboMon.SelectedIndexChanged += cboMon_SelectedIndexChanged;
            // 
            // lblHocKy
            // 
            lblHocKy.AutoSize = true;
            lblHocKy.Location = new Point(550, 18);
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
            cboHocKy.Location = new Point(614, 14);
            cboHocKy.Name = "cboHocKy";
            cboHocKy.Size = new Size(90, 28);
            cboHocKy.TabIndex = 5;
            cboHocKy.SelectedIndexChanged += cboHocKy_SelectedIndexChanged;
            // 
            // btnTaiDanhSach
            // 
            btnTaiDanhSach.Location = new Point(720, 12);
            btnTaiDanhSach.Name = "btnTaiDanhSach";
            btnTaiDanhSach.Size = new Size(140, 32);
            btnTaiDanhSach.TabIndex = 6;
            btnTaiDanhSach.Text = "Tải danh sách";
            btnTaiDanhSach.UseVisualStyleBackColor = true;
            btnTaiDanhSach.Click += btnTaiDanhSach_Click;
            // 
            // lblHuongDan
            // 
            lblHuongDan.ForeColor = SystemColors.GrayText;
            lblHuongDan.Location = new Point(14, 56);
            lblHuongDan.Name = "lblHuongDan";
            lblHuongDan.Size = new Size(700, 24);
            lblHuongDan.TabIndex = 7;
            lblHuongDan.Text = "Nhấp vào ô Điểm QT / Điểm GK / Điểm CK trên danh sách để sửa điểm trực tiếp.";
            // 
            // lvDiem
            // 
            lvDiem.Columns.AddRange(new ColumnHeader[] { colMSSV, colHoTen, colDiemQT, colDiemGK, colDiemCK, colTongKet, colXepLoai, colDtbTichLuy, colGhiChu });
            lvDiem.Dock = DockStyle.Fill;
            lvDiem.FullRowSelect = true;
            lvDiem.GridLines = true;
            lvDiem.LabelEdit = true;
            lvDiem.Location = new Point(0, 95);
            lvDiem.MultiSelect = false;
            lvDiem.Name = "lvDiem";
            lvDiem.Size = new Size(1000, 465);
            lvDiem.TabIndex = 1;
            lvDiem.UseCompatibleStateImageBehavior = false;
            lvDiem.View = View.Details;
            lvDiem.AfterLabelEdit += lvDiem_AfterLabelEdit;
            // 
            // colMSSV
            // 
            colMSSV.Text = "MSSV";
            colMSSV.Width = 100;
            // 
            // colHoTen
            // 
            colHoTen.Text = "Họ tên";
            colHoTen.Width = 200;
            // 
            // colDiemQT
            // 
            colDiemQT.Text = "Điểm QT";
            colDiemQT.TextAlign = HorizontalAlignment.Center;
            colDiemQT.Width = 80;
            // 
            // colDiemGK
            // 
            colDiemGK.Text = "Điểm GK";
            colDiemGK.TextAlign = HorizontalAlignment.Center;
            colDiemGK.Width = 80;
            // 
            // colDiemCK
            // 
            colDiemCK.Text = "Điểm CK";
            colDiemCK.TextAlign = HorizontalAlignment.Center;
            colDiemCK.Width = 80;
            // 
            // colTongKet
            // 
            colTongKet.Text = "Tổng kết";
            colTongKet.TextAlign = HorizontalAlignment.Center;
            colTongKet.Width = 100;
            // 
            // colXepLoai
            // 
            colXepLoai.Text = "Xếp loại";
            colXepLoai.Width = 100;
            // 
            // colDtbTichLuy
            // 
            colDtbTichLuy.Text = "ĐTB tích lũy";
            colDtbTichLuy.TextAlign = HorizontalAlignment.Center;
            colDtbTichLuy.Width = 110;
            // 
            // colGhiChu
            // 
            colGhiChu.Text = "Ghi chú";
            colGhiChu.Width = 140;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(lblTongKet);
            pnlBottom.Controls.Add(btnLuuDiem);
            pnlBottom.Controls.Add(btnXuatCsv);
            pnlBottom.Controls.Add(btnTaiLai);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 560);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1000, 60);
            pnlBottom.TabIndex = 2;
            // 
            // lblTongKet
            // 
            lblTongKet.AutoSize = true;
            lblTongKet.Location = new Point(14, 20);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(280, 20);
            lblTongKet.TabIndex = 0;
            lblTongKet.Text = "Số sinh viên: 0 - Số môn rớt: 0";
            // 
            // btnLuuDiem
            // 
            btnLuuDiem.Location = new Point(470, 14);
            btnLuuDiem.Name = "btnLuuDiem";
            btnLuuDiem.Size = new Size(110, 34);
            btnLuuDiem.TabIndex = 1;
            btnLuuDiem.Text = "Lưu điểm";
            btnLuuDiem.UseVisualStyleBackColor = true;
            btnLuuDiem.Click += btnLuuDiem_Click;
            // 
            // btnXuatCsv
            // 
            btnXuatCsv.Location = new Point(590, 14);
            btnXuatCsv.Name = "btnXuatCsv";
            btnXuatCsv.Size = new Size(150, 34);
            btnXuatCsv.TabIndex = 2;
            btnXuatCsv.Text = "Xuất bảng điểm CSV";
            btnXuatCsv.UseVisualStyleBackColor = true;
            btnXuatCsv.Click += btnXuatCsv_Click;
            // 
            // btnTaiLai
            // 
            btnTaiLai.Location = new Point(750, 14);
            btnTaiLai.Name = "btnTaiLai";
            btnTaiLai.Size = new Size(110, 34);
            btnTaiLai.TabIndex = 3;
            btnTaiLai.Text = "Tải lại";
            btnTaiLai.UseVisualStyleBackColor = true;
            btnTaiLai.Click += btnTaiLai_Click;
            // 
            // FrmNhapDiem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 620);
            Controls.Add(lvDiem);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            Name = "FrmNhapDiem";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nhập điểm - Bảng điểm (SV3)";
            Load += FrmNhapDiem_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private Label lblLop;
        private ComboBox cboLop;
        private Label lblMon;
        private ComboBox cboMon;
        private Label lblHocKy;
        private ComboBox cboHocKy;
        private Button btnTaiDanhSach;
        private Label lblHuongDan;
        private ListView lvDiem;
        private ColumnHeader colMSSV;
        private ColumnHeader colHoTen;
        private ColumnHeader colDiemQT;
        private ColumnHeader colDiemGK;
        private ColumnHeader colDiemCK;
        private ColumnHeader colTongKet;
        private ColumnHeader colXepLoai;
        private ColumnHeader colDtbTichLuy;
        private ColumnHeader colGhiChu;
        private Panel pnlBottom;
        private Label lblTongKet;
        private Button btnLuuDiem;
        private Button btnXuatCsv;
        private Button btnTaiLai;
    }
}
