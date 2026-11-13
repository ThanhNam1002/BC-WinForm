using BCWF.Common;
using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// Chức năng nâng cao: danh sách sinh viên bị cảnh báo học vụ.
    /// Cảnh báo theo điểm trung bình tích lũy và các ngưỡng trong App.config
    /// (DiemCanhBao / DiemCanhBaoNang / DiemBuocNghi).
    /// </summary>
    public partial class FrmCanhBaoHocVu : Form
    {
        private const string TatCa = "(Tất cả)";

        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        public FrmCanhBaoHocVu()
        {
            InitializeComponent();
        }

        private void FrmCanhBaoHocVu_Load(object sender, EventArgs e)
        {
            NapBoLoc();
            NapDanhSach();
        }

        #region Bộ lọc

        /// <summary>Nạp danh sách lớp và học kỳ vào các ComboBox lọc.</summary>
        private void NapBoLoc()
        {
            cboLop.Items.Clear();
            cboLop.Items.Add(TatCa);
            foreach (Lop lop in _store.Lops
                .Where(l => _store.SinhViens.Any(sv => string.Equals(sv.MaLop, l.MaLop, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(l => l.MaLop))
            {
                cboLop.Items.Add(lop.MaLop);
            }
            cboLop.SelectedIndex = 0;

            cboHocKy.Items.Clear();
            cboHocKy.Items.Add(TatCa);
            foreach (string hocKy in _store.DangKys
                .Select(dk => dk.HocKy)
                .Where(hk => !string.IsNullOrWhiteSpace(hk))
                .Distinct()
                .OrderBy(hk => hk))
            {
                cboHocKy.Items.Add(hocKy);
            }
            cboHocKy.SelectedIndex = 0;

            cboLop.SelectedIndexChanged += (s, e) => NapDanhSach();
            cboHocKy.SelectedIndexChanged += (s, e) => NapDanhSach();
            txtTimKiem.TextChanged += (s, e) => NapDanhSach();
        }

        /// <summary>Lớp đang chọn trong ô lọc (null nghĩa là tất cả).</summary>
        private string? LayLopChon()
        {
            return cboLop.SelectedItem is string maLop && maLop != TatCa ? maLop : null;
        }

        /// <summary>Học kỳ đang chọn trong ô lọc (null nghĩa là tất cả).</summary>
        private string? LayHocKyChon()
        {
            return cboHocKy.SelectedItem is string hocKy && hocKy != TatCa ? hocKy : null;
        }

        /// <summary>Từ khoá tìm kiếm (MSSV hoặc họ tên), rỗng nghĩa là không lọc.</summary>
        private string LayTuKhoa()
        {
            return txtTimKiem.Text.Trim();
        }

        #endregion

        #region Danh sách cảnh báo

        /// <summary>Đổ danh sách sinh viên bị cảnh báo lên ListView.</summary>
        private void NapDanhSach()
        {
            if (cboLop.Items.Count == 0)
            {
                return;
            }

            string? maLop = LayLopChon();
            string? hocKy = LayHocKyChon();
            string tuKhoa = LayTuKhoa();

            List<ThongTinCanhBao> danhSach = _diemService.LayDanhSachCanhBao(maLop, hocKy)
                .Where(t => string.IsNullOrEmpty(tuKhoa)
                            || t.MSSV.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase)
                            || t.HoTen.Contains(tuKhoa, StringComparison.OrdinalIgnoreCase))
                .ToList();

            lvCanhBao.BeginUpdate();
            try
            {
                lvCanhBao.Items.Clear();

                foreach (ThongTinCanhBao tt in danhSach)
                {
                    var item = new ListViewItem(tt.MSSV) { Tag = tt };
                    item.SubItems.Add(tt.HoTen);
                    item.SubItems.Add(tt.MaLop);
                    item.SubItems.Add(tt.DtbTichLuy.ToString("0.##", CultureInfo.InvariantCulture));
                    item.SubItems.Add(tt.DtbHocKy.HasValue
                        ? tt.DtbHocKy.Value.ToString("0.##", CultureInfo.InvariantCulture)
                        : string.Empty);
                    item.SubItems.Add(tt.SoMonRot.ToString(CultureInfo.InvariantCulture));
                    item.SubItems.Add(tt.TongTinChiRot.ToString(CultureInfo.InvariantCulture));
                    item.SubItems.Add(tt.MucCanhBao);
                    item.SubItems.Add(tt.MoTaMonRot);
                    item.ForeColor = LayMauMuc(tt.MucCanhBao);
                    lvCanhBao.Items.Add(item);
                }
            }
            finally
            {
                lvCanhBao.EndUpdate();
            }

            int canhBao = danhSach.Count(t => t.MucCanhBao == DiemService.MucCanhBao);
            int canhBaoNang = danhSach.Count(t => t.MucCanhBao == DiemService.MucCanhBaoNang);
            int buocNghi = danhSach.Count(t => t.MucCanhBao == DiemService.MucBuocNghi);

            lblTongHop.Text =
                $"Cảnh báo: {canhBao} | Cảnh báo nặng: {canhBaoNang} | Buộc nghỉ: {buocNghi} | Tổng: {danhSach.Count}";

            lblGhiChu.Text =
                $"Ngưỡng: cảnh báo < {AppConfig.DiemCanhBao:0.0}, cảnh báo nặng < {AppConfig.DiemCanhBaoNang:0.0}, " +
                $"buộc nghỉ học < {AppConfig.DiemBuocNghi:0.0} (App.config). " +
                "Nhấp đúp vào một sinh viên để xem bảng điểm cá nhân.";
        }

        /// <summary>Màu chữ theo mức cảnh báo.</summary>
        private static Color LayMauMuc(string muc)
        {
            if (muc == DiemService.MucBuocNghi)
            {
                return Color.Red;
            }

            if (muc == DiemService.MucCanhBaoNang)
            {
                return Color.FromArgb(200, 90, 0);
            }

            return Color.FromArgb(150, 110, 0);
        }

        #endregion

        #region Sự kiện

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            try
            {
                _store.LoadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NapBoLoc();
            NapDanhSach();
        }

        private void btnXoaTim_Click(object sender, EventArgs e)
        {
            txtTimKiem.Clear();
        }

        private void lvCanhBao_DoubleClick(object sender, EventArgs e)
        {
            XemBangDiemCaNhan();
        }

        private void btnXemBangDiem_Click(object sender, EventArgs e)
        {
            XemBangDiemCaNhan();
        }

        /// <summary>Mở bảng điểm cá nhân của sinh viên đang chọn trên danh sách.</summary>
        private void XemBangDiemCaNhan()
        {
            if (lvCanhBao.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn sinh viên trên danh sách.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvCanhBao.SelectedItems[0].Tag is not ThongTinCanhBao tt)
            {
                return;
            }

            using FrmBangDiemCaNhan form = new FrmBangDiemCaNhan(tt.MSSV);
            form.ShowDialog(this);
        }

        private void btnXuatCsv_Click(object sender, EventArgs e)
        {
            if (lvCanhBao.Items.Count == 0)
            {
                MessageBox.Show("Không có sinh viên nào bị cảnh báo để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Xuất danh sách cảnh báo học vụ",
                Filter = "Tệp CSV (*.csv)|*.csv",
                FileName = $"CanhBaoHocVu_{DateTime.Now:yyyyMMdd}.csv"
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                var cacDong = new List<string[]>
                {
                    new[]
                    {
                        "MSSV", "Họ tên", "Lớp", "ĐTB tích lũy", "ĐTB học kỳ",
                        "Số môn rớt", "Tổng TC rớt", "Mức cảnh báo", "Môn bị rớt"
                    }
                };

                foreach (ListViewItem item in lvCanhBao.Items)
                {
                    if (item.Tag is not ThongTinCanhBao tt)
                    {
                        continue;
                    }

                    cacDong.Add(new[]
                    {
                        tt.MSSV,
                        tt.HoTen,
                        tt.MaLop,
                        tt.DtbTichLuy.ToString("0.##", CultureInfo.InvariantCulture),
                        tt.DtbHocKy.HasValue ? tt.DtbHocKy.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty,
                        tt.SoMonRot.ToString(CultureInfo.InvariantCulture),
                        tt.TongTinChiRot.ToString(CultureInfo.InvariantCulture),
                        tt.MucCanhBao,
                        tt.MoTaMonRot
                    });
                }

                FileHelpers.GhiCsv(sfd.FileName, cacDong);
                MessageBox.Show($"Đã xuất danh sách cảnh báo ra tệp:\n{sfd.FileName}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                FileHelpers.GhiLog("Lỗi xuất CSV cảnh báo học vụ: " + loi.Message);
                MessageBox.Show(loi.Message, "Lỗi xuất CSV",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
