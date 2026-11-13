using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BCWF.Common;

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
            // Khôi phục vị trí/kích thước cửa sổ đã lưu trong Registry.
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
