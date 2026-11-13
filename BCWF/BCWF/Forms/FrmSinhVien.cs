using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BCWF.Forms
{
    public partial class FrmSinhVien : Form
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly SinhVienService _sinhVienService = new SinhVienService();

        // Ba trạng thái của form: chỉ xem, đang thêm mới, đang sửa.
        private enum TrangThaiForm
        {
            Xem,
            Them,
            Sua
        }

        private TrangThaiForm _trangThai = TrangThaiForm.Xem;

        // Tên tệp ảnh đã lưu trong Data/Images.
        private string _anhHienTai = string.Empty;

        // Ảnh người dùng vừa chọn; chỉ sao chép vào Data/Images khi bấm Lưu.
        private string _duongDanAnhMoi = string.Empty;

        private string _mssvDangChon = string.Empty;

        // Sinh viên gốc không có ngày sinh: DateTimePicker hiển thị hôm nay nhưng
        // khi lưu không được tự đổi ngày trống thành ngày hôm nay.
        private bool _ngaySinhGocTrong;

        // Bản sao dữ liệu trước khi sửa, dùng để khôi phục khi Hủy.
        private SinhVien? _banGoc;

        // Chặn sự kiện chọn dòng khi form tự nạp lại danh sách.
        private bool _dangCapNhatGiaoDien;

        public FrmSinhVien()
        {
            InitializeComponent();

            KeyPreview = true;
            KeyDown += FrmSinhVien_KeyDown;
            FormClosing += FrmSinhVien_FormClosing;

            txtMSSV.CharacterCasing = CharacterCasing.Upper;
            txtMSSV.ImeMode = ImeMode.Disable;
            txtMSSV.KeyPress += txtMSSV_KeyPress;
            txtMSSV.TextChanged += txtMSSV_TextChanged;

            // Bộ gõ tiếng Việt (Unikey/VNI) gửi ký tự theo kiểu IME.
            // Trong lúc đang soạn thì không được sửa/kiểm tra nội dung ô nhập
            // (sửa Text lúc này sẽ huỷ vùng soạn và làm mất ký tự).
            txtMSSV.CompositionStarted += (s, e) => _dangComposition = true;
            txtMSSV.CompositionEnded += (s, e) =>
            {
                _dangComposition = false;
                ChuanHoaMSSV();
            };

            // Chốt trạng thái khi rời ô nhập (phòng trường hợp bộ gõ không gửi
            // thông báo kết thúc soạn).
            txtMSSV.Leave += (s, e) =>
            {
                _dangComposition = false;
                ChuanHoaMSSV();
            };

            txtHoTen.KeyPress += txtHoTen_KeyPress;
            txtHoTen.TextChanged += txtHoTen_TextChanged;
            txtHoTen.CompositionStarted += (s, e) => _dangCompositionHoTen = true;
            txtHoTen.CompositionEnded += (s, e) => _dangCompositionHoTen = false;
        }

        private void FrmSinhVien_Load(object sender, EventArgs e)
        {
            NapCay();
            NapDanhSachLop();
            XoaTrang();
            DatCheDoNhap(false);
            HienThiTheoNode(treeKhoaLop.SelectedNode);
        }

        //Nạp cây danh sách khoa và lớp
        private void NapCay()
        {
            #region Nạp cây danh sách khoa và lớp
            treeKhoaLop.BeginUpdate();
            treeKhoaLop.Nodes.Clear();

            foreach (Khoa khoa in _store.Khoas.OrderBy(k => k.TenKhoa))
            {
                TreeNode nodeKhoa = new TreeNode($"{khoa.MaKhoa} - {khoa.TenKhoa}")
                {
                    Tag = "KHOA:" + khoa.MaKhoa
                };

                foreach (Lop lop in _store.Lops.Where(l => l.MaKhoa == khoa.MaKhoa).OrderBy(l => l.MaLop))
                {
                    TreeNode nodeLop = new TreeNode($"{lop.MaLop} - {lop.TenLop}")
                    {
                        Tag = "LOP:" + lop.MaLop
                    };
                    nodeKhoa.Nodes.Add(nodeLop);
                }

                treeKhoaLop.Nodes.Add(nodeKhoa);
            }

            treeKhoaLop.EndUpdate();
            treeKhoaLop.ExpandAll();
            #endregion
        }

        //Nạp danh sách lớp vào ComboBox
        private void NapDanhSachLop()
        {
            #region Nạp danh sách lớp vào ComboBox
            cboLop.Items.Clear();
            foreach (Lop lop in _store.Lops.OrderBy(l => l.MaLop))
            {
                cboLop.Items.Add(lop);
            }

            if (cboLop.Items.Count > 0)
            {
                cboLop.SelectedIndex = 0;
            }
            #endregion
        }

        private void treeKhoaLop_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_trangThai != TrangThaiForm.Xem)
            {
                return;
            }

            HienThiTheoNode(e.Node);
        }

        //Hiển thị danh sách sinh viên theo node
        private void HienThiTheoNode(TreeNode? node)
        {
            #region Hiển thị danh sách sinh viên theo node
            string? tag = node?.Tag?.ToString();

            if (tag != null && tag.StartsWith("KHOA:", StringComparison.Ordinal))
            {
                HienThiDanhSach(_sinhVienService.LayTheoKhoa(tag.Substring(5)));
            }
            else if (tag != null && tag.StartsWith("LOP:", StringComparison.Ordinal))
            {
                HienThiDanhSach(_sinhVienService.LayTheoMaLop(tag.Substring(4)));
            }
            else
            {
                HienThiDanhSach(_store.SinhViens);
            }

            // Sinh viên đang xem không còn trong danh sách thì xóa trắng vùng nhập.
            if (_trangThai == TrangThaiForm.Xem && !ChonDongTheoMSSV(_mssvDangChon))
            {
                XoaTrang();
            }
            #endregion
        }

        //Hiển thị danh sách sinh viên
        private void HienThiDanhSach(IEnumerable<SinhVien> danhSach)
        {
            #region Hiển thị danh sách sinh viên
            _dangCapNhatGiaoDien = true;
            lvSinhVien.BeginUpdate();
            try
            {
                lvSinhVien.Items.Clear();

                foreach (SinhVien sv in danhSach)
                {
                    ListViewItem item = new ListViewItem(sv.MSSV);
                    item.SubItems.Add(sv.HoTen);
                    item.SubItems.Add(sv.NgaySinh == DateTime.MinValue ? string.Empty : sv.NgaySinh.ToString(SinhVien.DinhDangNgay));
                    item.SubItems.Add(sv.GioiTinh);
                    item.SubItems.Add(sv.MaLop);
                    item.SubItems.Add(sv.Anh);
                    item.Tag = sv.MSSV;
                    lvSinhVien.Items.Add(item);
                }
            }
            finally
            {
                lvSinhVien.EndUpdate();
                _dangCapNhatGiaoDien = false;
            }
            #endregion
        }

        //Hiển thị thông tin sinh viên khi chọn
        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Hiển thị thông tin sinh viên khi chọn
            if (_dangCapNhatGiaoDien || _trangThai != TrangThaiForm.Xem)
            {
                return;
            }

            if (lvSinhVien.SelectedItems.Count == 0)
            {
                return;
            }

            string? mssv = lvSinhVien.SelectedItems[0].Tag?.ToString();
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return;
            }

            SinhVien? sv = _store.SinhViens.FirstOrDefault(s =>
                string.Equals(s.MSSV, mssv, StringComparison.OrdinalIgnoreCase));

            if (sv == null)
            {
                return;
            }

            HienThiSinhVien(sv);
            #endregion
        }

        // Đổ dữ liệu một sinh viên lên vùng nhập (không đổi trạng thái form).
        private void HienThiSinhVien(SinhVien sv)
        {
            txtMSSV.Text = sv.MSSV;
            txtHoTen.Text = sv.HoTen;
            dtpNgaySinh.Value = sv.NgaySinh == DateTime.MinValue ? DateTime.Today : sv.NgaySinh;
            radNam.Checked = !string.Equals(sv.GioiTinh, "Nữ", StringComparison.OrdinalIgnoreCase);
            radNu.Checked = string.Equals(sv.GioiTinh, "Nữ", StringComparison.OrdinalIgnoreCase);

            for (int i = 0; i < cboLop.Items.Count; i++)
            {
                if (cboLop.Items[i] is Lop lop &&
                    string.Equals(lop.MaLop, sv.MaLop, StringComparison.OrdinalIgnoreCase))
                {
                    cboLop.SelectedIndex = i;
                    break;
                }
            }

            _mssvDangChon = sv.MSSV;
            _banGoc = SaoChep(sv);
            _ngaySinhGocTrong = sv.NgaySinh == DateTime.MinValue;
            _anhHienTai = sv.Anh;
            _duongDanAnhMoi = string.Empty;
            HienThiAnh(sv.Anh);
            DatCheDoNhap(false);
        }

        //Thêm sinh viên
        private void btnThem_Click(object sender, EventArgs e)
        {
            #region Thêm sinh viên
            if (_trangThai == TrangThaiForm.Them)
            {
                LuuThem();
                return;
            }

            if (_trangThai == TrangThaiForm.Sua)
            {
                return;
            }

            BatDauThem();
            #endregion
        }

        // Chuyển sang chế độ nhập sinh viên mới với biểu mẫu trắng.
        private void BatDauThem()
        {
            _dangCapNhatGiaoDien = true;
            try
            {
                lvSinhVien.SelectedItems.Clear();
            }
            finally
            {
                _dangCapNhatGiaoDien = false;
            }

            XoaTrang();
            BatDauChinhSua(TrangThaiForm.Them);
            txtMSSV.Focus();
        }

        //Sửa sinh viên
        private void btnSua_Click(object sender, EventArgs e)
        {
            #region Sửa sinh viên
            if (_trangThai == TrangThaiForm.Sua)
            {
                LuuSua();
                return;
            }

            if (_trangThai == TrangThaiForm.Them)
            {
                return;
            }

            SinhVien? sv = LaySinhVienDangChon();
            if (sv == null)
            {
                MessageBox.Show("Hãy chọn sinh viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Lưu bản sao tại thời điểm bắt đầu sửa để có thể khôi phục khi Hủy.
            _banGoc = SaoChep(sv);
            _mssvDangChon = sv.MSSV;
            _anhHienTai = sv.Anh;
            _duongDanAnhMoi = string.Empty;

            BatDauChinhSua(TrangThaiForm.Sua);
            txtHoTen.Focus();
            #endregion
        }

        // Bật chế độ nhập và khóa các thao tác có thể làm mất thay đổi.
        private void BatDauChinhSua(TrangThaiForm trangThai)
        {
            _trangThai = trangThai;
            DatCheDoNhap(true);

            treeKhoaLop.Enabled = false;
            lvSinhVien.Enabled = false;

            btnThem.Enabled = trangThai == TrangThaiForm.Them;
            btnThem.Text = trangThai == TrangThaiForm.Them ? "Lưu" : "Thêm";

            btnSua.Enabled = trangThai == TrangThaiForm.Sua;
            btnSua.Text = trangThai == TrangThaiForm.Sua ? "Lưu" : "Sửa";

            btnXoa.Enabled = false;
            btnLamMoi.Enabled = false;
            btnNhapCsv.Enabled = false;
            btnHuy.Enabled = true;
        }

        // Trở về chế độ chỉ xem và mở lại các nút bị khóa.
        private void KetThucChinhSua()
        {
            _trangThai = TrangThaiForm.Xem;
            DatCheDoNhap(false);

            treeKhoaLop.Enabled = true;
            lvSinhVien.Enabled = true;

            btnThem.Enabled = true;
            btnThem.Text = "Thêm";
            btnSua.Enabled = true;
            btnSua.Text = "Sửa";
            btnXoa.Enabled = true;
            btnLamMoi.Enabled = true;
            btnNhapCsv.Enabled = true;
            btnHuy.Enabled = false;
        }

        private void LuuThem()
        {
            if (!KiemTraDuLieu(out SinhVien? sv) || sv == null)
            {
                return;
            }

            if (_sinhVienService.KiemTraTrungMSSV(sv.MSSV))
            {
                MessageBox.Show("MSSV đã tồn tại.", "Trùng MSSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMSSV.Focus();
                return;
            }

            try
            {
                // Chỉ sao chép ảnh khi thực sự lưu, tránh tạo tệp ảnh rác nếu người dùng Hủy.
                if (!string.IsNullOrWhiteSpace(_duongDanAnhMoi))
                {
                    sv.Anh = _sinhVienService.LuuAnh(sv.MSSV, _duongDanAnhMoi);
                }

                _sinhVienService.Them(sv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _anhHienTai = sv.Anh;
            _duongDanAnhMoi = string.Empty;
            _banGoc = SaoChep(sv);

            KetThucChinhSua();
            HienThiTheoNode(treeKhoaLop.SelectedNode);

            if (!ChonDongTheoMSSV(sv.MSSV))
            {
                HienThiSinhVien(sv);
            }

            MessageBox.Show("Thêm sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LuuSua()
        {
            if (_banGoc == null || string.IsNullOrWhiteSpace(_mssvDangChon))
            {
                MessageBox.Show("Không xác định được sinh viên đang sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!KiemTraDuLieu(out SinhVien? sv) || sv == null)
            {
                return;
            }

            if (GiongBanGoc(sv))
            {
                KetThucChinhSua();
                MessageBox.Show("Không có thay đổi nào cần lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool doiMSSV = !string.Equals(sv.MSSV, _mssvDangChon, StringComparison.OrdinalIgnoreCase);

            if (doiMSSV && _sinhVienService.KiemTraTrungMSSV(sv.MSSV))
            {
                MessageBox.Show("MSSV đã tồn tại.", "Trùng MSSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMSSV.Focus();
                return;
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(_duongDanAnhMoi))
                {
                    sv.Anh = _sinhVienService.LuuAnh(sv.MSSV, _duongDanAnhMoi);
                }

                if (doiMSSV)
                {
                    _sinhVienService.DoiMSSV(_mssvDangChon, sv);
                }
                else
                {
                    _sinhVienService.CapNhat(sv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _anhHienTai = sv.Anh;
            _duongDanAnhMoi = string.Empty;
            _banGoc = SaoChep(sv);

            KetThucChinhSua();
            HienThiTheoNode(treeKhoaLop.SelectedNode);

            if (!ChonDongTheoMSSV(sv.MSSV))
            {
                HienThiSinhVien(sv);
            }

            MessageBox.Show("Cập nhật sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //Hủy việc thêm/sửa
        private void btnHuy_Click(object? sender, EventArgs e)
        {
            if (_trangThai == TrangThaiForm.Xem)
            {
                return;
            }

            if (CoThayDoiChuaLuu())
            {
                string cauHoi = _trangThai == TrangThaiForm.Them
                    ? "Bỏ sinh viên đang nhập?"
                    : "Bỏ các thay đổi của sinh viên này?";

                DialogResult xacNhan = MessageBox.Show(
                    cauHoi,
                    "Xác nhận hủy",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (xacNhan != DialogResult.Yes)
                {
                    return;
                }
            }

            HuyChinhSua();
        }

        // Khôi phục dữ liệu trước khi sửa (hoặc xóa trắng nếu đang thêm).
        private void HuyChinhSua()
        {
            TrangThaiForm trangThaiTruoc = _trangThai;
            _trangThai = TrangThaiForm.Xem;

            if (trangThaiTruoc == TrangThaiForm.Sua && _banGoc != null)
            {
                SinhVien banGoc = SaoChep(_banGoc);
                HienThiSinhVien(banGoc);
                ChonDongTheoMSSV(banGoc.MSSV);
            }
            else
            {
                XoaTrang();
            }

            KetThucChinhSua();
        }

        private void FrmSinhVien_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape && _trangThai != TrangThaiForm.Xem)
            {
                e.SuppressKeyPress = true;
                btnHuy_Click(sender, EventArgs.Empty);
            }
        }

        private void FrmSinhVien_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_trangThai == TrangThaiForm.Xem || !CoThayDoiChuaLuu())
            {
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                "Bạn có thay đổi chưa lưu. Thoát và bỏ các thay đổi?",
                "Xác nhận thoát",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (xacNhan != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        //Xóa sinh viên
        private void btnXoa_Click(object sender, EventArgs e)
        {
            #region Xóa sinh viên
            if (_trangThai != TrangThaiForm.Xem)
            {
                return;
            }

            if (lvSinhVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn sinh viên cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string mssv = lvSinhVien.SelectedItems[0].Tag?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return;
            }

            DialogResult xacNhan = MessageBox.Show(
                $"Xóa sinh viên {mssv} và các đăng ký liên quan?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button2);

            if (xacNhan != DialogResult.Yes)
            {
                return;
            }

            try
            {
                _sinhVienService.Xoa(mssv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            XoaTrang();
            HienThiTheoNode(treeKhoaLop.SelectedNode);
            MessageBox.Show("Đã xóa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            #endregion
        }

        //Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            if (_trangThai != TrangThaiForm.Xem)
            {
                return;
            }

            XoaTrang();
        }

        //Xóa trắng vùng nhập
        private void XoaTrang()
        {
            #region Xóa trắng
            _dangCapNhatGiaoDien = true;
            try
            {
                if (lvSinhVien.SelectedItems.Count > 0)
                {
                    lvSinhVien.SelectedItems.Clear();
                }
            }
            finally
            {
                _dangCapNhatGiaoDien = false;
            }

            _mssvDangChon = string.Empty;
            _banGoc = null;
            _ngaySinhGocTrong = false;
            _anhHienTai = string.Empty;
            _duongDanAnhMoi = string.Empty;

            txtMSSV.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            radNam.Checked = true;
            radNu.Checked = false;

            cboLop.SelectedIndex = cboLop.Items.Count > 0 ? 0 : -1;

            HienThiAnh(string.Empty);
            #endregion
        }

        //Chọn ảnh
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            #region Chọn ảnh
            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Chọn ảnh thẻ sinh viên",
                Filter = "Tệp ảnh|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tất cả tệp|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            // Chỉ xem trước; tệp ảnh thật sự được sao chép khi bấm Lưu.
            _duongDanAnhMoi = ofd.FileName;
            HienThiAnhTuDuongDan(ofd.FileName);
            #endregion
        }

        //Nhập CSV
        private void btnNhapCsv_Click(object sender, EventArgs e)
        {
            #region Nhập CSV
            if (_trangThai != TrangThaiForm.Xem)
            {
                return;
            }

            using OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Chọn tệp CSV",
                Filter = "Tệp CSV|*.csv|Tất cả tệp|*.*"
            };

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                int so = _sinhVienService.NhapTuCsv(ofd.FileName);
                MessageBox.Show($"Đã nhập {so} sinh viên.", "Nhập CSV", MessageBoxButtons.OK, MessageBoxIcon.Information);
                XoaTrang();
                HienThiTheoNode(treeKhoaLop.SelectedNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }

        /// <summary>
        /// Lọc ký tự khi gõ MSSV. Không kiểm tra vị trí/chữ hoa tại đây vì khi dùng
        /// bộ gõ tiếng Việt, nội dung ô nhập trong lúc soạn chưa ổn định,
        /// kiểm tra theo vị trí sẽ loại mất ký tự hợp lệ.
        /// Định dạng được kiểm tra lại khi bấm Lưu.
        /// </summary>
        private void txtMSSV_KeyPress(object? sender, KeyPressEventArgs e)
        {
            // Cho phép các phím điều khiển: Backspace, Delete, Enter, mũi tên...
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Đang soạn với bộ gõ tiếng Việt: để bộ gõ quyết định ký tự.
            if (_dangComposition)
            {
                return;
            }

            char c = char.ToUpperInvariant(e.KeyChar);
            if (c == 'S' || c == 'V' || (c >= '0' && c <= '9'))
            {
                return;
            }

            // Chặn mọi ký tự khác (kể cả chữ có dấu) nhưng không hiện thông báo
            // để không gián đoạn khi người dùng đang gõ.
            e.Handled = true;
        }

        private bool _dangChuanHoaMSSV;

        /// <summary>Đang soạn văn bản bằng bộ gõ tiếng Việt hay không.</summary>
        private bool _dangComposition;

        /// <summary>Đang soạn tên sinh viên bằng bộ gõ tiếng Việt hay không.</summary>
        private bool _dangCompositionHoTen;

        private void txtMSSV_TextChanged(object? sender, EventArgs e)
        {
            if (_dangChuanHoaMSSV || _dangComposition)
            {
                return;
            }

            ChuanHoaMSSV();
        }

        /// <summary>
        /// Chuẩn hoá nội dung ô MSSV: chỉ giữ lại chữ S, V và chữ số, viết hoa,
        /// và tự hoàn thiện tiền tố "SV" khi đã có phần chữ số
        /// (người dùng có thể gõ "30010" hoặc "S3" và vẫn ra "SV30010"/"SV3").
        /// </summary>
        private void ChuanHoaMSSV()
        {
            string goc = txtMSSV.Text;

            var chu = new StringBuilder();
            var so = new StringBuilder();

            foreach (char ch in goc)
            {
                if (ch >= '0' && ch <= '9')
                {
                    so.Append(ch);
                    continue;
                }

                if (ch is 'S' or 's' or 'V' or 'v')
                {
                    chu.Append(char.ToUpperInvariant(ch));
                }
            }

            string sach = chu.ToString() + so.ToString();

            // Đã có chữ số thì luôn dùng tiền tố chuẩn "SV".
            if (so.Length > 0)
            {
                sach = "SV" + so.ToString();
            }

            if (string.Equals(sach, goc, StringComparison.Ordinal))
            {
                return;
            }

            _dangChuanHoaMSSV = true;
            try
            {
                txtMSSV.Text = sach;
                txtMSSV.SelectionStart = sach.Length;
            }
            finally
            {
                _dangChuanHoaMSSV = false;
            }
        }

        private void txtHoTen_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // Đang soạn với bộ gõ tiếng Việt: để bộ gõ quyết định ký tự.
            if (_dangCompositionHoTen)
            {
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (!(char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == '\'' || e.KeyChar == '-'))
            {
                e.Handled = true;
            }
        }

        private void txtHoTen_TextChanged(object? sender, EventArgs e)
        {
            // Đang soạn với bộ gõ tiếng Việt: không sửa lại nội dung để tránh huỷ vùng soạn.
            if (_dangCompositionHoTen)
            {
                return;
            }

            string goc = txtHoTen.Text;
            string sach = new string(goc.Where(ch => !char.IsDigit(ch)).ToArray());
            if (!string.Equals(sach, goc, StringComparison.Ordinal))
            {
                int viTri = txtHoTen.SelectionStart;
                txtHoTen.Text = sach;
                txtHoTen.SelectionStart = Math.Min(viTri, sach.Length);
            }
        }

        private void DatCheDoNhap(bool choPhep)
        {
            txtMSSV.ReadOnly = !choPhep;
            txtHoTen.ReadOnly = !choPhep;
            dtpNgaySinh.Enabled = choPhep;
            radNam.Enabled = choPhep;
            radNu.Enabled = choPhep;
            cboLop.Enabled = choPhep;
            btnChonAnh.Enabled = choPhep;
        }

        // So sánh dữ liệu trên form với bản gốc trước khi sửa.
        private bool GiongBanGoc(SinhVien sv)
        {
            if (_banGoc == null)
            {
                return false;
            }

            // Có ảnh mới chờ lưu thì chắc chắn đã có thay đổi.
            if (!string.IsNullOrWhiteSpace(_duongDanAnhMoi))
            {
                return false;
            }

            return string.Equals(sv.MSSV, _banGoc.MSSV, StringComparison.OrdinalIgnoreCase)
                && string.Equals(sv.HoTen, _banGoc.HoTen, StringComparison.OrdinalIgnoreCase)
                && CungNgaySinh(dtpNgaySinh.Value)
                && string.Equals(sv.GioiTinh, _banGoc.GioiTinh, StringComparison.OrdinalIgnoreCase)
                && string.Equals(sv.MaLop, _banGoc.MaLop, StringComparison.OrdinalIgnoreCase)
                && string.Equals(sv.Anh, _banGoc.Anh, StringComparison.OrdinalIgnoreCase);
        }

        // Có thay đổi chưa lưu hay không (dùng cho Hủy/đóng form).
        private bool CoThayDoiChuaLuu()
        {
            if (_trangThai == TrangThaiForm.Them)
            {
                return CoDuLieuNhap();
            }

            if (_trangThai != TrangThaiForm.Sua || _banGoc == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(_duongDanAnhMoi))
            {
                return true;
            }

            string gioiTinh = radNu.Checked ? "Nữ" : "Nam";
            string maLop = cboLop.SelectedItem is Lop lop ? lop.MaLop : string.Empty;

            return !string.Equals(txtMSSV.Text.Trim(), _banGoc.MSSV, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(txtHoTen.Text.Trim(), _banGoc.HoTen, StringComparison.OrdinalIgnoreCase)
                || !CungNgaySinh(dtpNgaySinh.Value)
                || !string.Equals(gioiTinh, _banGoc.GioiTinh, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(maLop, _banGoc.MaLop, StringComparison.OrdinalIgnoreCase)
                || !string.Equals(_anhHienTai, _banGoc.Anh, StringComparison.OrdinalIgnoreCase);
        }

        // So sánh ngày sinh, có tính trường hợp dữ liệu gốc để trống ngày.
        private bool CungNgaySinh(DateTime ngayTrenForm)
        {
            if (_banGoc == null)
            {
                return false;
            }

            if (_banGoc.NgaySinh == DateTime.MinValue)
            {
                return ngayTrenForm.Date == DateTime.Today;
            }

            return ngayTrenForm.Date == _banGoc.NgaySinh.Date;
        }

        private bool CoDuLieuNhap()
        {
            return !string.IsNullOrWhiteSpace(txtMSSV.Text)
                || !string.IsNullOrWhiteSpace(txtHoTen.Text)
                || !string.IsNullOrWhiteSpace(_duongDanAnhMoi)
                || dtpNgaySinh.Value.Date != DateTime.Today
                || radNu.Checked
                || cboLop.SelectedIndex > 0;
        }

        private SinhVien? LaySinhVienDangChon()
        {
            if (lvSinhVien.SelectedItems.Count == 0)
            {
                return null;
            }

            string? mssv = lvSinhVien.SelectedItems[0].Tag?.ToString();
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return null;
            }

            return _store.SinhViens.FirstOrDefault(s =>
                string.Equals(s.MSSV, mssv, StringComparison.OrdinalIgnoreCase));
        }

        // Chọn dòng có MSSV tương ứng; trả về false nếu không có trong danh sách hiện tại.
        private bool ChonDongTheoMSSV(string mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return false;
            }

            foreach (ListViewItem item in lvSinhVien.Items)
            {
                if (string.Equals(item.Tag?.ToString(), mssv, StringComparison.OrdinalIgnoreCase))
                {
                    item.Selected = true;
                    item.Focused = true;
                    lvSinhVien.EnsureVisible(item.Index);
                    return true;
                }
            }

            return false;
        }

        private static SinhVien SaoChep(SinhVien sv)
        {
            return new SinhVien
            {
                MSSV = sv.MSSV,
                HoTen = sv.HoTen,
                NgaySinh = sv.NgaySinh,
                GioiTinh = sv.GioiTinh,
                MaLop = sv.MaLop,
                Anh = sv.Anh
            };
        }

        private bool KiemTraDuLieu(out SinhVien? sinhVien)
        {
            #region Kiểm tra dữ liệu
            sinhVien = null;

            string mssv = txtMSSV.Text.Trim().ToUpperInvariant();
            string hoTen = txtHoTen.Text.Trim();

            if (string.IsNullOrWhiteSpace(mssv))
            {
                MessageBox.Show("MSSV không được để trống.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMSSV.Focus();
                return false;
            }

            if (!SinhVien.IsValidMSSV(mssv))
            {
                MessageBox.Show("MSSV phải có dạng SV theo sau là số (ví dụ SV01, SV001, SV10001), không dấu.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMSSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Họ tên không được để trống.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (hoTen.Any(char.IsDigit))
            {
                MessageBox.Show("Họ tên không được chứa số.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (!hoTen.Any(char.IsLetter))
            {
                MessageBox.Show("Họ tên phải chứa chữ cái.", "Sai định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (!radNam.Checked && !radNu.Checked)
            {
                MessageBox.Show("Hãy chọn giới tính.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cboLop.SelectedItem is not Lop lop)
            {
                MessageBox.Show("Hãy chọn lớp.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLop.Focus();
                return false;
            }

            // Giữ nguyên ngày sinh trống nếu người dùng không thực sự đổi ngày.
            DateTime ngaySinh = _ngaySinhGocTrong && dtpNgaySinh.Value.Date == DateTime.Today
                ? DateTime.MinValue
                : dtpNgaySinh.Value.Date;

            sinhVien = new SinhVien
            {
                MSSV = mssv,
                HoTen = hoTen,
                NgaySinh = ngaySinh,
                GioiTinh = radNu.Checked ? "Nữ" : "Nam",
                MaLop = lop.MaLop,
                Anh = _anhHienTai
            };

            return true;
            #endregion
        }

        //Hiển thị ảnh theo tên tệp đã lưu
        private void HienThiAnh(string tenAnh)
        {
            HienThiAnhTuDuongDan(string.IsNullOrWhiteSpace(tenAnh) ? string.Empty : _store.LayDuongDanAnh(tenAnh));
        }

        //Hiển thị ảnh từ một đường dẫn bất kỳ (dùng để xem trước ảnh mới chọn)
        private void HienThiAnhTuDuongDan(string duongDan)
        {
            #region Hiển thị ảnh
            if (picAnh.Image != null)
            {
                Image anhCu = picAnh.Image;
                picAnh.Image = null;
                anhCu.Dispose();
            }

            if (string.IsNullOrWhiteSpace(duongDan) || !File.Exists(duongDan))
            {
                return;
            }

            try
            {
                using FileStream fs = new FileStream(duongDan, FileMode.Open, FileAccess.Read);
                using Image anhGoc = Image.FromStream(fs);
                picAnh.Image = new Bitmap(anhGoc);
            }
            catch
            {
                // Bỏ qua nếu tệp ảnh hỏng.
            }
            #endregion
        }
    }
}
