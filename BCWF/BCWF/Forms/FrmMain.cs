using System;
using System.Collections.Generic;
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
        }

        /// <summary>
        /// Mở form chức năng trong vùng nội dung. Mỗi form chỉ tạo một lần;
        /// khi chọn lại thì hiển thị lại form cũ thay vì mở cửa sổ mới.
        /// </summary>
        private void MoFormCon<T>() where T : Form, new()
        {
            if (_cacFormDaMo.TryGetValue(typeof(T), out Form? formDangMo) && formDangMo != null && !formDangMo.IsDisposed)
            {
                HienThiForm(formDangMo);
                return;
            }

            var formCon = new T
            {
                TopLevel = false,
                FormBorderStyle = FormBorderStyle.None,
                Dock = DockStyle.Fill
            };

            _cacFormDaMo[typeof(T)] = formCon;
            panelNoiDung.Controls.Add(formCon);
            HienThiForm(formCon);
        }

        /// <summary>Ẩn form đang hiện và hiển thị form được chọn.</summary>
        private void HienThiForm(Form formCanHienThi)
        {
            foreach (Form form in _cacFormDaMo.Values)
            {
                if (form != formCanHienThi && form.Visible)
                {
                    form.Hide();
                }
            }

            formCanHienThi.Show();
            formCanHienThi.BringToFront();
        }

        #region Hệ thống

        private void thoatMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Quản lý

        private void quanLySinhVienLopMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmSinhVien>();
        }

        private void dangKyHocPhanMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmDangKy>();
        }

        private void nhapDiemMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmNhapDiem>();
        }

        #endregion

        #region Báo cáo

        private void canhBaoHocVuMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmCanhBaoHocVu>();
        }

        #endregion
    }
}
