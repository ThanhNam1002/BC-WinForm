using BCWF.Services;
using System;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// Chức năng nâng cao: danh sách sinh viên bị cảnh báo học vụ.
    /// Dựa trên điểm trung bình tích lũy và ngưỡng DiemCanhBao trong App.config.
    /// </summary>
    public partial class FrmCanhBaoHocVu : Form
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        public FrmCanhBaoHocVu()
        {
            InitializeComponent();
        }

        private void FrmCanhBaoHocVu_Load(object sender, EventArgs e)
        {
            // TODO (nâng cao): lấy _diemService.LayDanhSachCanhBao() và đổ lên lvCanhBao.
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            // TODO (nâng cao): tải lại danh sách cảnh báo.
        }

        private void btnXuatCsv_Click(object sender, EventArgs e)
        {
            // TODO (nâng cao): xuất danh sách cảnh báo ra CSV.
        }
    }
}
