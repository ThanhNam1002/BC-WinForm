namespace BCWF.Forms
{
    partial class FrmCanhBaoHocVu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed components should be disposed.</param>
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
            pnlLoc = new Panel();
            lblLop = new Label();
            cboLop = new ComboBox();
            lblHocKy = new Label();
            cboHocKy = new ComboBox();
            lblTimKiem = new Label();
            txtTimKiem = new TextBox();
            btnXoaTim = new Button();
            lblGhiChu = new Label();
            lvCanhBao = new ListView();
            colMSSV = new ColumnHeader();
            colHoTen = new ColumnHeader();
            colLop = new ColumnHeader();
            colDiemTichLuy = new ColumnHeader();
            colDiemHocKy = new ColumnHeader();
            colSoMonRot = new ColumnHeader();
            colTinChiRot = new ColumnHeader();
            colMucCanhBao = new ColumnHeader();
            colMonRot = new ColumnHeader();
            pnlBottom = new Panel();
            lblTongHop = new Label();
            btnTaiLai = new Button();
            btnXemBangDiem = new Button();
            btnXuatCsv = new Button();
            pnlTop.SuspendLayout();
            pnlLoc.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            //
            // pnlTop
            //
            pnlTop.Controls.Add(pnlLoc);
            pnlTop.Controls.Add(lblGhiChu);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(1040, 86);
            pnlTop.TabIndex = 0;
            //
            // pnlLoc
            //
            pnlLoc.Controls.Add(btnXoaTim);
            pnlLoc.Controls.Add(txtTimKiem);
            pnlLoc.Controls.Add(lblTimKiem);
            pnlLoc.Controls.Add(cboHocKy);
            pnlLoc.Controls.Add(lblHocKy);
            pnlLoc.Controls.Add(cboLop);
            pnlLoc.Controls.Add(lblLop);
            pnlLoc.Dock = DockStyle.Bottom;
            pnlLoc.Location = new Point(0, 34);
            pnlLoc.Name = "pnlLoc";
            pnlLoc.Size = new Size(1040, 52);
            pnlLoc.TabIndex = 1;
            //
            // lblLop
            //
            lblLop.AutoSize = true;
            lblLop.Location = new Point(14, 17);
            lblLop.Name = "lblLop";
            lblLop.Size = new Size(29, 20);
            lblLop.TabIndex = 0;
            lblLop.Text = "Lớp:";
            //
            // cboLop
            //
            cboLop.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLop.Location = new Point(50, 13);
            cboLop.Name = "cboLop";
            cboLop.Size = new Size(200, 28);
            cboLop.TabIndex = 1;
            //
            // lblHocKy
            //
            lblHocKy.AutoSize = true;
            lblHocKy.Location = new Point(266, 17);
            lblHocKy.Name = "lblHocKy";
            lblHocKy.Size = new Size(57, 20);
            lblHocKy.TabIndex = 2;
            lblHocKy.Text = "Học kỳ:";
            //
            // cboHocKy
            //
            cboHocKy.DropDownStyle = ComboBoxStyle.DropDownList;
            cboHocKy.Location = new Point(330, 13);
            cboHocKy.Name = "cboHocKy";
            cboHocKy.Size = new Size(140, 28);
            cboHocKy.TabIndex = 3;
            //
            // lblTimKiem
            //
            lblTimKiem.AutoSize = true;
            lblTimKiem.Location = new Point(486, 17);
            lblTimKiem.Name = "lblTimKiem";
            lblTimKiem.Size = new Size(38, 20);
            lblTimKiem.TabIndex = 4;
            lblTimKiem.Text = "Tìm:";
            //
            // txtTimKiem
            //
            txtTimKiem.Location = new Point(530, 13);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(220, 28);
            txtTimKiem.TabIndex = 5;
            //
            // btnXoaTim
            //
            btnXoaTim.Location = new Point(760, 12);
            btnXoaTim.Name = "btnXoaTim";
            btnXoaTim.Size = new Size(110, 30);
            btnXoaTim.TabIndex = 6;
            btnXoaTim.Text = "Xóa tìm";
            btnXoaTim.UseVisualStyleBackColor = true;
            btnXoaTim.Click += btnXoaTim_Click;
            //
            // lblGhiChu
            //
            lblGhiChu.Dock = DockStyle.Top;
            lblGhiChu.Location = new Point(0, 0);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(1040, 34);
            lblGhiChu.TabIndex = 0;
            lblGhiChu.Text = "Sinh viên có điểm trung bình tích lũy dưới ngưỡng cảnh báo trong App.config.";
            //
            // lvCanhBao
            //
            lvCanhBao.Columns.AddRange(new ColumnHeader[] { colMSSV, colHoTen, colLop, colDiemTichLuy, colDiemHocKy, colSoMonRot, colTinChiRot, colMucCanhBao, colMonRot });
            lvCanhBao.Dock = DockStyle.Fill;
            lvCanhBao.FullRowSelect = true;
            lvCanhBao.GridLines = true;
            lvCanhBao.HideSelection = false;
            lvCanhBao.Location = new Point(0, 86);
            lvCanhBao.MultiSelect = false;
            lvCanhBao.Name = "lvCanhBao";
            lvCanhBao.Size = new Size(1040, 384);
            lvCanhBao.TabIndex = 1;
            lvCanhBao.UseCompatibleStateImageBehavior = false;
            lvCanhBao.View = View.Details;
            lvCanhBao.DoubleClick += lvCanhBao_DoubleClick;
            //
            // colMSSV
            //
            colMSSV.Text = "MSSV";
            colMSSV.Width = 100;
            //
            // colHoTen
            //
            colHoTen.Text = "Họ tên";
            colHoTen.Width = 190;
            //
            // colLop
            //
            colLop.Text = "Lớp";
            colLop.Width = 90;
            //
            // colDiemTichLuy
            //
            colDiemTichLuy.Text = "ĐTB tích lũy";
            colDiemTichLuy.TextAlign = HorizontalAlignment.Center;
            colDiemTichLuy.Width = 100;
            //
            // colDiemHocKy
            //
            colDiemHocKy.Text = "ĐTB học kỳ";
            colDiemHocKy.TextAlign = HorizontalAlignment.Center;
            colDiemHocKy.Width = 100;
            //
            // colSoMonRot
            //
            colSoMonRot.Text = "Số môn rớt";
            colSoMonRot.TextAlign = HorizontalAlignment.Center;
            colSoMonRot.Width = 90;
            //
            // colTinChiRot
            //
            colTinChiRot.Text = "TC rớt";
            colTinChiRot.TextAlign = HorizontalAlignment.Center;
            colTinChiRot.Width = 80;
            //
            // colMucCanhBao
            //
            colMucCanhBao.Text = "Mức cảnh báo";
            colMucCanhBao.Width = 130;
            //
            // colMonRot
            //
            colMonRot.Text = "Môn bị rớt";
            colMonRot.Width = 260;
            //
            // pnlBottom
            //
            pnlBottom.Controls.Add(lblTongHop);
            pnlBottom.Controls.Add(btnTaiLai);
            pnlBottom.Controls.Add(btnXemBangDiem);
            pnlBottom.Controls.Add(btnXuatCsv);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 470);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(1040, 60);
            pnlBottom.TabIndex = 2;
            //
            // lblTongHop
            //
            lblTongHop.AutoSize = true;
            lblTongHop.Location = new Point(14, 20);
            lblTongHop.Name = "lblTongHop";
            lblTongHop.Size = new Size(154, 20);
            lblTongHop.TabIndex = 0;
            lblTongHop.Text = "Số sinh viên bị cảnh báo: 0";
            //
            // btnTaiLai
            //
            btnTaiLai.Location = new Point(560, 14);
            btnTaiLai.Name = "btnTaiLai";
            btnTaiLai.Size = new Size(110, 34);
            btnTaiLai.TabIndex = 1;
            btnTaiLai.Text = "Tải lại";
            btnTaiLai.UseVisualStyleBackColor = true;
            btnTaiLai.Click += btnTaiLai_Click;
            //
            // btnXemBangDiem
            //
            btnXemBangDiem.Location = new Point(680, 14);
            btnXemBangDiem.Name = "btnXemBangDiem";
            btnXemBangDiem.Size = new Size(160, 34);
            btnXemBangDiem.TabIndex = 2;
            btnXemBangDiem.Text = "Xem bảng điểm cá nhân";
            btnXemBangDiem.UseVisualStyleBackColor = true;
            btnXemBangDiem.Click += btnXemBangDiem_Click;
            //
            // btnXuatCsv
            //
            btnXuatCsv.Location = new Point(850, 14);
            btnXuatCsv.Name = "btnXuatCsv";
            btnXuatCsv.Size = new Size(170, 34);
            btnXuatCsv.TabIndex = 3;
            btnXuatCsv.Text = "Xuất CSV";
            btnXuatCsv.UseVisualStyleBackColor = true;
            btnXuatCsv.Click += btnXuatCsv_Click;
            //
            // FrmCanhBaoHocVu
            //
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1040, 530);
            Controls.Add(lvCanhBao);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            Name = "FrmCanhBaoHocVu";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Cảnh báo học vụ";
            Load += FrmCanhBaoHocVu_Load;
            pnlTop.ResumeLayout(false);
            pnlLoc.ResumeLayout(false);
            pnlLoc.PerformLayout();
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private Panel pnlLoc;
        private Label lblLop;
        private ComboBox cboLop;
        private Label lblHocKy;
        private ComboBox cboHocKy;
        private Label lblTimKiem;
        private TextBox txtTimKiem;
        private Button btnXoaTim;
        private Label lblGhiChu;
        private ListView lvCanhBao;
        private ColumnHeader colMSSV;
        private ColumnHeader colHoTen;
        private ColumnHeader colLop;
        private ColumnHeader colDiemTichLuy;
        private ColumnHeader colDiemHocKy;
        private ColumnHeader colSoMonRot;
        private ColumnHeader colTinChiRot;
        private ColumnHeader colMucCanhBao;
        private ColumnHeader colMonRot;
        private Panel pnlBottom;
        private Label lblTongHop;
        private Button btnTaiLai;
        private Button btnXemBangDiem;
        private Button btnXuatCsv;
    }
}
