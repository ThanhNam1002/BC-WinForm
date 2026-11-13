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

        /// <summary>Màu nền người dùng đã chọn; null nghĩa là dùng giao diện mặc định.</summary>
        private Color? _mauNen;

        public FrmMain()
        {
            InitializeComponent();

            // Wire up the "Cửa sổ" (Window) menu items.
            xepChongToolStripMenuItem.Click += xepChongToolStripMenuItem_Click;
            xepNgangToolStripMenuItem.Click += xepNgangToolStripMenuItem_Click;
            xepDocStripMenuItem.Click += xepDocStripMenuItem_Click;
            dongTatCaToolStripMenuItem.Click += dongTatCaToolStripMenuItem_Click;

            // Mở trang chủ mặc định ngay sau khi đăng nhập.
            MoFormCon<Forms.FrmTrangChu>();

            // Dựng lại menu Hệ thống theo thứ tự rõ ràng, không hiển thị phím tắt
            // và luôn có lối vào Đổi mật khẩu sau khi đăng nhập.
            var trangChuMenuItem = new ToolStripMenuItem("Trang chủ");
            trangChuMenuItem.Click += (s, e) => MoFormCon<Forms.FrmTrangChu>();

            var doiMatKhauMenuItem = new ToolStripMenuItem("Đổi mật khẩu...");
            doiMatKhauMenuItem.Click += DoiMatKhau_Click;

            // Menu tùy chỉnh giao diện (ColorDialog, lưu lựa chọn màu vào Registry).
            var tuyChinhMenuItem = new ToolStripMenuItem("Tùy chỉnh giao diện");
            var doiMauMenuItem = new ToolStripMenuItem("Đổi màu nền...");
            doiMauMenuItem.Click += DoiMauNen_Click;
            tuyChinhMenuItem.DropDownItems.Add(doiMauMenuItem);

            heThongMenu.DropDownItems.Clear();
            heThongMenu.DropDownItems.Add(trangChuMenuItem);
            heThongMenu.DropDownItems.Add(new ToolStripSeparator());
            heThongMenu.DropDownItems.Add(doiMatKhauMenuItem);
            heThongMenu.DropDownItems.Add(new ToolStripSeparator());
            heThongMenu.DropDownItems.Add(tuyChinhMenuItem);

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
                heThongMenu.DropDownItems.Add(new ToolStripSeparator());
                heThongMenu.DropDownItems.Add(duLieuMenuItem);
            }

            heThongMenu.DropDownItems.Add(new ToolStripSeparator());
            heThongMenu.DropDownItems.Add(thoatMenuItem);

            // Phân quyền: từng vai trò nhìn thấy menu khác nhau.
            ChinhMenuTheoVaiTro();

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

            _mauNen = dlg.Color;
            TuyChonNguoiDung.GhiChuoi("MauNen", dlg.Color.ToArgb().ToString());
            ApDungMauNen();
        }



        /// <summary>Khôi phục màu nền đã lưu trong Registry.</summary>
        private void KhoiPhucGiaoDien()
        {
            _mauNen = null;

            string? mau = TuyChonNguoiDung.DocChuoi("MauNen");
            if (int.TryParse(mau, out int argb))
            {
                _mauNen = Color.FromArgb(argb);
            }

            ApDungMauNen();
        }

        private void ApDungMauNen()
        {
            if (!_mauNen.HasValue)
            {
                return;
            }

            BackColor = _mauNen.Value;
            foreach (Control control in Controls)
            {
                if (control is MdiClient)
                {
                    control.BackColor = _mauNen.Value;
                }
            }

            foreach (Form form in MdiChildren)
            {
                form.BackColor = _mauNen.Value;
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

            // Áp dụng màu nền hiện tại cho form mở sau khi đã tùy chỉnh.
            if (_mauNen.HasValue)
            {
                formCon.BackColor = _mauNen.Value;
            }

            _cacFormDaMo[typeof(T)] = formCon;
            formCon.FormClosed += (s, e) => _cacFormDaMo.Remove(typeof(T));
            formCon.Show();
        }

        #region Hệ thống

        private void thoatMenuItem_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void DoiMatKhau_Click(object? sender, EventArgs e)
        {
            using var frm = new Forms.FrmDoiMatKhau();
            frm.ShowDialog(this);
        }

        private void FrmMain_Load(object? sender, EventArgs e)
        {
            // Khôi phục màu nền và vị trí/kích thước đã lưu trong Registry.
            KhoiPhucGiaoDien();

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
            // Hỏi xác nhận khi người dùng tự đóng bằng nút X hoặc menu Thoát.
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult xacNhan = MessageBox.Show(
                    "Bạn có chắc muốn thoát chương trình?",
                    "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (xacNhan != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

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
