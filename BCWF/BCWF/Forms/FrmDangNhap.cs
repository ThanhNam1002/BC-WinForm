using BCWF.Models;
using BCWF.Services;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    public partial class FrmDangNhap : Form
    {
        public FrmDangNhap()
        {
            InitializeComponent();
            ActiveControl = txtUsername;
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtUsername.Text.Trim();
            string matKhau = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            User? nguoiDung = DataStore.Instance.Users.FirstOrDefault(u =>
                string.Equals(u.TenDangNhap, tenDangNhap, StringComparison.OrdinalIgnoreCase)
                && u.MatKhau == matKhau);

            if (nguoiDung == null)
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}