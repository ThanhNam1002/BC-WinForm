using BCWF.Services;
using System;
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
        private readonly DataStore _store = DataStore.Instance;
        private readonly DangKyService _dangKyService = new DangKyService();

        public FrmDangKy()
        {
            InitializeComponent();
        }

        private void FrmDangKy_Load(object sender, EventArgs e)
        {
            // TODO (SV2): nạp danh sách môn học, lớp, sinh viên; chọn học kỳ mặc định từ App.config.
        }

        #region Quản lý môn học

        private void lvMonHoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO (SV2): đưa môn đang chọn lên vùng nhập liệu.
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            // TODO (SV2): thêm môn học (chặn trùng mã môn) và ghi monhoc.txt.
        }

        private void btnSuaMon_Click(object sender, EventArgs e)
        {
            // TODO (SV2): cập nhật môn học đang chọn.
        }

        private void btnXoaMon_Click(object sender, EventArgs e)
        {
            // TODO (SV2): xóa môn học, kiểm tra ràng buộc với dangky.txt.
        }

        #endregion

        #region Đăng ký học phần

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO (SV2): lọc danh sách sinh viên theo lớp đang chọn.
        }

        private void cboSinhVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO (SV2): nạp môn đã đăng ký / chưa đăng ký của sinh viên trong học kỳ.
        }

        private void cboHocKy_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO (SV2): nạp lại danh sách môn theo học kỳ.
        }

        private void btnChonMon_Click(object sender, EventArgs e)
        {
            // TODO (SV2): chuyển môn đang chọn từ "chưa đăng ký" sang "đã chọn",
            //             kiểm tra trùng môn, môn tiên quyết và trùng lịch.
        }

        private void btnBoMon_Click(object sender, EventArgs e)
        {
            // TODO (SV2): bỏ môn khỏi danh sách "đã chọn".
        }

        private void lstChuaDangKy_DoubleClick(object sender, EventArgs e)
        {
            // TODO (SV2): nhấp đúp để chuyển môn sang danh sách đã chọn.
        }

        private void lstDaDangKy_DoubleClick(object sender, EventArgs e)
        {
            // TODO (SV2): nhấp đúp để bỏ môn khỏi danh sách đã chọn.
        }

        private void btnLuuDangKy_Click(object sender, EventArgs e)
        {
            // TODO (SV2): kiểm tra tổng tín chỉ trong [MinTC, MaxTC] rồi ghi dangky.txt.
        }

        private void btnLamMoiDangKy_Click(object sender, EventArgs e)
        {
            // TODO (SV2): nạp lại danh sách môn cho sinh viên đang chọn.
        }

        #endregion
    }
}
