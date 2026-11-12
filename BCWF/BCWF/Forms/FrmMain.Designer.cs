namespace BCWF
{
    partial class FrmMain
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
            menuStrip1 = new MenuStrip();
            heThongMenu = new ToolStripMenuItem();
            thoatMenuItem = new ToolStripMenuItem();
            quanLyMenu = new ToolStripMenuItem();
            quanLySinhVienLopMenuItem = new ToolStripMenuItem();
            dangKyHocPhanMenuItem = new ToolStripMenuItem();
            nhapDiemMenuItem = new ToolStripMenuItem();
            baoCaoMenu = new ToolStripMenuItem();
            canhBaoHocVuMenuItem = new ToolStripMenuItem();
            panelNoiDung = new Panel();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { heThongMenu, quanLyMenu, baoCaoMenu });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1100, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // heThongMenu
            // 
            heThongMenu.DropDownItems.AddRange(new ToolStripItem[] { thoatMenuItem });
            heThongMenu.Name = "heThongMenu";
            heThongMenu.Size = new Size(88, 24);
            heThongMenu.Text = "Hệ Thống";
            // 
            // thoatMenuItem
            // 
            thoatMenuItem.Name = "thoatMenuItem";
            thoatMenuItem.ShortcutKeys = Keys.Alt | Keys.F4;
            thoatMenuItem.Size = new Size(180, 26);
            thoatMenuItem.Text = "Thoát";
            thoatMenuItem.Click += thoatMenuItem_Click;
            // 
            // quanLyMenu
            // 
            quanLyMenu.DropDownItems.AddRange(new ToolStripItem[] { quanLySinhVienLopMenuItem, dangKyHocPhanMenuItem, nhapDiemMenuItem });
            quanLyMenu.Name = "quanLyMenu";
            quanLyMenu.Size = new Size(76, 24);
            quanLyMenu.Text = "Quản Lý";
            // 
            // quanLySinhVienLopMenuItem
            // 
            quanLySinhVienLopMenuItem.Name = "quanLySinhVienLopMenuItem";
            quanLySinhVienLopMenuItem.Size = new Size(291, 26);
            quanLySinhVienLopMenuItem.Text = "SV1 - Sinh viên && Lớp";
            quanLySinhVienLopMenuItem.Click += quanLySinhVienLopMenuItem_Click;
            // 
            // dangKyHocPhanMenuItem
            // 
            dangKyHocPhanMenuItem.Name = "dangKyHocPhanMenuItem";
            dangKyHocPhanMenuItem.Size = new Size(291, 26);
            dangKyHocPhanMenuItem.Text = "SV2 - Đăng ký học phần";
            dangKyHocPhanMenuItem.Click += dangKyHocPhanMenuItem_Click;
            // 
            // nhapDiemMenuItem
            // 
            nhapDiemMenuItem.Name = "nhapDiemMenuItem";
            nhapDiemMenuItem.Size = new Size(291, 26);
            nhapDiemMenuItem.Text = "SV3 - Nhập điểm && Bảng điểm";
            nhapDiemMenuItem.Click += nhapDiemMenuItem_Click;
            // 
            // baoCaoMenu
            // 
            baoCaoMenu.DropDownItems.AddRange(new ToolStripItem[] { canhBaoHocVuMenuItem });
            baoCaoMenu.Name = "baoCaoMenu";
            baoCaoMenu.Size = new Size(76, 24);
            baoCaoMenu.Text = "Báo Cáo";
            // 
            // canhBaoHocVuMenuItem
            // 
            canhBaoHocVuMenuItem.Name = "canhBaoHocVuMenuItem";
            canhBaoHocVuMenuItem.Size = new Size(291, 26);
            canhBaoHocVuMenuItem.Text = "Danh sách cảnh báo học vụ";
            canhBaoHocVuMenuItem.Click += canhBaoHocVuMenuItem_Click;
            // 
            // panelNoiDung
            // 
            panelNoiDung.Dock = DockStyle.Fill;
            panelNoiDung.Location = new Point(0, 28);
            panelNoiDung.Name = "panelNoiDung";
            panelNoiDung.Size = new Size(1100, 622);
            panelNoiDung.TabIndex = 1;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(panelNoiDung);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FrmMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "HỆ THỐNG QUẢN LÝ ĐĂNG KÝ HỌC PHẦN & ĐIỂM";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem heThongMenu;
        private ToolStripMenuItem thoatMenuItem;
        private ToolStripMenuItem quanLyMenu;
        private ToolStripMenuItem quanLySinhVienLopMenuItem;
        private ToolStripMenuItem dangKyHocPhanMenuItem;
        private ToolStripMenuItem nhapDiemMenuItem;
        private ToolStripMenuItem baoCaoMenu;
        private ToolStripMenuItem canhBaoHocVuMenuItem;
        private Panel panelNoiDung;
    }
}
