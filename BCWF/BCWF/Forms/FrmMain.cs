using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BCWF.Common;
using BCWF.Services;

namespace BCWF
{
    public partial class FrmMain : Form
    {
        /// <summary>Các form đã tạo, giữ lại để chuyển qua lại mà không mất trạng thái.</summary>
        private readonly Dictionary<Type, Form> _cacFormDaMo = new Dictionary<Type, Form>();

        public FrmMain()
        {
            InitializeComponent();

            // Wire up the "Cửa sổ" (Window) menu items.
            xepChongToolStripMenuItem.Click += xepChongToolStripMenuItem_Click;
            xepNgangToolStripMenuItem.Click += xepNgangToolStripMenuItem_Click;
            xepDocStripMenuItem.Click += xepDocStripMenuItem_Click;
            dongTatCaToolStripMenuItem.Click += dongTatCaToolStripMenuItem_Click;

            // The MDI client area is used to host child windows; hide the
            // single-content panel so it does not cover them.
            panelNoiDung.Visible = false;

            // Mở trang chủ mặc định ngay sau khi đăng nhập.
            MoFormCon<Forms.FrmTrangChu>();

            // Mục "Trang chủ" trong menu Hệ thống để quay lại trang chủ.
            var trangChuMenuItem = new ToolStripMenuItem("Trang chủ")
            {
                ShortcutKeys = Keys.Control | Keys.Home
            };
            trangChuMenuItem.Click += (s, e) => MoFormCon<Forms.FrmTrangChu>();
            heThongMenu.DropDownItems.Insert(0, trangChuMenuItem);
            heThongMenu.DropDownItems.Insert(1, new ToolStripSeparator());

            // Phân quyền: từng vai trò nhìn thấy menu khác nhau.
            ChinhMenuTheoVaiTro();

            // Menu tùy chỉnh giao diện (ColorDialog / FontDialog, lưu lựa chọn vào Registry).
            var tuyChinhMenuItem = new ToolStripMenuItem("Tùy chỉnh giao diện");
            var doiMauMenuItem = new ToolStripMenuItem("Đổi màu nền...");
            doiMauMenuItem.Click += DoiMauNen_Click;
            var doiFontMenuItem = new ToolStripMenuItem("Đổi phông chữ...");
            doiFontMenuItem.Click += DoiPhongChu_Click;
            tuyChinhMenuItem.DropDownItems.Add(doiMauMenuItem);
            tuyChinhMenuItem.DropDownItems.Add(doiFontMenuItem);
            heThongMenu.DropDownItems.Insert(2, tuyChinhMenuItem);

            // Menu sao lưu / khôi phục dữ liệu (chỉ dành cho quản trị viên).
            if (LaQuanTri)
            {
                var saoLuuMenuItem = new ToolStripMenuItem("Sao lưu dữ liệu");
                saoLuuMenuItem.Click += SaoLuu_Click;
                var khoiPhucMenuItem = new ToolStripMenuItem("Khôi phục dữ liệu...");
                khoiPhucMenuItem.Click += KhoiPhuc_Click;

                var duLieuMenuItem = new ToolStripMenuItem("Dữ liệu");
                duLieuMenuItem.DropDownItems.Add(saoLuuMenuItem);
                duLieuMenuItem.DropDownItems.Add(khoiPhucMenuItem);
                heThongMenu.DropDownItems.Insert(3, duLieuMenuItem);
            }

            Load += FrmMain_Load;
            FormClosing += FrmMain_FormClosing;
        }

        /// <summary>
        /// Chỉ quản trị viên (Admin) thấy menu Báo cáo (cảnh báo học vụ) và các chức năng hệ thống;
        /// nhân viên/Giáo vụ chỉ thấy các chức năng nghiệp vụ.
        /// </summary>
        public static bool LaQuanTri => string.Equals(
            Forms.FrmDangNhap.NguoiDungHienTai?.VaiTro,
            "Admin",
            StringComparison.OrdinalIgnoreCase);

