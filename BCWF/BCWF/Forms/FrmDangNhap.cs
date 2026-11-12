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
    public partial class FrmDangNhap : Form
    {
        private readonly DataStore _store = DataStore.Instance;

        public FrmDangNhap()
        {
            InitializeComponent();
            lnkDoiMatKhau.Click += lnkDoiMatKhau_Click;
            btnHuy.Click += btnHuy_Click;
            MakePictureBoxCircular(pictureBox1);
            pictureBox1.SizeChanged += (s, e) => MakePictureBoxCircular(pictureBox1);
            try
            {
                // Ảnh được copy ra thư mục Resources cạnh tệp chạy (xem BCWF.csproj).
                string imgPath = System.IO.Path.Combine(AppContext.BaseDirectory, "Resources", "anh.png");
                if (System.IO.File.Exists(imgPath))
                {
                    using (var stream = new System.IO.FileStream(imgPath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải hình ảnh: " + ex.Message, "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MakePictureBoxCircular(PictureBox pic)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            int size = Math.Min(pic.Width, pic.Height);
            int x = (pic.Width - size) / 2;
            int y = (pic.Height - size) / 2;
            path.AddEllipse(x, y, size, size);
            pic.Region = new Region(path);
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.","Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //Kiểm tra đăng nhâp theo tài khoản trong users.txt
            Models.User? taiKhoan = _store.Users.FirstOrDefault(u =>
                string.Equals(u.TenDangNhap, username, StringComparison.OrdinalIgnoreCase));

            if (taiKhoan != null && string.Equals(taiKhoan.MatKhau, password, StringComparison.Ordinal))
            {
                MessageBox.Show("Đăng nhập thành công", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu.","Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lnkDoiMatKhau_Click(object? sender, EventArgs e)
        {
            using (FrmDoiMatKhau frm = new FrmDoiMatKhau())
            {
                frm.ShowDialog(this);
            }
            txtPassword.Focus();
        }

        private void btnHuy_Click(object? sender, EventArgs e)
        {
            this.Close();
        }
    }
}