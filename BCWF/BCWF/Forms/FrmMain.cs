using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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
        }

        /// <summary>
        /// Mở form chức năng như một cửa sổ MDI con. Mỗi form chỉ tạo một lần;
        /// khi chọn lại thì kích hoạt cửa sổ đã mở thay vì mở cửa sổ mới.
        /// </summary>
        private void MoFormCon<T>() where T : Form, new()
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
