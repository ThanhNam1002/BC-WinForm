using BCWF.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BCWF.Forms
{
    public partial class FrmDoiMatKhau : Form
    {
        private readonly DataStore _store = DataStore.Instance;

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

            Models.User? taiKhoan = _store.Users.FirstOrDefault(u =>
                string.Equals(u.TenDangNhap, username, StringComparison.OrdinalIgnoreCase));

            if (taiKhoan == null)
            {
                MessageBox.Show("Tên đăng nhập không tồn tại.", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.Equals(taiKhoan.MatKhau, oldPassword, StringComparison.Ordinal))
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

            try
            {
                // Ghi mật khẩu mới xuống users.txt để lần mở sau vẫn dùng được.
                taiKhoan.MatKhau = newPassword;
                _store.LuuUsers();
            }
            catch (Exception ex)
            {
                FileHelpers.GhiLog("Lỗi ghi users.txt khi đổi mật khẩu: " + ex.Message);
                MessageBox.Show("Không ghi được tệp users.txt: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
