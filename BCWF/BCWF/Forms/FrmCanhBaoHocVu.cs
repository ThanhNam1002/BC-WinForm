using BCWF.Common;
using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            NapDanhSach();
        }

        /// <summary>Đổ danh sách sinh viên bị cảnh báo lên ListView.</summary>
        private void NapDanhSach()
        {
            lvCanhBao.BeginUpdate();
            lvCanhBao.Items.Clear();

            double nguong = AppConfig.DiemCanhBao;

            // LayDanhSachCanhBao chỉ trả về sinh viên đã có ít nhất một môn nhập đủ điểm.
            foreach (SinhVien sv in _diemService.LayDanhSachCanhBao())
            {
                double dtb = _diemService.TinhDiemTrungBinhTichLuy(sv.MSSV);

                var item = new ListViewItem(sv.MSSV)
                {
                    Tag = sv
                };
                item.SubItems.Add(sv.HoTen);
                item.SubItems.Add(sv.MaLop);
                item.SubItems.Add(dtb.ToString("0.##", CultureInfo.InvariantCulture));
                item.SubItems.Add($"ĐTB dưới ngưỡng {nguong:0.0}");
                item.ForeColor = System.Drawing.Color.Red;
                lvCanhBao.Items.Add(item);
            }
            lvCanhBao.EndUpdate();

            lblTongHop.Text = $"Số sinh viên bị cảnh báo: {lvCanhBao.Items.Count} (ngưỡng ĐTB < {nguong:0.0})";
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            NapDanhSach();
        }

        private void btnXuatCsv_Click(object sender, EventArgs e)
        {
            if (lvCanhBao.Items.Count == 0)
            {
                MessageBox.Show("Không có sinh viên nào bị cảnh báo để xuất.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Xuất danh sách cảnh báo học vụ",
                Filter = "Tệp CSV (*.csv)|*.csv",
                FileName = $"CanhBaoHocVu_{DateTime.Now:yyyyMMdd}.csv"
            };

            if (sfd.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            try
            {
                double nguong = AppConfig.DiemCanhBao;
                var cacDong = new List<string[]>
                {
                    new[] { "MSSV", "Họ tên", "Lớp", "ĐTB tích lũy", "Mức cảnh báo" }
                };

                foreach (ListViewItem item in lvCanhBao.Items)
                {
                    if (item.Tag is SinhVien sv)
                    {
                        cacDong.Add(new[]
                        {
                            sv.MSSV,
                            sv.HoTen,
                            sv.MaLop,
                            _diemService.TinhDiemTrungBinhTichLuy(sv.MSSV).ToString("0.##", CultureInfo.InvariantCulture),
                            $"ĐTB dưới ngưỡng {nguong:0.0}"
                        });
                    }
                }

                FileHelpers.GhiCsv(sfd.FileName, cacDong);
                MessageBox.Show($"Đã xuất danh sách cảnh báo ra tệp:\n{sfd.FileName}", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception loi)
            {
                FileHelpers.GhiLog("Lỗi xuất CSV cảnh báo học vụ: " + loi.Message);
                MessageBox.Show(loi.Message, "Lỗi xuất CSV",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
