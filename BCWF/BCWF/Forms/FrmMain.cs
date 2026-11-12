using System;
using System.Windows.Forms;

namespace BCWF
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        /// <summary>Mở form con dạng MDI, nếu đã mở thì chỉ kích hoạt lại.</summary>
        private void MoFormCon<T>() where T : Form, new()
        {
            foreach (Form form in MdiChildren)
            {
                if (form is T)
                {
                    form.Activate();
                    return;
                }
            }

            var formCon = new T { MdiParent = this };
            formCon.Show();
        }

        #region Hệ thống

        private void thoatMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region Quản lý

        private void quanLySinhVienLopMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmSinhVien>();
        }

        private void dangKyHocPhanMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmDangKy>();
        }

        private void nhapDiemMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmNhapDiem>();
        }

        #endregion

        #region Báo cáo

        private void canhBaoHocVuMenuItem_Click(object sender, EventArgs e)
        {
            MoFormCon<Forms.FrmCanhBaoHocVu>();
        }

        #endregion

        #region Cửa sổ

        private void xepChongMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void xepNgangMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void xepDocMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void dongTatCaMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form form in MdiChildren)
            {
                form.Close();
            }
        }

        #endregion
    }
}
