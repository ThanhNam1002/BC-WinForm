using BCWF.Common;
using BCWF.Services;
using BCWF.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// Trang chủ hiển thị sau khi đăng nhập: thông tin tài khoản, học kỳ hiện tại,
    /// thống kê dữ liệu và lối tắt tới các chức năng chính.
    /// </summary>
    public class FrmTrangChu : Form
    {
        private readonly DataStore _store = DataStore.Instance;

        private Label lblChao = null!;
        private Label lblHocKy = null!;
        private Label lblThongKe = null!;
        private ListBox lstNhacViec = null!;
        private Label lblNhacViecTrong = null!;

        public FrmTrangChu()
        {
            Text = "Trang chủ";
            StartPosition = FormStartPosition.Manual;
            BackColor = SystemColors.Control;
            AutoScroll = true;
            Padding = new Padding(12);

            var root = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true
            };

            // ---- Tiêu đề ----
            var lblTieuDe = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ ĐĂNG KÝ HỌC PHẦN & ĐIỂM",
                Font = new Font("Segoe UI", 15F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 55, 120),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 4)
            };
            root.Controls.Add(lblTieuDe);

            lblChao = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F),
                Margin = new Padding(0, 0, 0, 10)
            };
            root.Controls.Add(lblChao);

            // ---- Học kỳ & cấu hình ----
            var grpHocKy = new GroupBox
            {
                Text = "Thông tin học kỳ & cấu hình",
                Width = 900,
                AutoSize = true,
                Padding = new Padding(10)
            };
            lblHocKy = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F)
            };
            grpHocKy.Controls.Add(lblHocKy);
            root.Controls.Add(grpHocKy);

            // ---- Thống kê ----
            var grpThongKe = new GroupBox
            {
                Text = "Thống kê dữ liệu",
                Width = 900,
                AutoSize = true,
                Padding = new Padding(10)
            };
            lblThongKe = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F)
            };
            grpThongKe.Controls.Add(lblThongKe);
            root.Controls.Add(grpThongKe);

            // ---- Nhắc việc ----
            var grpNhacViec = new GroupBox
            {
                Text = "Nhắc việc / Cảnh báo",
                Width = 900,
                Height = 180,
                Padding = new Padding(10)
            };
            lstNhacViec = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F)
            };
            lblNhacViecTrong = new Label
            {
                Text = "Không có nhắc việc.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Visible = false
            };
            grpNhacViec.Controls.Add(lstNhacViec);
            grpNhacViec.Controls.Add(lblNhacViecTrong);
            root.Controls.Add(grpNhacViec);

            // ---- Lối tắt ----
            var grpLoi = new GroupBox
            {
                Text = "Lối tắt chức năng",
                Width = 900,
                AutoSize = true,
                Padding = new Padding(10)
            };
            var flowLoi = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            ThemNutLopTat(flowLoi, "SV1 - Sinh viên & Lớp", () => MoForm<FrmSinhVien>());
            ThemNutLopTat(flowLoi, "SV2 - Đăng ký học phần", () => MoForm<FrmDangKy>());
            ThemNutLopTat(flowLoi, "SV3 - Nhập điểm & Bảng điểm", () => MoForm<FrmNhapDiem>());
            ThemNutLopTat(flowLoi, "Cảnh báo học vụ", () => MoForm<FrmCanhBaoHocVu>());
            ThemNutLopTat(flowLoi, "Đổi mật khẩu", MoDoiMatKhau);
            ThemNutLopTat(flowLoi, "Đóng tất cả cửa sổ", DongTatCa);
            grpLoi.Controls.Add(flowLoi);
            root.Controls.Add(grpLoi);

            Controls.Add(root);

            Load += (s, e) => CapNhat();
        }

        private void ThemNutLopTat(Control parent, string text, Action hanhDong)
        {
            var btn = new Button
            {
                Text = text,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 6),
                Margin = new Padding(4),
                FlatStyle = FlatStyle.Standard
            };
            btn.Click += (s, e) => hanhDong();
            parent.Controls.Add(btn);
        }

        private void MoForm<T>() where T : Form, new()
        {
            if (MdiParent is FrmMain main)
            {
                main.MoFormCon<T>();
            }
        }

        private void MoDoiMatKhau()
        {
            using (var frm = new FrmDoiMatKhau())
            {
                frm.ShowDialog(this);
            }
        }

        private void DongTatCa()
        {
            if (MdiParent is FrmMain main)
            {
                foreach (Form f in main.MdiChildren.ToList())
                {
                    if (f != this)
                    {
                        f.Close();
                    }
                }
            }
        }

        private void CapNhat()
        {
            var user = FrmDangNhap.NguoiDungHienTai;
            string tenHienThi = string.IsNullOrWhiteSpace(user?.HoTen) ? (user?.TenDangNhap ?? "Khách") : user.HoTen;
            string vaiTro = string.IsNullOrWhiteSpace(user?.VaiTro) ? "Người dùng" : user.VaiTro;
            lblChao.Text = $"Xin chào, {tenHienThi} ({user?.TenDangNhap})  •  Vai trò: {vaiTro}  •  {DateTime.Now:dddd, dd/MM/yyyy HH:mm}";

            lblHocKy.Text =
                $"Học kỳ hiện tại: {AppConfig.HocKyHienTai}\n" +
                $"Tín chỉ đăng ký: tối thiểu {AppConfig.MinTC} - tối đa {AppConfig.MaxTC}\n" +
                $"Ngưỡng cảnh báo học vụ (ĐTB tích lũy): {AppConfig.DiemCanhBao:0.0}\n" +
                $"Thư mục dữ liệu: {_store.DataFolder}";

            int soKhoa = _store.Khoas.Count;
            int soLop = _store.Lops.Count;
            int soSv = _store.SinhViens.Count;
            int soMon = _store.MonHocs.Count;
            int soDk = _store.DangKys.Count;

            var mssvDaDk = new HashSet<string>(_store.DangKys.Select(d => d.MSSV), StringComparer.OrdinalIgnoreCase);
            int svChuaDk = _store.SinhViens.Count(s => !mssvDaDk.Contains(s.MSSV));

            int svThieuAnh = _store.SinhViens.Count(s =>
                string.IsNullOrWhiteSpace(s.Anh) ||
                !System.IO.File.Exists(_store.LayDuongDanAnh(s.Anh)));

            // Cảnh báo học vụ: ĐTB tích lũy (trung bình các môn đã nhập đủ điểm) < ngưỡng
            int soSvCanhBao = 0;
            var nhacViec = new List<string>();
            var theoSV = _store.DangKys.GroupBy(d => d.MSSV, StringComparer.OrdinalIgnoreCase);
            foreach (var g in theoSV)
            {
                var daNhap = g.Where(d => d.DaNhapDiem).ToList();
                if (daNhap.Count == 0)
                {
                    continue;
                }
                double tb = daNhap.Average(d =>
                    d.DiemQT!.Value * 0.3 + d.DiemGK!.Value * 0.3 + d.DiemCK!.Value * 0.4);
                if (tb < AppConfig.DiemCanhBao)
                {
                    soSvCanhBao++;
                    var sv = _store.SinhViens.FirstOrDefault(s => string.Equals(s.MSSV, g.Key, StringComparison.OrdinalIgnoreCase));
                    nhacViec.Add($"• {g.Key} {(sv == null ? string.Empty : sv.HoTen)}: ĐTB tích lũy {tb:0.00} < {AppConfig.DiemCanhBao:0.0}");
                }
            }

            foreach (var sv in _store.SinhViens)
            {
                if (string.IsNullOrWhiteSpace(sv.Anh))
                {
                    nhacViec.Add($"• {sv.MSSV} {sv.HoTen}: chưa có ảnh thẻ");
                }
                else if (!System.IO.File.Exists(_store.LayDuongDanAnh(sv.Anh)))
                {
                    nhacViec.Add($"• {sv.MSSV} {sv.HoTen}: thiếu tệp ảnh '{sv.Anh}'");
                }
            }

            if (svChuaDk > 0)
            {
                nhacViec.Add($"• {svChuaDk} sinh viên chưa đăng ký môn học nào");
            }

            lblThongKe.Text =
                $"Khoa: {soKhoa}\n" +
                $"Lớp: {soLop}\n" +
                $"Sinh viên: {soSv}\n" +
                $"Môn học: {soMon}\n" +
                $"Lượt đăng ký: {soDk}\n" +
                $"Sinh viên cảnh báo học vụ: {soSvCanhBao}\n" +
                $"Sinh viên chưa đăng ký môn: {svChuaDk}\n" +
                $"Sinh viên thiếu ảnh thẻ: {svThieuAnh}";

            lstNhacViec.Items.Clear();
            if (nhacViec.Count == 0)
            {
                lstNhacViec.Visible = false;
                lblNhacViecTrong.Visible = true;
            }
            else
            {
                lstNhacViec.Visible = true;
                lblNhacViecTrong.Visible = false;
                foreach (var item in nhacViec.Take(200))
                {
                    lstNhacViec.Items.Add(item);
                }
                if (nhacViec.Count > 200)
                {
                    lstNhacViec.Items.Add($"... và {nhacViec.Count - 200} mục khác");
                }
            }
        }
    }
}
