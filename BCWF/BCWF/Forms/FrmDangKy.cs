using BCWF.Common;
using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// SV2 - Đăng ký học phần.
    /// Tab 1 quản lý môn học; tab 2 đăng ký bằng cách chuyển môn giữa 2 ListBox,
    /// kiểm tra tổng tín chỉ MinTC - MaxTC và chống đăng ký trùng môn.
    /// </summary>
    public partial class FrmDangKy : Form
    {
        private const string KhongTienQuyet = "(không có)";

        private readonly DataStore _store = DataStore.Instance;
        private readonly DangKyService _dangKyService = new DangKyService();

        public FrmDangKy()
        {
            InitializeComponent();
        }

        private void FrmDangKy_Load(object sender, EventArgs e)
        {
            NapMonHoc();
            NapDanhSachLop();

            // Chọn học kỳ mặc định đọc từ App.config.
            string hocKyMacDinh = AppConfig.HocKyHienTai;
            int viTri = cboHocKy.Items.IndexOf(hocKyMacDinh);
            cboHocKy.SelectedIndex = viTri >= 0 ? viTri : 0;

            lblGioiHan.Text = $"Giới hạn tín chỉ: {AppConfig.MinTC} – {AppConfig.MaxTC}";
        }

        #region Nạp dữ liệu

        /// <summary>Đổ danh sách môn học lên ListView và ComboBox tiên quyết.</summary>
        private void NapMonHoc()
        {
            lvMonHoc.BeginUpdate();
            lvMonHoc.Items.Clear();
            foreach (MonHoc mon in _dangKyService.LayTatCaMonHoc())
            {
                var item = new ListViewItem(mon.MaMon)
                {
                    Tag = mon
                };
                item.SubItems.Add(mon.TenMon);
                item.SubItems.Add(mon.SoTC.ToString());
                item.SubItems.Add(string.IsNullOrWhiteSpace(mon.MaMonTienQuyet) ? string.Empty : mon.MaMonTienQuyet);
                lvMonHoc.Items.Add(item);
            }
            lvMonHoc.EndUpdate();

            // ComboBox tiên quyết: phần tử đầu là "(không có)".
            cboTienQuyet.BeginUpdate();
            cboTienQuyet.Items.Clear();
            cboTienQuyet.Items.Add(KhongTienQuyet);
            foreach (MonHoc mon in _dangKyService.LayTatCaMonHoc())
            {
                cboTienQuyet.Items.Add(mon.MaMon);
            }
            cboTienQuyet.SelectedIndex = 0;
            cboTienQuyet.EndUpdate();
        }

        private void NapDanhSachLop()
        {
            _dangNap = true;
            try
            {
                cboLop.Items.Clear();
                foreach (Lop lop in _store.Lops.OrderBy(l => l.MaLop))
                {
                    cboLop.Items.Add(lop);
                }

                cboLop.SelectedIndex = cboLop.Items.Count > 0 ? 0 : -1;
            }
            finally
            {
                _dangNap = false;
            }
        }

        private void NapDanhSachSinhVien()
        {
            _dangNap = true;
            try
            {
                cboSinhVien.Items.Clear();

                string? maLop = (cboLop.SelectedItem as Lop)?.MaLop;
                if (!string.IsNullOrWhiteSpace(maLop))
                {
                    foreach (SinhVien sv in _store.SinhViens
                        .Where(s => string.Equals(s.MaLop, maLop, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(s => s.MSSV))
                    {
                        cboSinhVien.Items.Add(sv);
                    }
                }

                cboSinhVien.SelectedIndex = cboSinhVien.Items.Count > 0 ? 0 : -1;
            }
            finally
            {
                _dangNap = false;
            }
        }

        /// <summary>Đổ lại 2 ListBox theo dữ liệu dangky.txt của sinh viên/học kỳ đang chọn.</summary>
        private void NapDangKyHienTai()
        {
            string? mssv = (cboSinhVien.SelectedItem as SinhVien)?.MSSV;
            string? hocKy = cboHocKy.SelectedItem as string;

            lstChuaDangKy.BeginUpdate();
            lstDaDangKy.BeginUpdate();
            lstChuaDangKy.Items.Clear();
            lstDaDangKy.Items.Clear();

            var daDangKy = _dangKyService.LayMonDaDangKy(mssv ?? string.Empty, hocKy ?? string.Empty);
            var maDaDangKy = new HashSet<string>(daDangKy.Select(m => m.MaMon), StringComparer.OrdinalIgnoreCase);

            foreach (MonHoc mon in _dangKyService.LayTatCaMonHoc())
            {
                if (maDaDangKy.Contains(mon.MaMon))
                {
                    lstDaDangKy.Items.Add(mon);
                }
                else
                {
                    lstChuaDangKy.Items.Add(mon);
                }
            }

            lstChuaDangKy.EndUpdate();
            lstDaDangKy.EndUpdate();

            CapNhatTongTinChi();
        }

        /// <summary>Cập nhật nhãn tổng tín chỉ theo các môn đang chọn.</summary>
        private void CapNhatTongTinChi()
        {
            var dsMaMon = lstDaDangKy.Items.Cast<MonHoc>().Select(m => m.MaMon).ToList();
            int tong = _dangKyService.TinhTongTinChi(dsMaMon);
            lblTongTinChi.Text = $"Tổng tín chỉ: {tong}";

            if (tong == 0)
            {
                lblTongTinChi.ForeColor = SystemColors.ControlText;
            }
            else if (tong < AppConfig.MinTC)
            {
                lblTongTinChi.ForeColor = Color.OrangeRed;
            }
            else if (tong > AppConfig.MaxTC)
            {
                lblTongTinChi.ForeColor = Color.Red;
            }
            else
            {
                lblTongTinChi.ForeColor = Color.Green;
            }
        }

        #endregion

        #region Quản lý môn học

        private void lvMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMonHoc.SelectedItems.Count == 0)
            {
                return;
            }

            if (lvMonHoc.SelectedItems[0].Tag is MonHoc mon)
            {
                _dangNap = true;
                try
                {
                    txtMaMon.Text = mon.MaMon;
                    txtTenMon.Text = mon.TenMon;
                    nudSoTC.Value = Math.Min(Math.Max(mon.SoTC, nudSoTC.Minimum), nudSoTC.Maximum);

                    int viTri = string.IsNullOrWhiteSpace(mon.MaMonTienQuyet)
                        ? 0
                        : cboTienQuyet.FindStringExact(mon.MaMonTienQuyet);
                    cboTienQuyet.SelectedIndex = viTri >= 0 ? viTri : 0;
                }
                finally
                {
                    _dangNap = false;
                }
            }
        }

        private MonHoc? DocMonTuForm()
        {
            string ma = txtMaMon.Text.Trim();
            string ten = txtTenMon.Text.Trim();

            if (string.IsNullOrWhiteSpace(ma))
            {
                MessageBox.Show("Mã môn không được để trống.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaMon.Focus();
                return null;
            }

            if (string.IsNullOrWhiteSpace(ten))
            {
                MessageBox.Show("Tên môn không được để trống.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenMon.Focus();
                return null;
            }

            string? maTienQuyet = cboTienQuyet.SelectedItem as string;
            return new MonHoc
            {
                MaMon = ma,
                TenMon = ten,
                SoTC = (int)nudSoTC.Value,
                MaMonTienQuyet = string.Equals(maTienQuyet, KhongTienQuyet, StringComparison.OrdinalIgnoreCase)
                    ? string.Empty
                    : (maTienQuyet ?? string.Empty)
            };
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            MonHoc? mon = DocMonTuForm();
            if (mon == null)
            {
                return;
            }

            try
            {
                _dangKyService.ThemMonHoc(mon);
                NapMonHoc();
                XoaVungNhap();
                MessageBox.Show("Thêm môn học thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                MessageBox.Show(loi.Message, "Không thêm được môn học",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSuaMon_Click(object sender, EventArgs e)
        {
            if (lvMonHoc.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn môn học cần sửa trong danh sách bên phải.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvMonHoc.SelectedItems[0].Tag is not MonHoc monGoc)
            {
                return;
            }

            MonHoc? mon = DocMonTuForm();
            if (mon == null)
            {
                return;
            }

            try
            {
                _dangKyService.CapNhatMonHoc(mon, monGoc.MaMon);
                NapMonHoc();
                NapDangKyHienTai();
                MessageBox.Show("Cập nhật môn học thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                MessageBox.Show(loi.Message, "Không sửa được môn học",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            if (lvMonHoc.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn môn học cần xóa trong danh sách bên phải.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvMonHoc.SelectedItems[0].Tag is not MonHoc mon)
            {
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                $"Bạn có chắc muốn xóa môn '{mon.MaMon} - {mon.TenMon}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _dangKyService.XoaMonHoc(mon.MaMon);
                NapMonHoc();
                XoaVungNhap();
                NapDangKyHienTai();
                MessageBox.Show("Đã xóa môn học.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                MessageBox.Show(loi.Message, "Không xóa được môn học",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void XoaVungNhap()
        {
            _dangNap = true;
            try
            {
                txtMaMon.Text = string.Empty;
                txtTenMon.Text = string.Empty;
                nudSoTC.Value = 3;
                cboTienQuyet.SelectedIndex = 0;
            }
            finally
            {
                _dangNap = false;
            }
        }

        #endregion

        #region Đăng ký học phần

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dangNap)
            {
                return;
            }

            NapDanhSachSinhVien();
        }

        private void cboSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dangNap)
            {
                return;
            }

            NapDangKyHienTai();
        }

        private void cboHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dangNap)
            {
                return;
            }

            NapDangKyHienTai();
        }

        /// <summary>Kiểm tra một môn trước khi đưa vào danh sách đã chọn.</summary>
        private bool ChoPhepChonMon(string mssv, string maMon, string hocKy)
        {
            if (_dangKyService.DaDangKy(mssv, maMon, hocKy))
            {
                MessageBox.Show($"Môn '{maMon}' đã có trong danh sách đăng ký của học kỳ này.", "Trùng đăng ký",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (_dangKyService.DaDatMon(mssv, maMon))
            {
                MessageBox.Show($"Không thể đăng ký môn '{maMon}' vì sinh viên đã đạt môn này.", "Môn đã đạt",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!_dangKyService.KiemTraTienQuyet(mssv, maMon, out string baoTienQuyet))
            {
                MessageBox.Show(baoTienQuyet, "Chưa đạt môn tiên quyết",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnChonMon_Click(object sender, EventArgs e)
        {
            string? mssv = (cboSinhVien.SelectedItem as SinhVien)?.MSSV;
            string? hocKy = cboHocKy.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(mssv))
            {
                MessageBox.Show("Hãy chọn lớp và sinh viên trước.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Kiểm tra từng môn được chọn; các môn đạt điều kiện mới chuyển sang bên phải.
            var monHopLe = new List<MonHoc>();
            foreach (MonHoc mon in lstChuaDangKy.SelectedItems.Cast<MonHoc>().ToList())
            {
                if (ChoPhepChonMon(mssv, mon.MaMon, hocKy ?? string.Empty))
                {
                    monHopLe.Add(mon);
                }
            }

            if (monHopLe.Count == 0)
            {
                return;
            }

            foreach (MonHoc mon in monHopLe)
            {
                lstChuaDangKy.Items.Remove(mon);
                lstDaDangKy.Items.Add(mon);
            }

            CapNhatTongTinChi();
        }

        private void btnBoMon_Click(object sender, EventArgs e)
        {
            var monBo = lstDaDangKy.SelectedItems.Cast<MonHoc>().ToList();
            foreach (MonHoc mon in monBo)
            {
                lstDaDangKy.Items.Remove(mon);
                lstChuaDangKy.Items.Add(mon);
            }

            CapNhatTongTinChi();
        }

        private void lstChuaDangKy_DoubleClick(object sender, EventArgs e)
        {
            btnChonMon_Click(sender, e);
        }

        private void lstDaDangKy_DoubleClick(object sender, EventArgs e)
        {
            btnBoMon_Click(sender, e);
        }

        private void btnLuuDangKy_Click(object sender, EventArgs e)
        {
            string? mssv = (cboSinhVien.SelectedItem as SinhVien)?.MSSV;
            string? hocKy = cboHocKy.SelectedItem as string;

            if (string.IsNullOrWhiteSpace(mssv))
            {
                MessageBox.Show("Hãy chọn lớp và sinh viên trước khi lưu.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(hocKy))
            {
                MessageBox.Show("Hãy chọn học kỳ.", "Thiếu dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dsMaMon = lstDaDangKy.Items.Cast<MonHoc>().Select(m => m.MaMon).ToList();

            // Ràng buộc tổng tín chỉ trong giới hạn MinTC - MaxTC của App.config.
            if (!_dangKyService.KiemTraGioiHanTinChi(dsMaMon, out string baoTinChi))
            {
                MessageBox.Show(baoTinChi, "Vượt giới hạn tín chỉ",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nâng cao: chặn các môn trùng lịch học với nhau.
            if (!_dangKyService.KiemTraTrungLich(dsMaMon, out string baoTrungLich))
            {
                MessageBox.Show(baoTrungLich, "Trùng lịch học",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                $"Lưu đăng ký {dsMaMon.Count} môn ({_dangKyService.TinhTongTinChi(dsMaMon)} tín chỉ) "
                + $"cho sinh viên {mssv} trong học kỳ {hocKy}?",
                "Xác nhận lưu", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _dangKyService.LuuDanhSachDangKy(mssv, hocKy, dsMaMon);
                NapDangKyHienTai();
                MessageBox.Show("Đã lưu đăng ký học phần xuống tệp dangky.txt.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                MessageBox.Show(loi.Message, "Lỗi lưu đăng ký",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoiDangKy_Click(object sender, EventArgs e)
        {
            NapDangKyHienTai();
        }

        #endregion

        /// <summary>Cờ chặn sự kiện SelectedIndexChanged khi đang nạp dữ liệu bằng code.</summary>
        private bool _dangNap;
    }
}
