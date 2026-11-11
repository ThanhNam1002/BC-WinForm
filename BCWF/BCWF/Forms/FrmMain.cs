using BCWF.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCWF
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        #region 1. Menu Hệ Thống
        private void dangNhapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmDangNhap)
                {
                    f.Activate();
                    return;
                }
            }

            FrmDangNhap dangNhapItem = new FrmDangNhap();
            dangNhapItem.MdiParent = this;
            dangNhapItem.Show();
        }

        private void doiMatKhauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmDoiMatKhau)
                {
                    f.Activate();
                    return;
                }
            }

            FrmDoiMatKhau doiMatKhauItem = new FrmDoiMatKhau();
            doiMatKhauItem.MdiParent = this;
            doiMatKhauItem.Show();
        }
        private void thoatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 2. Menu Quản Lý Danh Mục
        private void quanLySinhVienLopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmSinhVien)
                {
                    f.Activate();
                    return;
                }
            }

            FrmSinhVien svItem = new FrmSinhVien();
            svItem.MdiParent = this;
            svItem.Show();
        }

        private void dangKyHocPhanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmDangKy)
                {
                    f.Activate();
                    return;
                }
            }

            FrmDangKy dkhpItem = new FrmDangKy();
            dkhpItem.MdiParent = this;
            dkhpItem.Show();
        }

        private void nhapDiemBangDiemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmNhapDiem)
                {
                    f.Activate();
                    return;
                }
            }

            FrmNhapDiem nhapDiemItem = new FrmNhapDiem();
            nhapDiemItem.MdiParent = this;
            nhapDiemItem.Show();
        }
        #endregion

        #region 3. Menu Thống Kê
        private void saoLuuKhoiPhucDuLieuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmSaoLuuBaoCao)
                {
                    f.Activate();
                    return;
                }
            }

            FrmSaoLuuBaoCao saoLuuItem = new FrmSaoLuuBaoCao();
            saoLuuItem.MdiParent = this;
            saoLuuItem.Show();
        }

        private void xuatBaoCaoThongKeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f is FrmSaoLuuBaoCao)
                {
                    f.Activate();
                    return;
                }
            }

            FrmSaoLuuBaoCao baoCaoItem = new FrmSaoLuuBaoCao();
            baoCaoItem.MdiParent = this;
            baoCaoItem.Show();
        }
        #endregion

        #region 4. Menu Cửa Sổ
        private void xepChongToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void xepNgangToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void xepDocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void dongTatCaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }
        }
        #endregion

    }
}
