using BCWF.Common;
using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// SV3 - Điểm.
    /// Nhập điểm theo lớp và môn, sửa trực tiếp trên ListView, tính điểm tổng kết
    /// (30% QT + 30% GK + 40% CK), xếp loại, tô đỏ môn rớt, điểm trung bình tích lũy
    /// và xuất bảng điểm của lớp ra CSV.
    /// </summary>
    public partial class FrmNhapDiem : Form
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        /// <summary>
        /// Chỉ số cột trên ListView: 0 = MSSV, 1 = Họ tên, 2 = Điểm QT,
        /// 3 = Điểm GK, 4 = Điểm CK, 5 = Tổng kết, 6 = Xếp loại,
        /// 7 = ĐTB tích lũy, 8 = Ghi chú.
        /// </summary>
        private const int CotMSSV = 0;
        private const int CotDiemQT = 2;
        private const int CotDiemGK = 3;
        private const int CotDiemCK = 4;
        private const int CotTongKet = 5;
        private const int CotXepLoai = 6;
        private const int CotDtbTichLuy = 7;
        private const int CotGhiChu = 8;

        private bool _dangNapDanhSach;

        /// <summary>Có điểm vừa sửa nhưng chưa ghi xuống dangky.txt hay không.</summary>
        private bool _coThayDoiDiem;

        private string _lopDangXem = string.Empty;
        private string _monDangXem = string.Empty;
        private string _hocKyDangXem = string.Empty;

        /// <summary>Ô nhập điểm đặt đè lên ô điểm đang sửa trên ListView.</summary>
        private readonly TextBox _txtDiem = new TextBox();

        /// <summary>Dòng đang sửa và cột điểm tương ứng.</summary>
        private ListViewItem? _itemDangSua;
        private int _cotDangSua = -1;

        /// <summary>Dòng/cột người dùng vừa bấm chuột (dùng để mở ô nhập khi thả chuột).</summary>
        private ListViewItem? _itemChuDong;
        private int _cotChuDong = -1;

        public FrmNhapDiem()
        {
            InitializeComponent();

            FormClosing += FrmNhapDiem_FormClosing;
            ChuẩnBịÔNhập();
        }

        private void FrmNhapDiem_Load(object sender, EventArgs e)
        {
            // Nạp danh sách lớp, môn học và chọn học kỳ mặc định.
            NapDanhSachLop();
            NapDanhSachMon();
            ChonHocKyMacDinh();

            // Menu ngữ cảnh để xem bảng điểm cá nhân của sinh viên đang chọn.
            var menuChuot = new ContextMenuStrip();
            menuChuot.Items.Add("Xem bảng điểm cá nhân...", null, (s, e) => XemBangDiemCaNhan());
            lvDiem.ContextMenuStrip = menuChuot;

            NapLaiBangDiem();
        }

        #region Nạp dữ liệu

        /// <summary>Nạp danh sách lớp vào ComboBox.</summary>
        private void NapDanhSachLop()
        {
            _dangNapDanhSach = true;
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
                _dangNapDanhSach = false;
            }
        }

        /// <summary>Nạp danh sách môn học vào ComboBox.</summary>
        private void NapDanhSachMon()
        {
            _dangNapDanhSach = true;
            try
            {
                cboMon.Items.Clear();
                foreach (MonHoc mon in _store.MonHocs.OrderBy(m => m.MaMon))
                {
                    cboMon.Items.Add(mon);
                }

                cboMon.SelectedIndex = cboMon.Items.Count > 0 ? 0 : -1;
            }
            finally
            {
                _dangNapDanhSach = false;
            }
        }

        /// <summary>Chọn học kỳ mặc định lấy từ App.config.</summary>
        private void ChonHocKyMacDinh()
        {
            _dangNapDanhSach = true;
            try
            {
                string hocKy = AppConfig.HocKyHienTai;

                for (int i = 0; i < cboHocKy.Items.Count; i++)
                {
                    string? giaTri = cboHocKy.Items[i]?.ToString();
                    if (string.Equals(giaTri, hocKy, StringComparison.OrdinalIgnoreCase))
                    {
                        cboHocKy.SelectedIndex = i;
                        return;
                    }
                }

                if (cboHocKy.Items.Count > 0)
                {
                    cboHocKy.SelectedIndex = 0;
                }
            }
            finally
            {
                _dangNapDanhSach = false;
            }
        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dangNapDanhSach)
            {
                return;
            }

            if (!KiemTraDiemChuaLuu())
            {
                KhoiPhucLop();
                return;
            }

            // Khi đổi lớp thì nạp lại bảng điểm (nếu đã chọn môn).
            if (cboMon.SelectedIndex >= 0)
            {
                NapLaiBangDiem();
            }
        }

        private void cboMon_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDanhSach)
            {
                return;
            }

            if (!KiemTraDiemChuaLuu())
            {
                KhoiPhucMon();
                return;
            }

            // Đổi môn phải nạp lại bảng điểm, nếu không ListView vẫn hiển thị
            // điểm của môn cũ trong khi ComboBox đã chọn môn mới.
            NapLaiBangDiem();
        }

        private void cboHocKy_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_dangNapDanhSach)
            {
                return;
            }

            if (!KiemTraDiemChuaLuu())
            {
                KhoiPhucHocKy();
                return;
            }

            NapLaiBangDiem();
        }

        private void btnTaiDanhSach_Click(object sender, EventArgs e)
        {
            if (!KiemTraDiemChuaLuu())
            {
                return;
            }

            // Đổ sinh viên của lớp và môn đang chọn lên lvDiem.
            NapLaiBangDiem();
        }

        /// <summary>
        /// Chốt ô điểm đang gõ và kiểm tra xem còn điểm chưa ghi xuống tệp hay không.
        /// Trả về false để form biết cần hủy thao tác chuyển lớp/môn/học kỳ.
        /// </summary>
        private bool KiemTraDiemChuaLuu()
        {
            if (_itemDangSua != null)
            {
                GhiDiemDangSua();
            }

            if (!_coThayDoiDiem)
            {
                return true;
            }

            MessageBox.Show(
                "Bạn có điểm chưa lưu. Hãy bấm 'Lưu điểm' trước khi đổi lớp, môn học hoặc học kỳ.",
                "Điểm chưa lưu",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return false;
        }

        private void KhoiPhucLop()
        {
            _dangNapDanhSach = true;
            try
            {
                foreach (object item in cboLop.Items)
                {
                    if (item is Lop lop &&
                        string.Equals(lop.MaLop, _lopDangXem, StringComparison.OrdinalIgnoreCase))
                    {
                        cboLop.SelectedItem = item;
                        break;
                    }
                }
            }
            finally
            {
                _dangNapDanhSach = false;
            }
        }

        private void KhoiPhucMon()
        {
            _dangNapDanhSach = true;
            try
            {
                foreach (object item in cboMon.Items)
                {
                    if (item is MonHoc mon &&
                        string.Equals(mon.MaMon, _monDangXem, StringComparison.OrdinalIgnoreCase))
                    {
                        cboMon.SelectedItem = item;
                        break;
                    }
                }
            }
            finally
            {
                _dangNapDanhSach = false;
            }
        }

        private void KhoiPhucHocKy()
        {
            _dangNapDanhSach = true;
            try
            {
                cboHocKy.SelectedItem = _hocKyDangXem;
            }
            finally
            {
                _dangNapDanhSach = false;
            }
        }

        private void FrmNhapDiem_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_itemDangSua != null)
            {
                GhiDiemDangSua();
            }

            if (!_coThayDoiDiem)
            {
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                "Bạn có điểm chưa lưu. Thoát và bỏ các thay đổi?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (xacNhan != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        #endregion

        #region Bảng điểm

        /// <summary>Đổ danh sách sinh viên của lớp (kèm điểm của môn/học kỳ đang chọn) lên ListView.</summary>
        private void NapLaiBangDiem()
        {
            HuyDangSua();

            if (cboLop.SelectedItem is not Lop lop)
            {
                MessageBox.Show("Hãy chọn lớp.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboMon.SelectedItem is not MonHoc mon)
            {
                MessageBox.Show("Hãy chọn môn học.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hocKy = cboHocKy.SelectedItem?.ToString() ?? AppConfig.HocKyHienTai;

            lvDiem.BeginUpdate();
            try
            {
                lvDiem.Items.Clear();

                foreach (SinhVien sv in _diemService.LaySinhVienLop(lop.MaLop))
                {
                    DangKy? dangKy = _diemService.TimDangKy(sv.MSSV, mon.MaMon, hocKy);
                    var dong = new DongDiem
                    {
                        MSSV = sv.MSSV,
                        HoTen = sv.HoTen,
                        MaMon = mon.MaMon,
                        HocKy = hocKy,
                        DangKy = dangKy
                    };

                    lvDiem.Items.Add(TaoListViewItem(dong));
                }
            }
            finally
            {
                lvDiem.EndUpdate();
            }

            CapNhatTongKet();

            // Ghi nhớ mốc đang xem để có thể quay lại nếu người dùng chọn không bỏ điểm chưa lưu.
            _lopDangXem = lop.MaLop;
            _monDangXem = mon.MaMon;
            _hocKyDangXem = hocKy;
            _coThayDoiDiem = false;
        }

        /// <summary>Tạo dòng ListView từ dữ liệu điểm của một sinh viên.</summary>
        private ListViewItem TaoListViewItem(DongDiem dong)
        {
            var item = new ListViewItem(dong.MSSV);
            item.SubItems.Add(dong.HoTen);          // 1 - Họ tên
            item.SubItems.Add(DinhDangDiem(dong.DiemQT)); // 2 - Điểm QT
            item.SubItems.Add(DinhDangDiem(dong.DiemGK)); // 3 - Điểm GK
            item.SubItems.Add(DinhDangDiem(dong.DiemCK)); // 4 - Điểm CK
            item.SubItems.Add(string.Empty);       // 5 - Tổng kết
            item.SubItems.Add(string.Empty);       // 6 - Xếp loại
            item.SubItems.Add(string.Empty);       // 7 - ĐTB tích lũy
            item.SubItems.Add(string.Empty);       // 8 - Ghi chú
            item.Tag = dong;

            CapNhatKetQuaDong(item, dong);
            return item;
        }

        /// <summary>Tính lại tổng kết, xếp loại, ĐTB tích lũy và màu chữ cho một dòng.</summary>
        private void CapNhatKetQuaDong(ListViewItem item, DongDiem dong)
        {
            double? tongKet = dong.DangKy == null ? null : _diemService.TinhDiemTongKet(dong.DangKy);
            bool rot = dong.DangKy != null && _diemService.LaMonRot(dong.DangKy);

            item.SubItems[CotTongKet].Text = tongKet.HasValue
                ? tongKet.Value.ToString("0.##", CultureInfo.InvariantCulture)
                : string.Empty;

            item.SubItems[CotXepLoai].Text = tongKet.HasValue ? _diemService.XepLoai(tongKet.Value) : string.Empty;

            double dtb = _diemService.TinhDiemTrungBinhTichLuy(dong.MSSV);
            bool coDiemTichLuy = _store.DangKys.Any(dk =>
                string.Equals(dk.MSSV, dong.MSSV, StringComparison.OrdinalIgnoreCase) && dk.DaNhapDiem);
            item.SubItems[CotDtbTichLuy].Text = coDiemTichLuy
                ? dtb.ToString("0.##", CultureInfo.InvariantCulture)
                : string.Empty;

            item.SubItems[CotGhiChu].Text = rot ? "Rớt môn" : (dong.DangKy == null ? "Chưa đăng ký" : string.Empty);

            // Tô đỏ dòng khi môn bị rớt.
            item.ForeColor = rot ? Color.Red : SystemColors.WindowText;
        }

        /// <summary>Cập nhật nhãn tổng kết (số sinh viên, số môn rớt).</summary>
        private void CapNhatTongKet()
        {
            int soSinhVien = lvDiem.Items.Count;
            int soMonRot = lvDiem.Items.Cast<ListViewItem>()
                .Count(i => i.Tag is DongDiem dong && dong.DangKy != null && _diemService.LaMonRot(dong.DangKy));

            lblTongKet.Text = $"Số sinh viên: {soSinhVien} - Số môn rớt: {soMonRot}";
        }

        private void btnLuuDiem_Click(object sender, EventArgs e)
        {
            // Chốt ô đang sửa rồi ghi điểm xuống dangky.txt.
            if (_itemDangSua != null)
            {
                GhiDiemDangSua();
            }

            if (lvDiem.Items.Count == 0)
            {
                MessageBox.Show("Chưa có dữ liệu điểm để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var diemCanNap = new List<DiemService.DiemCanNap>();

                foreach (ListViewItem item in lvDiem.Items)
                {
                    if (item.Tag is not DongDiem dong || dong.DangKy == null)
                    {
                        continue;
                    }

                    diemCanNap.Add(new DiemService.DiemCanNap(dong.MSSV, dong.MaMon, dong.HocKy,
                        dong.DiemQT, dong.DiemGK, dong.DiemCK));
                }

                _diemService.LuuDiem(diemCanNap);
                _coThayDoiDiem = false;

                MessageBox.Show($"Đã lưu điểm của {diemCanNap.Count} sinh viên xuống tệp dangky.txt.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi lưu điểm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NapLaiBangDiem();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            if (_itemDangSua != null)
            {
                GhiDiemDangSua();
            }

            if (_coThayDoiDiem)
            {
                DialogResult xacNhan = MessageBox.Show(
                    "Bạn có điểm chưa lưu. Tải lại sẽ bỏ các thay đổi này. Tiếp tục?",
                    "Xác nhận tải lại",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (xacNhan != DialogResult.Yes)
                {
                    return;
                }
            }

            // Tải lại bảng điểm từ tệp.
            try
            {
                _coThayDoiDiem = false;
                _store.LoadAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            NapDanhSachLop();
            NapDanhSachMon();
            ChonHocKyMacDinh();
            NapLaiBangDiem();
        }

        #endregion

        #region Sửa điểm trực tiếp trên ListView

        /// <summary>
        /// Chuẩn bị ô nhập điểm đặt đè lên ô đang sửa của ListView.
        /// ListView chỉ hỗ trợ sửa nhãn ở cột đầu tiên (MSSV) nên các cột điểm
        /// QT/GK/CK được sửa bằng ô nhập này, đặt đúng vị trí ô đang bấm.
        /// </summary>
        private void ChuẩnBịÔNhập()
        {
            _txtDiem.BorderStyle = BorderStyle.FixedSingle;
            _txtDiem.TextAlign = HorizontalAlignment.Center;
            _txtDiem.Visible = false;
            _txtDiem.KeyDown += _txtDiem_KeyDown;
            _txtDiem.LostFocus += (s, e) => GhiDiemDangSua();
            lvDiem.Controls.Add(_txtDiem);

            // Giữ ô nhập đúng vị trí khi danh sách được cuộn.
            lvDiem.MouseWheel += (s, e) => CapNhatViTriÔNhập();

            lvDiem.LabelEdit = false;
            lvDiem.MouseDown += lvDiem_MouseDown;
            lvDiem.MouseUp += lvDiem_MouseUp;
        }

        /// <summary>
        /// Bấm chuột vào một ô điểm để sửa trực tiếp trên dòng đang chọn.
        /// Việc bắt đầu sửa được thực hiện ở MouseUp để ListView đã nhận focus trước,
        /// tránh ô nhập mất focus ngay lập tức.
        /// </summary>
        private void lvDiem_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                _itemChuDong = null;
                _cotChuDong = -1;
                return;
            }

            ListViewItem? item = lvDiem.GetItemAt(e.X, e.Y);
            int cot = LayCotTaiToaDo(item, e.X);

            if (item?.Tag is not DongDiem dong || cot < CotDiemQT || cot > CotDiemCK)
            {
                _itemChuDong = null;
                _cotChuDong = -1;
                return;
            }

            _itemChuDong = item;
            _cotChuDong = cot;
        }

        private void lvDiem_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || _itemChuDong == null || _cotChuDong < 0)
            {
                return;
            }

            ListViewItem item = _itemChuDong;
            int cot = _cotChuDong;
            _itemChuDong = null;
            _cotChuDong = -1;

            if (item.Tag is not DongDiem dong)
            {
                return;
            }

            // Chỉ mở ô nhập nếu người dùng bấm đúng ô đang chọn.
            if (lvDiem.SelectedItems.Count == 0 || lvDiem.SelectedItems[0] != item)
            {
                item.Selected = true;
            }

            BatDauSua(item, dong, cot);
        }

        /// <summary>Xác định cột tương ứng với toạ độ X trên ListView.</summary>
        private int LayCotTaiToaDo(ListViewItem? item, int x)
        {
            // Ưu tiên dùng vùng của từng ô (sub-item) để chính xác hơn.
            if (item != null)
            {
                for (int i = 1; i < item.SubItems.Count; i++)
                {
                    Rectangle vung = item.SubItems[i].Bounds;
                    if (vung.Width > 0 && x >= vung.X && x < vung.X + vung.Width)
                    {
                        return i;
                    }
                }
            }

            // Dự phòng: tính theo chiều rộng cột.
            int viTri = 0;
            for (int i = 0; i < lvDiem.Columns.Count; i++)
            {
                if (x < viTri + lvDiem.Columns[i].Width)
                {
                    return i;
                }

                viTri += lvDiem.Columns[i].Width;
            }

            return CotMSSV;
        }

        /// <summary>Đưa ô nhập vào ô điểm tương ứng và cho người dùng gõ.</summary>
        private void BatDauSua(ListViewItem item, DongDiem dong, int cot)
        {
            // Chốt giá trị đang sửa (nếu có) trước khi mở ô nhập mới.
            if (_itemDangSua != null)
            {
                GhiDiemDangSua();
            }

            Rectangle vung = item.SubItems[cot].Bounds;
            if (vung.Width <= 0 || vung.Height <= 0)
            {
                return;
            }

            _itemDangSua = item;
            _cotDangSua = cot;

            _txtDiem.Text = item.SubItems[cot].Text;
            CapNhatViTriÔNhập();
            _txtDiem.SelectAll();
            _txtDiem.Visible = true;
            _txtDiem.BringToFront();
            _txtDiem.Focus();
        }

        /// <summary>Đặt ô nhập đúng vào ô điểm đang sửa (toạ độ tương đối trong ListView).</summary>
        private void CapNhatViTriÔNhập()
        {
            if (_itemDangSua == null || _cotDangSua < 0)
            {
                return;
            }

            Rectangle vung = _itemDangSua.SubItems[_cotDangSua].Bounds;
            if (vung.Width <= 0 || vung.Height <= 0)
            {
                return;
            }

            _txtDiem.Bounds = new Rectangle(vung.X + 1, vung.Y, Math.Max(vung.Width - 2, 20), Math.Max(vung.Height - 2, 16));
        }

        /// <summary>Enter: chốt điểm; Escape: hủy việc sửa.</summary>
        private void _txtDiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                GhiDiemDangSua();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                HuyDangSua();
            }
        }

        /// <summary>Lấy giá trị trong ô nhập, kiểm tra hợp lệ và cập nhật lại kết quả.</summary>
        private void GhiDiemDangSua()
        {
            if (_itemDangSua?.Tag is not DongDiem dong || _cotDangSua < 0)
            {
                HuyDangSua();
                return;
            }

            string giaTri = _txtDiem.Text.Trim();

            double? diem = null;
            if (giaTri.Length > 0)
            {
                if (!double.TryParse(giaTri, NumberStyles.Float, CultureInfo.CurrentCulture, out double giaTriParse) &&
                    !double.TryParse(giaTri, NumberStyles.Float, CultureInfo.InvariantCulture, out giaTriParse))
                {
                    MessageBox.Show("Điểm không hợp lệ. Vui lòng nhập một số trong khoảng [0, 10].",
                        "Điểm không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    HuyDangSua();
                    return;
                }

                if (giaTriParse < 0 || giaTriParse > 10)
                {
                    MessageBox.Show("Điểm phải nằm trong khoảng [0, 10].",
                        "Điểm không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    HuyDangSua();
                    return;
                }

                diem = giaTriParse;
            }

            // Chưa đăng ký môn này thì tạo bản ghi đăng ký mới (chỉ trong bộ nhớ).
            if (dong.DangKy == null)
            {
                if (!diem.HasValue)
                {
                    // Không có điểm nào thì không tạo bản ghi đăng ký rỗng.
                    ListViewItem itemXoa = _itemDangSua;
                    int cotXoa = _cotDangSua;
                    HuyDangSua();

                    itemXoa.SubItems[cotXoa].Text = string.Empty;
                    CapNhatKetQuaDong(itemXoa, dong);
                    CapNhatTongKet();
                    return;
                }

                dong.DangKy = _diemService.LayOrTaoDangKy(dong.MSSV, dong.MaMon, dong.HocKy);
            }

            ListViewItem item = _itemDangSua;
            int cot = _cotDangSua;

            double? diemCu = cot switch
            {
                CotDiemQT => dong.DangKy.DiemQT,
                CotDiemGK => dong.DangKy.DiemGK,
                _ => dong.DangKy.DiemCK
            };

            switch (cot)
            {
                case CotDiemQT:
                    dong.DangKy.DiemQT = diem;
                    break;
                case CotDiemGK:
                    dong.DangKy.DiemGK = diem;
                    break;
                default:
                    dong.DangKy.DiemCK = diem;
                    break;
            }

            if (diemCu != diem)
            {
                _coThayDoiDiem = true;
            }

            HuyDangSua();

            item.SubItems[cot].Text = DinhDangDiem(diem);
            CapNhatKetQuaDong(item, dong);
            CapNhatTongKet();
        }

        /// <summary>Hủy việc sửa điểm, không lưu giá trị.</summary>
        private void HuyDangSua()
        {
            _itemDangSua = null;
            _cotDangSua = -1;
            _txtDiem.Text = string.Empty;
            _txtDiem.Visible = false;
        }

        /// <summary>
        /// ListView chỉ cho sửa nhãn ở cột đầu tiên (MSSV) nên không cho phép ở đây.
        /// Việc sửa các cột điểm đã được xử lý bằng ô nhập đặt trực tiếp trên dòng.
        /// </summary>
        private void lvDiem_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            e.CancelEdit = true;

            if (e.Item < 0 || e.Item >= lvDiem.Items.Count)
            {
                return;
            }

            if (lvDiem.Items[e.Item].Tag is DongDiem dong)
            {
                lvDiem.Items[e.Item].Text = dong.MSSV;
            }
        }

        #endregion

        #region Xuất CSV

        private void btnXuatCsv_Click(object sender, EventArgs e)
        {
            // Chọn nơi lưu và xuất bảng điểm lớp ra CSV.
            if (cboLop.SelectedItem is not Lop lop)
            {
                MessageBox.Show("Hãy chọn lớp.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboMon.SelectedItem is not MonHoc mon)
            {
                MessageBox.Show("Hãy chọn môn học.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hocKy = cboHocKy.SelectedItem?.ToString() ?? AppConfig.HocKyHienTai;
            string tenTepMacDinh = $"BangDiem_{FileHelpers.ChuanHoaTenTep(lop.MaLop)}_{FileHelpers.ChuanHoaTenTep(mon.MaMon)}_{hocKy}.csv";

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Xuất bảng điểm lớp ra CSV",
                Filter = "Tệp CSV|*.csv|Tất cả tệp|*.*",
                FileName = tenTepMacDinh,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                _diemService.XuatBangDiemCsv(lop.MaLop, mon.MaMon, hocKy, sfd.FileName);
                MessageBox.Show($"Đã xuất bảng điểm ra tệp:\n{sfd.FileName}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xuất CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Bảng điểm cá nhân

        /// <summary>Mở bảng điểm cá nhân (toàn bộ môn) của sinh viên đang chọn trên ListView.</summary>
        private void XemBangDiemCaNhan()
        {
            if (lvDiem.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn sinh viên trên danh sách.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvDiem.SelectedItems[0].Tag is not DongDiem dong)
            {
                return;
            }

            using Form form = new Form
            {
                Text = $"Bảng điểm cá nhân - {dong.MSSV} {dong.HoTen}",
                StartPosition = FormStartPosition.CenterParent,
                ClientSize = new Size(900, 460)
            };

            var lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false
            };

            lv.Columns.Add("Môn học", 260);
            lv.Columns.Add("Tín chỉ", 80);
            lv.Columns.Add("Học kỳ", 90);
            lv.Columns.Add("Điểm QT", 90);
            lv.Columns.Add("Điểm GK", 90);
            lv.Columns.Add("Điểm CK", 90);
            lv.Columns.Add("Tổng kết", 100);
            lv.Columns.Add("Xếp loại", 100);
            lv.Columns.Add("Ghi chú", 120);

            foreach (DangKy dangKy in _diemService.LayBangDiemCaNhan(dong.MSSV))
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

            var lblTtb = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0),
                Text = $"Điểm trung bình tích lũy: {_diemService.TinhDiemTrungBinhTichLuy(dong.MSSV).ToString("0.##", CultureInfo.InvariantCulture)}"
            };

            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 50 };

            var btnXuat = new Button
            {
                Text = "Xuất bảng điểm cá nhân ra CSV",
                Size = new Size(230, 32),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };

            btnXuat.Location = new Point(pnlBottom.Width - 240, 9);
            pnlBottom.Controls.Add(btnXuat);

            btnXuat.Click += (s, e) =>
            {
                using SaveFileDialog sfd = new SaveFileDialog
                {
                    Title = "Xuất bảng điểm cá nhân ra CSV",
                    Filter = "Tệp CSV|*.csv|Tất cả tệp|*.*",
                    FileName = $"BangDiemCaNhan_{FileHelpers.ChuanHoaTenTep(dong.MSSV)}.csv"
                };

                if (sfd.ShowDialog(form) != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    _diemService.XuatBangDiemCaNhanCsv(dong.MSSV, sfd.FileName);
                    MessageBox.Show($"Đã xuất bảng điểm cá nhân ra tệp:\n{sfd.FileName}", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi xuất CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            form.Controls.Add(lv);
            form.Controls.Add(lblTtb);
            form.Controls.Add(pnlBottom);
            form.ShowDialog(this);
        }

        #endregion

        #region Tiện ích

        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }

        #endregion

        /// <summary>Dữ liệu một dòng trên ListView (dùng làm Tag của ListViewItem).</summary>
        private sealed class DongDiem
        {
            public string MSSV { get; set; } = string.Empty;
            public string HoTen { get; set; } = string.Empty;
            public string MaMon { get; set; } = string.Empty;
            public string HocKy { get; set; } = string.Empty;
            public DangKy? DangKy { get; set; }

            public double? DiemQT => DangKy?.DiemQT;
            public double? DiemGK => DangKy?.DiemGK;
            public double? DiemCK => DangKy?.DiemCK;
        }
    }
}
