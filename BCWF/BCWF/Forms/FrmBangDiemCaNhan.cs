using BCWF.Services;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// Bảng điểm cá nhân của một sinh viên.
    /// Dùng chung cho form Nhập điểm và form Cảnh báo học vụ.
    /// </summary>
    public class FrmBangDiemCaNhan : Form
    {
        private readonly DiemService _diemService = new DiemService();
        private readonly string _mssv;
        private readonly string _hoTen;

        /// <summary>Tạo form xem bảng điểm cá nhân của sinh viên.</summary>
        /// <param name="mssv">Mã sinh viên cần xem.</param>
        public FrmBangDiemCaNhan(string mssv)
        {
            _mssv = mssv;
            _hoTen = _diemService.LayTenSinhVien(mssv);

            Text = $"Bảng điểm cá nhân - {mssv} {_hoTen}";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(940, 480);
            MinimumSize = new Size(760, 380);

            TaoGiaoDien();
        }

        private void TaoGiaoDien()
        {
            var lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false
            };

            lv.Columns.Add("Môn học", 250);
            lv.Columns.Add("Tín chỉ", 70);
            lv.Columns.Add("Học kỳ", 80);
            lv.Columns.Add("Điểm QT", 80);
            lv.Columns.Add("Điểm GK", 80);
            lv.Columns.Add("Điểm CK", 80);
            lv.Columns.Add("Tổng kết", 90);
            lv.Columns.Add("Xếp loại", 90);
            lv.Columns.Add("Ghi chú", 110);

            foreach (Models.DangKy dangKy in _diemService.LayBangDiemCaNhan(_mssv))
            {
                double? tongKet = _diemService.TinhDiemTongKet(dangKy);
                bool rot = _diemService.LaMonRot(dangKy);

                var item = new ListViewItem(_diemService.LayTenMon(dangKy.MaMon));
                item.SubItems.Add(_diemService.LaySoTinChi(dangKy.MaMon).ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(dangKy.HocKy);
                item.SubItems.Add(DinhDangDiem(dangKy.DiemQT));
                item.SubItems.Add(DinhDangDiem(dangKy.DiemGK));
                item.SubItems.Add(DinhDangDiem(dangKy.DiemCK));
                item.SubItems.Add(tongKet.HasValue ? tongKet.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty);
                item.SubItems.Add(tongKet.HasValue ? _diemService.XepLoai(tongKet.Value) : string.Empty);
                item.SubItems.Add(rot ? "Rớt môn" : string.Empty);
                item.ForeColor = rot ? Color.Red : SystemColors.WindowText;

                lv.Items.Add(item);
            }

            double dtb = _diemService.TinhDiemTrungBinhTichLuy(_mssv);
            string muc = _diemService.XepMucCanhBao(dtb);

            var lblTtb = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0),
                Text = string.IsNullOrEmpty(muc)
                    ? $"Điểm trung bình tích lũy: {dtb:0.##} (Đạt)"
                    : $"Điểm trung bình tích lũy: {dtb:0.##} - {muc}"
            };

            if (!string.IsNullOrEmpty(muc))
            {
                lblTtb.ForeColor = Color.FromArgb(180, 0, 0);
            }

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 52 };

            var btnXuat = new Button
            {
                Text = "Xuất bảng điểm cá nhân ra CSV",
                Size = new Size(230, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            btnXuat.Location = new Point(pnlBottom.Width - 240, 9);
            pnlBottom.Controls.Add(btnXuat);

            btnXuat.Click += (s, e) => XuatCsv();

            var btnDong = new Button
            {
                Text = "Đóng",
                Size = new Size(90, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            btnDong.Location = new Point(pnlBottom.Width - 120, 9);
            pnlBottom.Controls.Add(btnDong);

            btnDong.Click += (s, e) => Close();

            Controls.Add(lv);
            Controls.Add(lblTtb);
            Controls.Add(pnlBottom);
        }

        private void XuatCsv()
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Xuất bảng điểm cá nhân ra CSV",
                Filter = "Tệp CSV|*.csv|Tất cả tệp|*.*",
                FileName = $"BangDiemCaNhan_{FileHelpers.ChuanHoaTenTep(_mssv)}.csv"
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                _diemService.XuatBangDiemCaNhanCsv(_mssv, sfd.FileName);
                MessageBox.Show($"Đã xuất bảng điểm cá nhân ra tệp:\n{sfd.FileName}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xuất CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }
    }
}
