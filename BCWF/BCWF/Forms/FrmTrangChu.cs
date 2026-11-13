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
        private TableLayoutPanel tlpThongKe = null!;
        private ListBox lstNhacViec = null!;
        private Label lblNhacViecTrong = null!;

        public FrmTrangChu()
        {
            Text = "Trang chủ";
            WindowState = FormWindowState.Maximized;
            BackColor = Color.White;
            AutoScroll = true;
            Padding = new Padding(16);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                Padding = new Padding(0)
            };
            root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // ---- Header ----
            var pnlHeader = new Panel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(0, 0, 0, 8) };
            var lblTieuDe = new Label
            {
                Text = "HỆ THỐNG QUẢN LÝ ĐĂNG KÝ HỌC PHẦN & ĐIỂM",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 55, 120),
                AutoSize = true,
                Dock = DockStyle.Top
            };
            lblChao = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.DimGray,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 4, 0, 0)
            };
            pnlHeader.Controls.Add(lblChao);
            pnlHeader.Controls.Add(lblTieuDe);
            root.Controls.Add(pnlHeader, 0, 0);

            // ---- Học kỳ & cấu hình ----
            var grpHocKy = new GroupBox
            {
                Text = "Học kỳ & cấu hình",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(12, 8, 12, 8),
                Margin = new Padding(0, 0, 0, 8)
            };
            lblHocKy = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F)
            };
            grpHocKy.Controls.Add(lblHocKy);
            root.Controls.Add(grpHocKy, 0, 1);

            // ---- Thống kê (lưới 2 cột) ----
            var grpThongKe = new GroupBox
            {
                Text = "Thống kê dữ liệu",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(12, 8, 12, 8),
                Margin = new Padding(0, 0, 0, 8)
            };
            tlpThongKe = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 2
            };
            for (int i = 0; i < 2; i++)
            {
                tlpThongKe.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            for (int i = 0; i < 4; i++)
            {
                tlpThongKe.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }
            grpThongKe.Controls.Add(tlpThongKe);
            root.Controls.Add(grpThongKe, 0, 2);

            // ---- Nhắc việc ----
            var grpNhacViec = new GroupBox
            {
                Text = "Nhắc việc / Cảnh báo",
                Dock = DockStyle.Top,
                Height = 170,
                Padding = new Padding(12, 8, 12, 8),
                Margin = new Padding(0, 0, 0, 8)
            };
            lstNhacViec = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                IntegralHeight = false
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
            root.Controls.Add(grpNhacViec, 0, 3);

            // ---- Lối tắt ----
            var grpLoi = new GroupBox
            {
                Text = "Lối tắt chức năng",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(12, 8, 12, 8),
                Margin = new Padding(0, 0, 0, 8)
            };
            var flowLoi = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true
            };
            ThemNut(flowLoi, "SV1 - Sinh viên & Lớp", () => MoForm<FrmSinhVien>());
            ThemNut(flowLoi, "SV2 - Đăng ký học phần", () => MoForm<FrmDangKy>());
            ThemNut(flowLoi, "SV3 - Nhập điểm & Bảng điểm", () => MoForm<FrmNhapDiem>());
            if (FrmMain.LaQuanTri)
            {
                ThemNut(flowLoi, "Cảnh báo học vụ", () => MoForm<FrmCanhBaoHocVu>());
            }
            ThemNut(flowLoi, "Đổi mật khẩu", MoDoiMatKhau);
            ThemNut(flowLoi, "Đóng tất cả cửa sổ", DongTatCa);
            grpLoi.Controls.Add(flowLoi);
            root.Controls.Add(grpLoi, 0, 4);

            Controls.Add(root);

            Load += (s, e) => CapNhat();
        }

        private void ThemNut(Control parent, string text, Action hanhDong)
        {
            var btn = new Button
            {
                Text = text,
                AutoSize = true,
                Padding = new Padding(12, 6, 12, 6),
                Margin = new Padding(4),
                FlatStyle = FlatStyle.System
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
                $"Học kỳ: {AppConfig.HocKyHienTai}     |     Tín chỉ: {AppConfig.MinTC} – {AppConfig.MaxTC}     |     Ngưỡng cảnh báo: {AppConfig.DiemCanhBao:0.0}     |     Dữ liệu: {_store.DataFolder}";

            int soKhoa = _store.Khoas.Count;
            int soLop = _store.Lops.Count;
            int soSv = _store.SinhViens.Count;
            int soMon = _store.MonHocs.Count;
            int soDk = _store.DangKys.Count;

            var mssvDaDk = new HashSet<string>(_store.DangKys.Select(d => d.MSSV), StringComparer.OrdinalIgnoreCase);
            int svChuaDk = _store.SinhViens.Count(s => !mssvDaDk.Contains(s.MSSV));

            // Chỉ cảnh báo khi sinh viên có khai báo ảnh nhưng tệp ảnh không tồn tại;
            // sinh viên chưa có ảnh thẻ là trạng thái bình thường, không phải lỗi dữ liệu.
            int svThieuAnh = _store.SinhViens.Count(s =>
                !string.IsNullOrWhiteSpace(s.Anh) &&
                !System.IO.File.Exists(_store.LayDuongDanAnh(s.Anh)));

            int soSvCanhBao = 0;
            var nhacViec = new List<string>();
            foreach (var g in _store.DangKys.GroupBy(d => d.MSSV, StringComparer.OrdinalIgnoreCase))
            {
                var daNhap = g.Where(d => d.DaNhapDiem).ToList();
                if (daNhap.Count == 0) continue;
                double tb = daNhap.Average(d =>
                    d.DiemQT!.Value * 0.3 + d.DiemGK!.Value * 0.3 + d.DiemCK!.Value * 0.4);
                if (tb < AppConfig.DiemCanhBao)
                {
                    soSvCanhBao++;
                    var sv = _store.SinhViens.FirstOrDefault(s => string.Equals(s.MSSV, g.Key, StringComparison.OrdinalIgnoreCase));
                    nhacViec.Add($"⚠ {g.Key} {(sv == null ? "" : sv.HoTen)}: ĐTB {tb:0.00} < {AppConfig.DiemCanhBao:0.0}");
                }
            }
            foreach (var sv in _store.SinhViens.Where(s => !string.IsNullOrWhiteSpace(s.Anh) &&
                !System.IO.File.Exists(_store.LayDuongDanAnh(s.Anh))).Take(5))
            {
                nhacViec.Add($"🖼 {sv.MSSV} {sv.HoTen}: thiếu tệp ảnh thẻ");
            }
            if (svChuaDk > 0)
            {
                nhacViec.Add($"📋 {svChuaDk} sinh viên chưa đăng ký môn học nào");
            }

            // Lưới thống kê
            tlpThongKe.Controls.Clear();
            ThemOThongKe("Khoa", soKhoa, 0, 0);
            ThemOThongKe("Lớp", soLop, 1, 0);
            ThemOThongKe("Sinh viên", soSv, 2, 0);
            ThemOThongKe("Môn học", soMon, 3, 0);
            ThemOThongKe("Lượt đăng ký", soDk, 0, 1);
            ThemOThongKe("SV cảnh báo", soSvCanhBao, 1, 1);
            ThemOThongKe("SV chưa ĐK", svChuaDk, 2, 1);
            ThemOThongKe("SV thiếu ảnh", svThieuAnh, 3, 1);

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
                foreach (var item in nhacViec.Take(8))
                {
                    lstNhacViec.Items.Add(item);
                }
                if (nhacViec.Count > 8)
                {
                    lstNhacViec.Items.Add($"... và {nhacViec.Count - 8} mục khác");
                }
            }
        }

        private void ThemOThongKe(string nhan, int giaTri, int col, int row)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(8),
                Margin = new Padding(4),
                BackColor = Color.FromArgb(245, 248, 255),
                Height = 60
            };
            var lblGiaTri = new Label
            {
                Text = giaTri.ToString(),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 55, 120),
                AutoSize = true,
                Location = new Point(8, 6)
            };
            var lblNhan = new Label
            {
                Text = nhan,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.DimGray,
                AutoSize = true,
                Location = new Point(8, 34)
            };
            pnl.Controls.Add(lblGiaTri);
            pnl.Controls.Add(lblNhan);
            tlpThongKe.Controls.Add(pnl, col, row);
        }
    }
}
