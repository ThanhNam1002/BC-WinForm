using BCWF.Services;
using BCWF.Common;
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

        /// <summary>Tài khoản đang đăng nhập (để trang chủ/trang cá nhân hiển thị thông tin).</summary>
        public static Models.User? NguoiDungHienTai { get; set; }

        /// <summary>Số lần nhập sai tối đa cho phép trước khi khóa tài khoản.</summary>
        private const int GioiHanSai = 3;

        /// <summary>Số lần sai liên tiếp theo từng tài khoản (chỉ lưu trong phiên làm việc).</summary>
        private static readonly Dictionary<string, int> _soLanSai = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Tài khoản đã nhập sai quá giới hạn, bị khóa đến khi khởi động lại chương trình.</summary>
        private static readonly HashSet<string> _taiKhoanBiKhoa = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public FrmDangNhap()
        {
            InitializeComponent();
            lnkDoiMatKhau.Click += lnkDoiMatKhau_Click;
            btnHuy.Click += btnHuy_Click;
            MakePictureBoxCircular(pictureBox1);
            pictureBox1.SizeChanged += (s, e) => MakePictureBoxCircular(pictureBox1);

            // Ghi nhớ đăng nhập: đọc tên đăng nhập đã lưu trong Registry.
            chkGhiNho.Checked = TuyChonNguoiDung.DocBool("GhiNhoDangNhap");
            if (chkGhiNho.Checked)
            {
                txtUsername.Text = TuyChonNguoiDung.DocChuoi("TenDangNhapLuu") ?? string.Empty;
                txtPassword.Focus();
            }
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
                FileHelpers.GhiLog("Lỗi tải ảnh đăng nhập: " + ex.Message);
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

            // Tài khoản đã bị khóa sau 3 lần sai thì không cho đăng nhập nữa.
            if (_taiKhoanBiKhoa.Contains(username))
            {
                MessageBox.Show($"Tài khoản '{username}' đã bị khóa do nhập sai {GioiHanSai} lần liên tiếp.\n"
                    + "Vui lòng liên hệ quản trị viên.", "Tài khoản bị khóa",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Kiểm tra đăng nhâp theo tài khoản trong users.txt
            Models.User? taiKhoan = _store.Users.FirstOrDefault(u =>
                string.Equals(u.TenDangNhap, username, StringComparison.OrdinalIgnoreCase));

            if (taiKhoan != null && string.Equals(taiKhoan.MatKhau, password, StringComparison.Ordinal))
            {
                NguoiDungHienTai = taiKhoan;
                _soLanSai.Remove(username);

                // Lưu/ xóa ghi nhớ đăng nhập trong Registry theo tùy chọn của người dùng.
                TuyChonNguoiDung.GhiBool("GhiNhoDangNhap", chkGhiNho.Checked);
                if (chkGhiNho.Checked)
                {
                    TuyChonNguoiDung.GhiChuoi("TenDangNhapLuu", username);
                }
                else
                {
                    TuyChonNguoiDung.Xoa("TenDangNhapLuu");
                }

                MessageBox.Show("Đăng nhập thành công", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Đếm số lần sai liên tiếp; đủ 3 lần thì khóa tài khoản.
                _soLanSai.TryGetValue(username, out int soLanSai);
                soLanSai++;
                _soLanSai[username] = soLanSai;

                if (soLanSai >= GioiHanSai)
                {
                    _taiKhoanBiKhoa.Add(username);
                    MessageBox.Show($"Bạn đã nhập sai {GioiHanSai} lần liên tiếp.\n"
                        + $"Tài khoản '{username}' bị khóa. Vui lòng liên hệ quản trị viên.",
                        "Tài khoản bị khóa", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Sai tên đăng nhập hoặc mật khẩu.\n"
                        + $"Bạn còn {GioiHanSai - soLanSai} lần thử trước khi tài khoản bị khóa.",
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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