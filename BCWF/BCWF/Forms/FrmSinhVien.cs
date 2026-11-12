using BCWF.Models;
using BCWF.Services;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    public partial class FrmSinhVien : Form
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly SinhVienService _sinhVienService = new SinhVienService();

        private string _anhHienTai = string.Empty;

        public FrmSinhVien()
        {
            InitializeComponent();
        }

        private void FrmSinhVien_Load(object sender, EventArgs e)
        {
            NapCay();
            NapDanhSachLop();
            XoaTrang();
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
            HienThiTheoNode(e.Node);
        }

        //Hiển thị danh sách sinh viên theo node
        private void HienThiTheoNode(TreeNode? node)
        {
            #region Hiển thị danh sách sinh viên theo node
            string? tag = node?.Tag?.ToString();

            if (tag != null && tag.StartsWith("KHOA:", StringComparison.Ordinal))
            {
                string maKhoa = tag.Substring(5);
                HienThiDanhSach(_sinhVienService.LayTheoKhoa(maKhoa));
            }
            else if (tag != null && tag.StartsWith("LOP:", StringComparison.Ordinal))
            {
                string maLop = tag.Substring(4);
                HienThiDanhSach(_sinhVienService.LayTheoMaLop(maLop));
            }
            else
            {
                HienThiDanhSach(_store.SinhViens);
            }
            #endregion
        }

        //Hiển thị danh sách sinh viên
        private void HienThiDanhSach(IEnumerable<SinhVien> danhSach)
        {
            #region Hiển thị danh sách sinh viên
            lvSinhVien.BeginUpdate();
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

            lvSinhVien.EndUpdate();
            #endregion
        }

        //Hiển thị thông tin sinh viên khi chọn
        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Hiển thị thông tin sinh viên khi chọn
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

            txtMSSV.Text = sv.MSSV;
            txtMSSV.ReadOnly = true;
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

            _anhHienTai = sv.Anh;
            HienThiAnh(sv.Anh);
            #endregion
        }

        //Thêm sinh viên
        private void btnThem_Click(object sender, EventArgs e)
        {
            #region Thêm sinh viên
            if (!KiemTraDuLieu(out SinhVien? sv) || sv == null)
            {
                return;
            }

            try
            {
                if (_sinhVienService.KiemTraTrungMSSV(sv.MSSV))
                {
                    MessageBox.Show("MSSV đã tồn tại.", "Trùng MSSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _sinhVienService.Them(sv);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Thêm sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            XoaTrang();
            HienThiTheoNode(treeKhoaLop.SelectedNode);
            #endregion
        }

        //Sửa sinh viên
        private void btnSua_Click(object sender, EventArgs e)
        {
            #region Sửa sinh viên
            if (lvSinhVien.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn sinh viên cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string mssvDangChon = lvSinhVien.SelectedItems[0].Tag?.ToString() ?? string.Empty;

            if (!KiemTraDuLieu(out SinhVien? sv) || sv == null)
            {
                return;
            }

            try
            {
                if (!string.Equals(sv.MSSV, mssvDangChon, StringComparison.OrdinalIgnoreCase) &&
                    _sinhVienService.KiemTraTrungMSSV(sv.MSSV))
                {
                    MessageBox.Show("MSSV đã tồn tại.", "Trùng MSSV", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.Equals(sv.MSSV, mssvDangChon, StringComparison.OrdinalIgnoreCase))
                {
                    _sinhVienService.CapNhat(sv);
                }
                else
                {
                    _sinhVienService.Them(sv);

                    foreach (DangKy dk in _store.DangKys.Where(d =>
                        string.Equals(d.MSSV, mssvDangChon, StringComparison.OrdinalIgnoreCase)).ToList())
                    {
                        dk.MSSV = sv.MSSV;
                    }

                    _store.LuuDangKy();
                    _sinhVienService.Xoa(mssvDangChon);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Cập nhật sinh viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            XoaTrang();
            HienThiTheoNode(treeKhoaLop.SelectedNode);
            #endregion
        }

        //Xóa sinh viên
        private void btnXoa_Click(object sender, EventArgs e)
        {
            #region Xóa sinh viên
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
                MessageBoxIcon.Question);

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

            MessageBox.Show("Đã xóa sinh viên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            XoaTrang();
            HienThiTheoNode(treeKhoaLop.SelectedNode);
            #endregion
        }

        //Làm mới
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            XoaTrang();
        }

        //Xóa trắng
        private void XoaTrang()
        {
            #region Xóa trắng
            txtMSSV.ReadOnly = false;
            txtMSSV.Clear();
            txtHoTen.Clear();
            dtpNgaySinh.Value = DateTime.Today;
            radNam.Checked = true;
            radNu.Checked = false;

            if (cboLop.Items.Count > 0)
            {
                cboLop.SelectedIndex = 0;
            }
            else
            {
                cboLop.SelectedIndex = -1;
            }

            _anhHienTai = string.Empty;
            HienThiAnh(string.Empty);

            if (lvSinhVien.SelectedItems.Count > 0)
            {
                lvSinhVien.SelectedItems.Clear();
            }
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

            try
            {
                string tenAnh = _sinhVienService.LuuAnh(txtMSSV.Text.Trim(), ofd.FileName);
                _anhHienTai = tenAnh;
                HienThiAnh(tenAnh);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi chọn ảnh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }

        //Nhập CSV
        private void btnNhapCsv_Click(object sender, EventArgs e)
        {
            #region Nhập CSV
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
                HienThiTheoNode(treeKhoaLop.SelectedNode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi nhập CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            #endregion
        }

        private bool KiemTraDuLieu(out SinhVien? sinhVien)
        {
            #region Kiểm tra dữ liệu
            sinhVien = null;

            string mssv = txtMSSV.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();

            if (string.IsNullOrWhiteSpace(mssv))
            {
                MessageBox.Show("MSSV không được để trống.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMSSV.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(hoTen))
            {
                MessageBox.Show("Họ tên không được để trống.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }

            if (cboLop.SelectedItem is not Lop lop)
            {
                MessageBox.Show("Hãy chọn lớp.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLop.Focus();
                return false;
            }

            sinhVien = new SinhVien
            {
                MSSV = mssv,
                HoTen = hoTen,
                NgaySinh = dtpNgaySinh.Value.Date,
                GioiTinh = radNu.Checked ? "Nữ" : "Nam",
                MaLop = lop.MaLop,
                Anh = _anhHienTai
            };

            return true;
            #endregion
        }

        //Hiển thị ảnh
        private void HienThiAnh(string tenAnh)
        {
            #region Hiển thị ảnh
            if (picAnh.Image != null)
            {
                Image anhCu = picAnh.Image;
                picAnh.Image = null;
                anhCu.Dispose();
            }

            if (string.IsNullOrWhiteSpace(tenAnh))
            {
                return;
            }

            string duongDan = _store.LayDuongDanAnh(tenAnh);
            if (!File.Exists(duongDan))
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