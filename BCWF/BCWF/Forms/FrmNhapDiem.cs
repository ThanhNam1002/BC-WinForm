using BCWF.Services;
using System;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// SV3 - Điểm.
    /// Nhập điểm theo lớp và môn, sửa trực tiếp trên ListView, tính điểm tổng kết
    /// (30% QT + 30% GK + 40% CK), xếp loại, tô đỏ môn rớt, điểm trung bình tích lũy
    /// và xuất bảng điểm của lớp ra CSV.
    /// </summary>
    public partial class FrmNhapDiem : Form, ILamMoiKhiHienThi
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        public FrmNhapDiem()
        {
            InitializeComponent();
        }

        private void FrmNhapDiem_Load(object sender, EventArgs e)
        {
            LamMoiKhiHienThi();
        }

        /// <summary>Nạp lại lớp, môn học và học kỳ khi form được hiển thị.</summary>
        public void LamMoiKhiHienThi()
        {
            // TODO (SV3): nạp danh sách lớp, môn học và chọn học kỳ mặc định.
        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            // TODO (SV3): khi đổi lớp thì nạp lại bảng điểm (nếu đã chọn môn).
        }

        private void btnTaiDanhSach_Click(object sender, EventArgs e)
        {
            // TODO (SV3): đổ sinh viên của lớp và môn đang chọn lên lvDiem.
        }

        private void lvDiem_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            // TODO (SV3): chỉ cho sửa cột Điểm QT/GK/CK, kiểm tra điểm trong [0, 10],
            //             tính lại tổng kết + xếp loại và tô đỏ dòng môn rớt.
        }

        private void btnLuuDiem_Click(object sender, EventArgs e)
        {
            // TODO (SV3): ghi điểm vừa sửa xuống dangky.txt.
        }

        private void btnXuatCsv_Click(object sender, EventArgs e)
        {
            // TODO (SV3): chọn nơi lưu và xuất bảng điểm lớp ra CSV.
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            // TODO (SV3): tải lại bảng điểm từ tệp.
        }
    }
}
