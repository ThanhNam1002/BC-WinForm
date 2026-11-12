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
            lblGhiChu = new Label();
            lvCanhBao = new ListView();
            colMSSV = new ColumnHeader();
            colHoTen = new ColumnHeader();
            colLop = new ColumnHeader();
            colDiemTichLuy = new ColumnHeader();
            colMucCanhBao = new ColumnHeader();
            pnlBottom = new Panel();
            lblTongHop = new Label();
            btnTaiLai = new Button();
            btnXuatCsv = new Button();
            pnlTop.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(lblGhiChu);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(820, 55);
            pnlTop.TabIndex = 0;
            // 
            // lblGhiChu
            // 
            lblGhiChu.Location = new Point(14, 16);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(780, 24);
            lblGhiChu.TabIndex = 0;
            lblGhiChu.Text = "Sinh viên có điểm trung bình tích lũy dưới ngưỡng DiemCanhBao trong App.config.";
            // 
            // lvCanhBao
            // 
            lvCanhBao.Columns.AddRange(new ColumnHeader[] { colMSSV, colHoTen, colLop, colDiemTichLuy, colMucCanhBao });
            lvCanhBao.Dock = DockStyle.Fill;
            lvCanhBao.FullRowSelect = true;
            lvCanhBao.GridLines = true;
            lvCanhBao.Location = new Point(0, 55);
            lvCanhBao.MultiSelect = false;
            lvCanhBao.Name = "lvCanhBao";
            lvCanhBao.Size = new Size(820, 355);
            lvCanhBao.TabIndex = 1;
            lvCanhBao.UseCompatibleStateImageBehavior = false;
            lvCanhBao.View = View.Details;
            // 
            // colMSSV
            // 
            colMSSV.Text = "MSSV";
            colMSSV.Width = 110;
            // 
            // colHoTen
            // 
            colHoTen.Text = "Họ tên";
            colHoTen.Width = 220;
            // 
            // colLop
            // 
            colLop.Text = "Lớp";
            colLop.Width = 110;
            // 
            // colDiemTichLuy
            // 
            colDiemTichLuy.Text = "ĐTB tích lũy";
            colDiemTichLuy.TextAlign = HorizontalAlignment.Center;
            colDiemTichLuy.Width = 120;
            // 
            // colMucCanhBao
            // 
            colMucCanhBao.Text = "Mức cảnh báo";
            colMucCanhBao.Width = 160;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(lblTongHop);
            pnlBottom.Controls.Add(btnTaiLai);
            pnlBottom.Controls.Add(btnXuatCsv);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 410);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(820, 60);
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
            btnTaiLai.Location = new Point(530, 14);
            btnTaiLai.Name = "btnTaiLai";
            btnTaiLai.Size = new Size(110, 34);
            btnTaiLai.TabIndex = 1;
            btnTaiLai.Text = "Tải lại";
            btnTaiLai.UseVisualStyleBackColor = true;
            btnTaiLai.Click += btnTaiLai_Click;
            // 
            // btnXuatCsv
            // 
            btnXuatCsv.Location = new Point(650, 14);
            btnXuatCsv.Name = "btnXuatCsv";
            btnXuatCsv.Size = new Size(150, 34);
            btnXuatCsv.TabIndex = 2;
            btnXuatCsv.Text = "Xuất CSV";
            btnXuatCsv.UseVisualStyleBackColor = true;
            btnXuatCsv.Click += btnXuatCsv_Click;
            // 
            // FrmCanhBaoHocVu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(820, 470);
            Controls.Add(lvCanhBao);
            Controls.Add(pnlTop);
            Controls.Add(pnlBottom);
            Name = "FrmCanhBaoHocVu";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Danh sách sinh viên bị cảnh báo học vụ";
            Load += FrmCanhBaoHocVu_Load;
            pnlTop.ResumeLayout(false);
            pnlBottom.ResumeLayout(false);
            pnlBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private Label lblGhiChu;
        private ListView lvCanhBao;
        private ColumnHeader colMSSV;
        private ColumnHeader colHoTen;
        private ColumnHeader colLop;
        private ColumnHeader colDiemTichLuy;
        private ColumnHeader colMucCanhBao;
        private Panel pnlBottom;
        private Label lblTongHop;
        private Button btnTaiLai;
        private Button btnXuatCsv;
    }
}
