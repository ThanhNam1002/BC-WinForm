using BCWF.Common;
using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// Trang chủ dạng dashboard: thông tin tài khoản, học kỳ hiện tại,
    /// các số liệu nhanh và danh sách việc cần xử lý.
    /// </summary>
    public class FrmTrangChu : Form
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        private Label lblChao = null!;
        private Label lblHocKy = null!;
        private Label lblTongQuan = null!;
        private TableLayoutPanel tlpThongKe = null!;
        private ListBox lstNhacViec = null!;
        private Label lblNhacViecTrong = null!;

        private static readonly Color MauChinh = Color.FromArgb(31, 58, 120);
        private static readonly Color MauNen = Color.FromArgb(244, 246, 250);
        private static readonly Color MauChuPhu = Color.FromArgb(100, 110, 125);

        public FrmTrangChu()
        {
            Text = "Trang chủ";
            WindowState = FormWindowState.Maximized;
            BackColor = MauNen;
            AutoScroll = true;
            Padding = new Padding(20);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = Color.Transparent,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 146F));
            for (int i = 1; i < 4; i++)
            {
                root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            // ---- Đầu trang ----
            var pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 128,
                BackColor = MauChinh,
                Margin = new Padding(0, 0, 0, 18)
            };

            var lblTieuDe = new Label
            {
                Text = "TỔNG QUAN HỆ THỐNG",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(24, 16)
            };
            lblChao = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(214, 226, 255),
                Location = new Point(24, 58)
            };
            lblHocKy = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.White,
                Location = new Point(24, 86)
            };
            pnlHeader.Controls.Add(lblTieuDe);
            pnlHeader.Controls.Add(lblChao);
            pnlHeader.Controls.Add(lblHocKy);
            root.Controls.Add(pnlHeader, 0, 0);

            // ---- Tiêu đề khu thống kê ----
            lblTongQuan = new Label
            {
                Text = "Thông tin nhanh",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = MauChinh,
                BackColor = Color.Transparent,
                AutoSize = true,
                Margin = new Padding(4, 0, 0, 8)
            };
            root.Controls.Add(lblTongQuan, 0, 1);

            // ---- Lưới thẻ thống kê ----
            tlpThongKe = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 4,
                RowCount = 2,
                BackColor = Color.Transparent,
                Margin = new Padding(0)
            };
            for (int i = 0; i < 4; i++)
            {
                tlpThongKe.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            }
            for (int i = 0; i < 2; i++)
            {
                tlpThongKe.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            root.Controls.Add(tlpThongKe, 0, 2);

            // ---- Việc cần xử lý ----
            var pnlNhacViec = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.White,
                Padding = new Padding(16),
                Margin = new Padding(0, 18, 0, 0)
            };
            pnlNhacViec.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlNhacViec.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            pnlNhacViec.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));

            var lblNhacViec = new Label
            {
                Text = "Cần xử lý",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = MauChinh,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 8)
            };
            var pnlDanhSach = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
            lstNhacViec = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F),
                IntegralHeight = false,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                SelectionMode = SelectionMode.None,
                TabStop = false
            };
            lblNhacViecTrong = new Label
            {
                Text = "Không có việc cần xử lý.",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                ForeColor = Color.Gray,
                Visible = false
            };
            pnlDanhSach.Controls.Add(lstNhacViec);
            pnlDanhSach.Controls.Add(lblNhacViecTrong);
            pnlNhacViec.Controls.Add(lblNhacViec, 0, 0);
            pnlNhacViec.Controls.Add(pnlDanhSach, 0, 1);
            root.Controls.Add(pnlNhacViec, 0, 3);

            Controls.Add(root);
            CapNhatMauChu();

            Load += (s, e) => CapNhat();
            Shown += (s, e) =>
            {
                // Không để focus tự cuộn trang làm mất phần đầu trang khi vừa mở.
                ActiveControl = null;
                AutoScrollPosition = Point.Empty;
            };
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            CapNhatMauChu();
        }

        private void CapNhatMauChu()
        {
            if (lblTongQuan == null)
            {
                return;
            }

            // Màu nền do người dùng chọn có thể rất tối; đổi màu chữ để vẫn đọc được.
            lblTongQuan.ForeColor = LaMauToi(BackColor) ? Color.White : MauChinh;
        }

        private static bool LaMauToi(Color mau)
        {
            return (0.299 * mau.R + 0.587 * mau.G + 0.114 * mau.B) < 140;
        }

        private void CapNhat()
        {
            var user = FrmDangNhap.NguoiDungHienTai;
            string tenHienThi = string.IsNullOrWhiteSpace(user?.HoTen) ? (user?.TenDangNhap ?? "Khách") : user.HoTen;
            string vaiTro = string.IsNullOrWhiteSpace(user?.VaiTro) ? "Người dùng" : user.VaiTro;
            lblChao.Text = $"Xin chào, {tenHienThi} ({user?.TenDangNhap})  •  Vai trò: {vaiTro}  •  {DateTime.Now:dddd, dd/MM/yyyy HH:mm}";
            lblHocKy.Text = $"Học kỳ {AppConfig.HocKyHienTai}   •   Tín chỉ {AppConfig.MinTC}–{AppConfig.MaxTC}   •   Ngưỡng cảnh báo {AppConfig.DiemCanhBao:0.0}";

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

            var nhacViec = new List<string>();

            // Dùng chung cách tính với form Cảnh báo học vụ để số liệu luôn khớp nhau.
            var dsCanhBao = _diemService.LayDanhSachCanhBao();
            int soSvCanhBao = dsCanhBao.Count;
            foreach (ThongTinCanhBao tt in dsCanhBao)
            {
                nhacViec.Add($"⚠ {tt.MSSV} {tt.HoTen}: ĐTB {tt.DtbTichLuy:0.00} ({tt.MucCanhBao})");
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
            ThemOThongKe("Khoa", soKhoa, 0, 0, Color.FromArgb(30, 90, 180));
            ThemOThongKe("Lớp", soLop, 1, 0, Color.FromArgb(0, 145, 160));
            ThemOThongKe("Sinh viên", soSv, 2, 0, Color.FromArgb(35, 145, 90));
            ThemOThongKe("Môn học", soMon, 3, 0, Color.FromArgb(220, 130, 30));
            ThemOThongKe("Lượt đăng ký", soDk, 0, 1, Color.FromArgb(110, 80, 180));
            ThemOThongKe("SV cảnh báo", soSvCanhBao, 1, 1, Color.FromArgb(200, 70, 70));
            ThemOThongKe("SV chưa ĐK", svChuaDk, 2, 1, Color.FromArgb(180, 140, 30));
            ThemOThongKe("SV thiếu ảnh", svThieuAnh, 3, 1, Color.FromArgb(110, 120, 135));

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

        private void ThemOThongKe(string nhan, int giaTri, int col, int row, Color mauNhan)
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Margin = new Padding(6),
                Padding = new Padding(16, 10, 10, 10),
                Height = 86
            };
            var accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = mauNhan
            };
            var lblGiaTri = new Label
            {
                Text = giaTri.ToString(),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = MauChinh,
                AutoSize = true,
                Location = new Point(20, 8)
            };
            var lblNhan = new Label
            {
                Text = nhan,
                Font = new Font("Segoe UI", 9F),
                ForeColor = MauChuPhu,
                AutoSize = true,
                Location = new Point(20, 48)
            };
            pnl.Controls.Add(accent);
            pnl.Controls.Add(lblGiaTri);
            pnl.Controls.Add(lblNhan);
            tlpThongKe.Controls.Add(pnl, col, row);
        }
    }
}
