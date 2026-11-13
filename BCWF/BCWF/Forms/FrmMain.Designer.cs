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
            cửaSổToolStripMenuItem = new ToolStripMenuItem();
            xepChongToolStripMenuItem = new ToolStripMenuItem();
            xepNgangToolStripMenuItem = new ToolStripMenuItem();
            xepDocStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            dongTatCaToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { heThongMenu, quanLyMenu, baoCaoMenu, cửaSổToolStripMenuItem });
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
            thoatMenuItem.Size = new Size(224, 26);
            thoatMenuItem.Text = "Thoát";
            thoatMenuItem.Click += thoatMenuItem_Click;
            // 
            // quanLyMenu
            // 
            quanLyMenu.DropDownItems.AddRange(new ToolStripItem[] { quanLySinhVienLopMenuItem, dangKyHocPhanMenuItem, nhapDiemMenuItem });
            quanLyMenu.Name = "quanLyMenu";
            quanLyMenu.Size = new Size(146, 24);
            quanLyMenu.Text = "Quản Lý Danh Mục";
            // 
            // quanLySinhVienLopMenuItem
            // 
            quanLySinhVienLopMenuItem.Name = "quanLySinhVienLopMenuItem";
            quanLySinhVienLopMenuItem.Size = new Size(297, 26);
            quanLySinhVienLopMenuItem.Text = "SV1 - Sinh viên && Lớp";
            quanLySinhVienLopMenuItem.Click += quanLySinhVienLopMenuItem_Click;
            // 
            // dangKyHocPhanMenuItem
            // 
            dangKyHocPhanMenuItem.Name = "dangKyHocPhanMenuItem";
            dangKyHocPhanMenuItem.Size = new Size(297, 26);
            dangKyHocPhanMenuItem.Text = "SV2 - Đăng ký học phần";
            dangKyHocPhanMenuItem.Click += dangKyHocPhanMenuItem_Click;
            // 
            // nhapDiemMenuItem
            // 
            nhapDiemMenuItem.Name = "nhapDiemMenuItem";
            nhapDiemMenuItem.Size = new Size(297, 26);
            nhapDiemMenuItem.Text = "SV3 - Nhập điểm && Bảng điểm";
            nhapDiemMenuItem.Click += nhapDiemMenuItem_Click;
            // 
            // baoCaoMenu
            // 
            baoCaoMenu.DropDownItems.AddRange(new ToolStripItem[] { canhBaoHocVuMenuItem });
            baoCaoMenu.Name = "baoCaoMenu";
            baoCaoMenu.Size = new Size(79, 24);
            baoCaoMenu.Text = "Báo Cáo";
            // 
            // canhBaoHocVuMenuItem
            // 
            canhBaoHocVuMenuItem.Name = "canhBaoHocVuMenuItem";
            canhBaoHocVuMenuItem.Size = new Size(272, 26);
            canhBaoHocVuMenuItem.Text = "Danh sách cảnh báo học vụ";
            canhBaoHocVuMenuItem.Click += canhBaoHocVuMenuItem_Click;
            // 
            // cửaSổToolStripMenuItem
            // 
            cửaSổToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { xepChongToolStripMenuItem, xepNgangToolStripMenuItem, xepDocStripMenuItem, toolStripSeparator1, dongTatCaToolStripMenuItem });
            cửaSổToolStripMenuItem.Name = "cửaSổToolStripMenuItem";
            cửaSổToolStripMenuItem.Size = new Size(70, 24);
            cửaSổToolStripMenuItem.Text = "Cửa Sổ";
            // 
            // xepChongToolStripMenuItem
            // 
            xepChongToolStripMenuItem.Name = "xepChongToolStripMenuItem";
            xepChongToolStripMenuItem.Size = new Size(175, 26);
            xepChongToolStripMenuItem.Text = "Xếp Chồng";
            // 
            // xepNgangToolStripMenuItem
            // 
            xepNgangToolStripMenuItem.Name = "xepNgangToolStripMenuItem";
            xepNgangToolStripMenuItem.Size = new Size(175, 26);
            xepNgangToolStripMenuItem.Text = "Xếp Ngang";
            // 
            // xepDocStripMenuItem
            // 
            xepDocStripMenuItem.Name = "xepDocStripMenuItem";
            xepDocStripMenuItem.Size = new Size(175, 26);
            xepDocStripMenuItem.Text = "Xếp Dọc";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(172, 6);
            // 
            // dongTatCaToolStripMenuItem
            // 
            dongTatCaToolStripMenuItem.Name = "dongTatCaToolStripMenuItem";
            dongTatCaToolStripMenuItem.Size = new Size(175, 26);
            dongTatCaToolStripMenuItem.Text = "Đóng Tất Cả";
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 650);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
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
        private ToolStripMenuItem cửaSổToolStripMenuItem;
        private ToolStripMenuItem xepChongToolStripMenuItem;
        private ToolStripMenuItem xepNgangToolStripMenuItem;
        private ToolStripMenuItem xepDocStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem dongTatCaToolStripMenuItem;
    }
}
