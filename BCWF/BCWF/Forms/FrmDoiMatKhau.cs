using System;
using System.Windows.Forms;
using BCWF.Forms;

namespace BCWF.Forms
{
    public partial class FrmDoiMatKhau : Form
    {
        public FrmDoiMatKhau()
        {
            InitializeComponent();
        }

        //Đổi mật khẩu
        private void btnDoi_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string oldPassword = txtOldPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirm = txtConfirm.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(oldPassword) ||
                string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirm))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username != "admin")
            {
                MessageBox.Show("Tên đăng nhập không tồn tại.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (oldPassword != FrmDangNhap.MatKhauHienTai)
            {
                MessageBox.Show("Mật khẩu cũ không đúng.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirm)
            {
                MessageBox.Show("Xác nhận mật khẩu mới không khớp.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword == oldPassword)
            {
                MessageBox.Show("Mật khẩu mới phải khác mật khẩu cũ.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FrmDangNhap.MatKhauHienTai = newPassword;
            MessageBox.Show("Đổi mật khẩu thành công. Vui lòng đăng nhập bằng mật khẩu mới.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        //Hủy đổi mật khẩu
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}