using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// SV1 - Sinh viên &amp; lớp.
    /// Cây Khoa -> Lớp (TreeView); chọn lớp hiển thị sinh viên trên ListView;
    /// thêm/sửa/xóa sinh viên, chặn trùng MSSV, hiển thị ảnh thẻ và nhập từ CSV.
    /// </summary>
    public partial class FrmSinhVien : Form, ILamMoiKhiHienThi
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly SinhVienService _sinhVienService = new SinhVienService();

        public FrmSinhVien()
        {
            InitializeComponent();
        }

        private void FrmSinhVien_Load(object sender, EventArgs e)
        {
            LamMoiKhiHienThi();
        }

        /// <summary>Nạp lại cây Khoa - Lớp và danh sách sinh viên đang hiển thị.</summary>
        public void LamMoiKhiHienThi()
        {
            string? tagDangChon = treeKhoaLop.SelectedNode?.Tag as string;
            string? maLopDangChon = cboLop.SelectedItem is Lop lopDangChon ? lopDangChon.MaLop : null;

            treeKhoaLop.BeginUpdate();
            treeKhoaLop.Nodes.Clear();

            foreach (Khoa khoa in _store.Khoas.OrderBy(k => k.MaKhoa))
            {
                TreeNode nodeKhoa = new TreeNode($"{khoa.MaKhoa} - {khoa.TenKhoa}")
                {
                    Tag = $"K:{khoa.MaKhoa}"
                };

                foreach (Lop lop in _store.Lops
                    .Where(l => string.Equals(l.MaKhoa, khoa.MaKhoa, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(l => l.MaLop))
                {
                    nodeKhoa.Nodes.Add(new TreeNode($"{lop.MaLop} - {lop.TenLop}")
                    {
                        Tag = $"L:{lop.MaLop}"
                    });
                }

                treeKhoaLop.Nodes.Add(nodeKhoa);
            }

            treeKhoaLop.ExpandAll();
            treeKhoaLop.EndUpdate();

            TreeNode? nodeChon = null;
            if (tagDangChon != null)
            {
                nodeChon = TimNodeTheoTag(treeKhoaLop.Nodes, tagDangChon);
            }
            if (nodeChon == null && treeKhoaLop.Nodes.Count > 0)
            {
                nodeChon = treeKhoaLop.Nodes[0];
            }

            treeKhoaLop.SelectedNode = nodeChon;
            CapNhatDanhSachTheoNode(nodeChon);

            cboLop.BeginUpdate();
            cboLop.Items.Clear();
            foreach (Lop lop in _store.Lops.OrderBy(l => l.MaLop))
            {
                cboLop.Items.Add(lop);
            }
            cboLop.EndUpdate();

            if (maLopDangChon != null)
            {
                Lop? lopCanChon = cboLop.Items.Cast<Lop>()
                    .FirstOrDefault(l => string.Equals(l.MaLop, maLopDangChon, StringComparison.OrdinalIgnoreCase));
                if (lopCanChon != null)
                {
                    cboLop.SelectedItem = lopCanChon;
                }
            }
        }

        private static TreeNode? TimNodeTheoTag(TreeNodeCollection cacNode, string tag)
        {
            foreach (TreeNode node in cacNode)
            {
                if (string.Equals(node.Tag as string, tag, StringComparison.Ordinal))
                {
                    return node;
                }

                TreeNode? ketQua = TimNodeTheoTag(node.Nodes, tag);
                if (ketQua != null)
                {
                    return ketQua;
                }
            }

            return null;
        }

        private void CapNhatDanhSachTheoNode(TreeNode? node)
        {
            if (node?.Tag is not string tag)
            {
                HienThiDanhSach(Array.Empty<SinhVien>());
                return;
            }

            List<SinhVien> danhSach;
            if (tag.StartsWith("K:", StringComparison.Ordinal))
            {
                danhSach = _sinhVienService.LayTheoKhoa(tag.Substring(2));
            }
            else if (tag.StartsWith("L:", StringComparison.Ordinal))
            {
                danhSach = _sinhVienService.LayTheoMaLop(tag.Substring(2));
            }
            else
            {
                danhSach = new List<SinhVien>();
            }

            HienThiDanhSach(danhSach);
        }

        private void HienThiDanhSach(IEnumerable<SinhVien> danhSach)
        {
            lvSinhVien.BeginUpdate();
            lvSinhVien.Items.Clear();

            foreach (SinhVien sinhVien in danhSach)
            {
                ListViewItem item = new ListViewItem(sinhVien.MSSV);
                item.SubItems.Add(sinhVien.HoTen);
                item.SubItems.Add(sinhVien.NgaySinh == DateTime.MinValue
                    ? string.Empty
                    : sinhVien.NgaySinh.ToString(SinhVien.DinhDangNgay));
                item.SubItems.Add(sinhVien.GioiTinh);
                item.SubItems.Add(sinhVien.MaLop);
                item.SubItems.Add(string.IsNullOrWhiteSpace(sinhVien.Anh) ? "Chưa có" : sinhVien.Anh);
                item.Tag = sinhVien;
                lvSinhVien.Items.Add(item);
            }

            lvSinhVien.EndUpdate();
        }

        private void treeKhoaLop_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CapNhatDanhSachTheoNode(e.Node);
        }

        private void lvSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO (SV1): đưa thông tin sinh viên đang chọn lên vùng nhập liệu và picAnh.
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // TODO (SV1): kiểm tra dữ liệu, chặn trùng MSSV, thêm mới và ghi sinhvien.txt.
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            // TODO (SV1): cập nhật sinh viên đang chọn.
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // TODO (SV1): xác nhận và xóa sinh viên đang chọn.
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // TODO (SV1): xóa trắng vùng nhập liệu để nhập sinh viên mới.
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            // TODO (SV1): chọn tệp ảnh (OpenFileDialog), sao chép vào Data/Images và hiển thị lên picAnh.
        }

        private void btnNhapCsv_Click(object sender, EventArgs e)
        {
            // TODO (SV1): chọn tệp CSV và gọi _sinhVienService.NhapTuCsv(...).
        }
    }
}
