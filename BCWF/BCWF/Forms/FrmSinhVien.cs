using BCWF.Services;
using System;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// SV1 - Sinh viên &amp; lớp.
    /// Cây Khoa -> Lớp (TreeView); chọn lớp hiển thị sinh viên trên ListView;
    /// thêm/sửa/xóa sinh viên, chặn trùng MSSV, hiển thị ảnh thẻ và nhập từ CSV.
    /// </summary>
    public partial class FrmSinhVien : Form
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly SinhVienService _sinhVienService = new SinhVienService();

        public FrmSinhVien()
        {
            InitializeComponent();
        }

        private void FrmSinhVien_Load(object sender, EventArgs e)
        {
            // TODO (SV1): nạp cây Khoa -> Lớp từ _store.Khoas/_store.Lops,
            //             đổ danh sách lớp vào cboLop và xóa trắng vùng nhập liệu.
        }

        private void treeKhoaLop_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // TODO (SV1): nút Khoa -> hiển thị toàn bộ sinh viên của khoa;
            //             nút Lớp  -> hiển thị sinh viên của lớp trên lvSinhVien.
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