        private void ChinhMenuTheoVaiTro()
        {
            if (!LaQuanTri)
            {
                baoCaoMenu.Visible = false;
            }
        }

        #region Tùy chỉnh giao diện

        private void DoiMauNen_Click(object? sender, EventArgs e)
        {
            using ColorDialog dlg = new ColorDialog
            {
                FullOpen = true,
                Color = BackColor
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            BackColor = dlg.Color;
            TuyChonNguoiDung.GhiChuoi("MauNen", dlg.Color.ToArgb().ToString());
        }

        private void DoiPhongChu_Click(object? sender, EventArgs e)
        {
            using FontDialog dlg = new FontDialog
            {
                Font = menuStrip1.Font
            };

            if (dlg.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            menuStrip1.Font = dlg.Font;
            TuyChonNguoiDung.GhiChuoi("PhongChu",
                $"{dlg.Font.Name};{dlg.Font.Size:0.##};{(int)dlg.Font.Style}");
        }

        /// <summary>Khôi phục màu nền và phông chữ đã lưu trong Registry.</summary>
        private static void KhoiPhucGiaoDien(Form form, MenuStrip menu)
        {
            string? mau = TuyChonNguoiDung.DocChuoi("MauNen");
            if (int.TryParse(mau, out int argb))
            {
                form.BackColor = Color.FromArgb(argb);
            }

            string? phong = TuyChonNguoiDung.DocChuoi("PhongChu");
            if (!string.IsNullOrWhiteSpace(phong))
            {
                string[] phan = phong.Split(';');
                if (phan.Length >= 3
                    && float.TryParse(phan[1], System.Globalization.NumberStyles.Float,
                        System.Globalization.CultureInfo.InvariantCulture, out float coChu))
                {
                    try
                    {
                        menu.Font = new Font(phan[0], coChu, (FontStyle)int.Parse(phan[2]));
                    }
                    catch
                    {
                        // Font hệ thống không còn thì giữ font mặc định.
                    }
                }
            }
        }

        #endregion

        #region Sao lưu / khôi phục dữ liệu

        private void SaoLuu_Click(object? sender, EventArgs e)
        {
            DialogResult xacNhan = MessageBox.Show(
                "Sao lưu toàn bộ tệp dữ liệu của thư mục Data sang thư mục Backup?",
                "Xác nhận sao lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var saoLuu = new Services.BackupService();
                string thuMuc = saoLuu.SaoLuu();
                FileHelpers.GhiLog("Đã sao lưu dữ liệu vào " + thuMuc);
                MessageBox.Show($"Đã sao lưu dữ liệu vào:\n{thuMuc}", "Sao lưu thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                FileHelpers.GhiLog("Lỗi sao lưu dữ liệu: " + loi.Message);
                MessageBox.Show(loi.Message, "Lỗi sao lưu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void KhoiPhuc_Click(object? sender, EventArgs e)
        {
            using var chonThuMuc = new FolderBrowserDialog
            {
                Description = "Chọn thư mục sao lưu cần khôi phục (trong Data\\Backup).",
                ShowNewFolderButton = false
            };

            string thuMucBackup = Path.Combine(DataStore.Instance.DataFolder, "Backup");
            if (Directory.Exists(thuMucBackup))
            {
                chonThuMuc.SelectedPath = thuMucBackup;
            }

            if (chonThuMuc.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                "Khôi phục dữ liệu sẽ GHI ĐÈ toàn bộ tệp .txt hiện tại trong thư mục Data.\n"
                + "Bạn có chắc muốn khôi phục từ:\n" + chonThuMuc.SelectedPath + "?",
                "Xác nhận khôi phục", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);
            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            try
            {
                var saoLuu = new Services.BackupService();
                saoLuu.KhoiPhuc(chonThuMuc.SelectedPath);
                DataStore.Instance.LoadAll();

                // Đóng mọi cửa sổ con và mở lại trang chủ để dữ liệu hiển thị đồng bộ.
                foreach (Form form in MdiChildren.ToList())
                {
                    form.Close();
                }
                MoFormCon<Forms.FrmTrangChu>();

                FileHelpers.GhiLog("Đã khôi phục dữ liệu từ " + chonThuMuc.SelectedPath);
                MessageBox.Show("Đã khôi phục dữ liệu thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                FileHelpers.GhiLog("Lỗi khôi phục dữ liệu: " + loi.Message);
                MessageBox.Show(loi.Message, "Lỗi khôi phục",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        /// <summary>
        /// Mở form chức năng như một cửa sổ MDI con. Mỗi form chỉ tạo một lần;
        /// khi chọn lại thì kích hoạt cửa sổ đã mở thay vì mở cửa sổ mới.
        /// </summary>
        public void MoFormCon<T>() where T : Form, new()
        {
            if (_cacFormDaMo.TryGetValue(typeof(T), out Form? formDangMo) && formDangMo != null && !formDangMo.IsDisposed)
            {
                formDangMo.Activate();
                if (formDangMo.WindowState == FormWindowState.Minimized)
                {
                    formDangMo.WindowState = FormWindowState.Normal;
                }
                return;
            }

            var formCon = new T
            {
                MdiParent = this
            };

            _cacFormDaMo[typeof(T)] = formCon;
            formCon.FormClosed += (s, e) => _cacFormDaMo.Remove(typeof(T));
            formCon.Show();
        }

        #region Hệ thống

        private void thoatMenuItem_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void FrmMain_Load(object? sender, EventArgs e)
        {
            // Khôi phục màu nền/phông chữ và vị trí/kích thước đã lưu trong Registry.
            KhoiPhucGiaoDien(this, menuStrip1);

            string? daLuu = TuyChonNguoiDung.DocChuoi("ViTriForm");
            if (!string.IsNullOrWhiteSpace(daLuu))
            {
                string[] phan = daLuu.Split(';');
                if (phan.Length >= 4
                    && int.TryParse(phan[0], out int x)
                    && int.TryParse(phan[1], out int y)
                    && int.TryParse(phan[2], out int rong)
                    && int.TryParse(phan[3], out int cao)
                    && rong > 200 && cao > 200)
                {
                    WindowState = FormWindowState.Normal;
                    StartPosition = FormStartPosition.Manual;
                    Location = new Point(x, y);
                    Size = new Size(rong, cao);
                }
            }
        }

        private void FrmMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Lưu vị trí/kích thước cửa sổ vào Registry.
            Rectangle phanVung = RestoreBounds;
            TuyChonNguoiDung.GhiChuoi("ViTriForm",
                $"{phanVung.Left};{phanVung.Top};{phanVung.Width};{phanVung.Height}");
        }

        #endregion

        #region Quản lý

        private void quanLySinhVienLopMenuItem_Click(object? sender, EventArgs e)
        {
            MoFormCon<Forms.FrmSinhVien>();
        }

        private void dangKyHocPhanMenuItem_Click(object? sender, EventArgs e)
        {
            MoFormCon<Forms.FrmDangKy>();
        }

        private void nhapDiemMenuItem_Click(object? sender, EventArgs e)
        {
            MoFormCon<Forms.FrmNhapDiem>();
        }

        #endregion

        #region Báo cáo

        private void canhBaoHocVuMenuItem_Click(object? sender, EventArgs e)
        {
            MoFormCon<Forms.FrmCanhBaoHocVu>();
        }

        #endregion

        #region Cửa sổ

        private void xepChongToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void xepNgangToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void xepDocStripMenuItem_Click(object? sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void dongTatCaToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            foreach (Form form in MdiChildren.ToList())
            {
                form.Close();
            }
        }

        #endregion
    }
}
