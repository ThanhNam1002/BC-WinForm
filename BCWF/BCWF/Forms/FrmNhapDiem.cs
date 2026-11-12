using BCWF.Models;
using BCWF.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace BCWF.Forms
{
    /// <summary>
    /// SV3 - Điểm.
    /// Nhập điểm theo lớp và môn, sửa trực tiếp trên ListView, tính điểm tổng kết
    /// (30% QT + 30% GK + 40% CK), xếp loại, tô đỏ môn rớt, điểm trung bình tích lũy
    /// và xuất bảng điểm của lớp ra CSV.
    /// </summary>
    public partial class FrmNhapDiem : Form
    {
        /// <summary>Vị trí cột bắt đầu trong ListView (0 = MSSV, 1 = Họ tên).</summary>
        private const int CotDiemQT = 2;
        private const int CotDiemGK = 3;
        private const int CotDiemCK = 4;
        private const int CotTongKet = 5;
        private const int CotXepLoai = 6;
        private const int CotDtbTichLuy = 7;
        private const int CotGhiChu = 8;

        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        private Button _btnBangDiemCaNhan = new Button();

        //Ô nhập điểm đặt đè lên ô đang sửa trên ListView (ListView chỉ sửa được cột đầu tiên).
        private readonly TextBox _txtEditDiem = new TextBox();
        private ListViewItem? _dongDangSua;
        private int _cotDangSua = -1;

        public FrmNhapDiem()
        {
            InitializeComponent();

            ThemNutBangDiemCaNhan();
            ChuBienHienThiBangDiem();

            cboMon.SelectedIndexChanged += cboMon_SelectedIndexChanged;
            lvDiem.SelectedIndexChanged += lvDiem_SelectedIndexChanged;
        }

        private void FrmNhapDiem_Load(object sender, EventArgs e)
        {
            NapDanhSachLop();
            NapDanhSachMon();
            NapHocKy();
        }

        #region Nạp dữ liệu lên ComboBox

        //Nạp danh sách lớp vào ComboBox
        private void NapDanhSachLop()
        {
            cboLop.Items.Clear();
            foreach (Lop lop in _store.Lops.OrderBy(l => l.MaLop, StringComparer.OrdinalIgnoreCase))
            {
                cboLop.Items.Add(lop);
            }

            cboLop.SelectedIndex = cboLop.Items.Count > 0 ? 0 : -1;
        }

        //Nạp danh sách môn học vào ComboBox
        private void NapDanhSachMon()
        {
            cboMon.Items.Clear();
            foreach (MonHoc mon in _store.MonHocs.OrderBy(m => m.MaMon, StringComparer.OrdinalIgnoreCase))
            {
                cboMon.Items.Add(mon);
            }

            cboMon.SelectedIndex = cboMon.Items.Count > 0 ? 0 : -1;
        }

        //Chọn học kỳ đang thao tác theo App.config
        private void NapHocKy()
        {
            string hocKy = Common.AppConfig.HocKyHienTai;

            for (int i = 0; i < cboHocKy.Items.Count; i++)
            {
                if (string.Equals(cboHocKy.Items[i]?.ToString(), hocKy, StringComparison.OrdinalIgnoreCase))
                {
                    cboHocKy.SelectedIndex = i;
                    return;
                }
            }

            if (cboHocKy.SelectedIndex < 0 && cboHocKy.Items.Count > 0)
            {
                cboHocKy.SelectedIndex = 0;
            }
        }

        //Thêm nút xem bảng điểm cá nhân (không sửa file Designer)
        private void ThemNutBangDiemCaNhan()
        {
            _btnBangDiemCaNhan.Name = "btnBangDiemCaNhan";
            _btnBangDiemCaNhan.Text = "Bảng điểm cá nhân";
            _btnBangDiemCaNhan.Size = new Size(150, 34);
            _btnBangDiemCaNhan.Location = new Point(300, 14);
            _btnBangDiemCaNhan.UseVisualStyleBackColor = true;
            _btnBangDiemCaNhan.Enabled = false;
            _btnBangDiemCaNhan.Click += btnBangDiemCaNhan_Click;

            pnlBottom.Controls.Add(_btnBangDiemCaNhan);
        }

        #endregion

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Khi đổi lớp thì bảng điểm cũ không còn đúng, nên xóa và tải lại danh sách.
            XoaBangDiem();
        }

        private void cboMon_SelectedIndexChanged(object? sender, EventArgs e)
        {
            XoaBangDiem();
        }

        private void btnTaiDanhSach_Click(object sender, EventArgs e)
        {
            if (!KiemTraChonLopMon(out Lop? lop, out MonHoc? mon))
            {
                return;
            }

            LoadBangDiem(lop, mon);
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            //Bỏ ô đang sửa (nếu có) rồi tải lại toàn bộ dữ liệu từ tệp.
            DongBangDiem();
            _store.LoadAll();

            NapDanhSachLop();
            NapDanhSachMon();
            NapHocKy();

            if (KiemTraChonLopMon(out Lop? lop, out MonHoc? mon))
            {
                LoadBangDiem(lop, mon);
            }
        }

        #region Hiển thị bảng điểm

        //Đổ danh sách sinh viên của lớp và môn đang chọn lên lvDiem
        private void LoadBangDiem(Lop lop, MonHoc mon)
        {
            string hocKy = HocKyHienTai();

            lvDiem.BeginUpdate();
            lvDiem.Items.Clear();

            foreach (SinhVien sv in _diemService.LaySinhVienTrongLop(lop.MaLop))
            {
                DangKy? dangKy = _diemService.TimDangKy(sv.MSSV, mon.MaMon, hocKy);

                ListViewItem item = new ListViewItem(sv.MSSV);
                item.SubItems.Add(sv.HoTen);
                item.SubItems.Add(DinhDangDiem(dangKy?.DiemQT));
                item.SubItems.Add(DinhDangDiem(dangKy?.DiemGK));
                item.SubItems.Add(DinhDangDiem(dangKy?.DiemCK));
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                item.SubItems.Add(string.Empty);
                item.Tag = new ThongTinDong
                {
                    MSSV = sv.MSSV,
                    DiemQT = dangKy?.DiemQT,
                    DiemGK = dangKy?.DiemGK,
                    DiemCK = dangKy?.DiemCK
                };

                lvDiem.Items.Add(item);

                CapNhatDong(item);
            }

            lvDiem.EndUpdate();

            CapNhatTongKet();
        }

        //Xóa bảng điểm trên ListView
        private void XoaBangDiem()
        {
            DongBangDiem();
            lvDiem.Items.Clear();
            CapNhatTongKet();
        }

        //Tính lại tổng kết, xếp loại, điểm tích lũy và tô đỏ môn rớt cho một dòng
        private void CapNhatDong(ListViewItem item)
        {
            if (item.Tag is not ThongTinDong thongTin)
            {
                return;
            }

            if (cboMon.SelectedItem is not MonHoc mon)
            {
                return;
            }

            DangKy dangKy = new DangKy
            {
                MSSV = thongTin.MSSV,
                MaMon = mon.MaMon,
                HocKy = HocKyHienTai(),
                DiemQT = thongTin.DiemQT,
                DiemGK = thongTin.DiemGK,
                DiemCK = thongTin.DiemCK
            };

            double? tongKet = _diemService.TinhDiemTongKet(dangKy);
            bool rot = _diemService.LaMonRot(dangKy);
            bool daDangKy = _diemService.TimDangKy(thongTin.MSSV, mon.MaMon, HocKyHienTai()) != null;

            item.SubItems[CotTongKet].Text = tongKet.HasValue ? DinhDangDiem(tongKet) : string.Empty;
            item.SubItems[CotXepLoai].Text = tongKet.HasValue ? _diemService.XepLoai(tongKet.Value) : string.Empty;
            item.SubItems[CotDtbTichLuy].Text = DinhDangDiem(_diemService.TinhDiemTrungBinhTichLuy(thongTin.MSSV, dangKy));
            item.SubItems[CotGhiChu].Text = rot ? "Rớt môn" : (daDangKy ? string.Empty : "Chưa đăng ký");

            //Tô đỏ toàn bộ dòng khi sinh viên rớt môn.
            item.ForeColor = rot ? Color.Red : SystemColors.WindowText;
        }

        //Cập nhật nhãn tổng kết số sinh viên / số môn rớt
        private void CapNhatTongKet()
        {
            int soRot = 0;
            foreach (ListViewItem item in lvDiem.Items)
            {
                if (string.Equals(item.SubItems[CotGhiChu].Text, "Rớt môn", StringComparison.OrdinalIgnoreCase))
                {
                    soRot++;
                }
            }

            lblTongKet.Text = $"Số sinh viên: {lvDiem.Items.Count} - Số môn rớt: {soRot}";
        }

        #endregion

        #region Sửa điểm trực tiếp trên ListView

        //Chuẩn bị ô nhập đè lên ListView để sửa các cột điểm
        private void ChuBienHienThiBangDiem()
        {
            _txtEditDiem.BorderStyle = BorderStyle.FixedSingle;
            _txtEditDiem.Font = lvDiem.Font;
            _txtEditDiem.TextAlign = HorizontalAlignment.Center;
            _txtEditDiem.Visible = false;
            _txtEditDiem.KeyDown += txtEditDiem_KeyDown;

            lvDiem.Controls.Add(_txtEditDiem);

            //ListView chỉ cho sửa cột đầu tiên, nên tắt chế độ sửa có sẵn của ListView.
            lvDiem.BeforeLabelEdit += lvDiem_BeforeLabelEdit;
            lvDiem.MouseDown += lvDiem_MouseDown;
            lvDiem.MouseWheel += lvDiem_MouseWheel;

            //Rời khỏi form thì chốt luôn giá trị đang sửa.
            Deactivate += FrmNhapDiem_Deactivate;

            //Đóng form thì bỏ ô nhập đang dở.
            FormClosing += FrmNhapDiem_FormClosing;
        }

        //Chặn chế độ sửa mặc định của ListView (chỉ sửa được cột MSSV)
        private void lvDiem_BeforeLabelEdit(object? sender, LabelEditEventArgs e)
        {
            e.CancelEdit = true;
        }

        //Bấm vào ô điểm QT/GK/CK thì hiện ô nhập đè lên ô đó
        private void lvDiem_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            //HitTest cho biết đang bấm vào dòng nào, cột nào.
            ListViewHitTestInfo hit = lvDiem.HitTest(e.Location);

            //Chốt lần sửa đang dở (nếu có) trước khi bắt đầu sửa ô mới.
            CommitEdit();

            if (hit.Item == null || hit.SubItem == null)
            {
                return;
            }

            //Chỉ số của subitem trong ListViewItem trùng với chỉ số cột trên ListView.
            int cot = hit.Item.SubItems.IndexOf(hit.SubItem);
            if (cot < CotDiemQT || cot > CotDiemCK)
            {
                return;
            }

            BatDauSua(hit.Item, cot, hit.SubItem.Bounds);
        }

        //Lăn chuột (cuộn danh sách) thì ẩn ô nhập cho khỏi lệch vị trí
        private void lvDiem_MouseWheel(object? sender, MouseEventArgs e)
        {
            DongBangDiem();
        }

        //Rời khỏi form thì chốt giá trị đang sửa
        private void FrmNhapDiem_Deactivate(object? sender, EventArgs e)
        {
            CommitEdit();
        }

        //Đóng form thì bỏ ô nhập đang dở
        private void FrmNhapDiem_FormClosing(object? sender, FormClosingEventArgs e)
        {
            DongBangDiem();
        }

        //Hiện ô nhập đè lên ô điểm của dòng
        private void BatDauSua(ListViewItem item, int cot, Rectangle boundsO)
        {
            if (item.Tag is not ThongTinDong thongTin)
            {
                return;
            }

            _dongDangSua = item;
            _cotDangSua = cot;

            _txtEditDiem.Text = item.SubItems[cot].Text.Trim();

            Rectangle khung = LayKhungO(item, cot, boundsO);
            _txtEditDiem.Bounds = khung;
            _txtEditDiem.Visible = true;

            //Trì hoãn lấy focus để thắng việc ListView giành focus khi bấm chuột.
            try
            {
                lvDiem.BeginInvoke(new Action(() =>
                {
                    if (_txtEditDiem.Visible && !_txtEditDiem.Focused)
                    {
                        _txtEditDiem.Focus();
                        _txtEditDiem.SelectAll();
                    }
                }));
            }
            catch (InvalidOperationException)
            {
                //Form đang đóng thì bỏ qua.
            }
        }

        //Tính vị trí ô điểm để đặt ô nhập đè lên
        private Rectangle LayKhungO(ListViewItem item, int cot, Rectangle boundsO)
        {
            //Ưu tiên hình chữ nhật do ListView cung cấp khi bấm chuột.
            if (boundsO.Width > 0 && boundsO.Height > 0)
            {
                return new Rectangle(
                    boundsO.X + 1,
                    boundsO.Y + 1,
                    Math.Max(boundsO.Width - 2, 30),
                    Math.Max(boundsO.Height - 2, 18));
            }

            //Nếu không có thì tính lại từ độ rộng các cột.
            Rectangle khungDong = lvDiem.GetItemRect(item.Index, ItemBoundsPortion.Entire);

            int x = 0;
            for (int i = 0; i < cot && i < lvDiem.Columns.Count; i++)
            {
                x += lvDiem.Columns[i].Width;
            }

            int rong = cot < lvDiem.Columns.Count ? lvDiem.Columns[cot].Width : 60;

            return new Rectangle(
                x + 2,
                khungDong.Y + 2,
                Math.Max(rong - 4, 30),
                Math.Max(khungDong.Height - 3, 18));
        }

        //Chốt giá trị đang sửa (Enter, bấm ô khác, rời form...)
        private void CommitEdit()
        {
            if (!_txtEditDiem.Visible)
            {
                return;
            }

            string noiDung = _txtEditDiem.Text.Trim();
            ListViewItem? item = _dongDangSua;
            int cot = _cotDangSua;

            DongBangDiem();

            if (item == null || cot < CotDiemQT || cot > CotDiemCK || item.Tag is not ThongTinDong thongTin)
            {
                return;
            }

            //Nội dung không đổi thì không cần cập nhật.
            if (string.Equals(noiDung, item.SubItems[cot].Text.Trim(), StringComparison.Ordinal))
            {
                return;
            }

            double? diem = null;
            if (noiDung.Length > 0)
            {
                if (!ThuDocDiem(noiDung, out double giaTri) || giaTri < 0 || giaTri > 10)
                {
                    MessageBox.Show(
                        "Điểm phải là số trong khoảng [0, 10]. Để trống nếu chưa có điểm.",
                        "Điểm không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    item.SubItems[cot].Text = DiemCuaCot(thongTin, cot);
                    return;
                }

                diem = giaTri;
            }

            switch (cot)
            {
                case CotDiemQT:
                    thongTin.DiemQT = diem;
                    break;
                case CotDiemGK:
                    thongTin.DiemGK = diem;
                    break;
                case CotDiemCK:
                    thongTin.DiemCK = diem;
                    break;
            }

            item.SubItems[cot].Text = DinhDangDiem(diem);

            CapNhatDong(item);
            CapNhatTongKet();
        }

        //Đóng ô nhập mà không áp dụng giá trị (Esc, cuộn danh sách)
        private void DongBangDiem()
        {
            _txtEditDiem.Visible = false;
            _dongDangSua = null;
            _cotDangSua = -1;
        }

        //Enter để xác nhận, Esc để hủy
        private void txtEditDiem_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                CommitEdit();
                lvDiem.Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                e.SuppressKeyPress = true;
                DongBangDiem();
                lvDiem.Focus();
            }
        }

        #endregion

        private void lvDiem_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Item < 0 || e.Item >= lvDiem.Items.Count)
            {
                e.CancelEdit = true;
                return;
            }

            ListViewItem? item = lvDiem.Items[e.Item];
            if (item == null || item.Tag is not ThongTinDong thongTin)
            {
                e.CancelEdit = true;
                return;
            }

            //Tìm cột vừa được sửa (so sánh nội dung hiện tại với giá trị đang lưu trên dòng).
            int cot = TimCotVuaDong(item, thongTin);
            if (cot < 0)
            {
                //Không có thay đổi nào (người dùng gõ lại đúng giá trị cũ).
                e.CancelEdit = true;
                return;
            }

            //Chỉ cho phép sửa 3 cột điểm QT / GK / CK.
            if (cot < CotDiemQT || cot > CotDiemCK)
            {
                e.CancelEdit = true;
                return;
            }

            string noiDung = item.SubItems[cot].Text.Trim();
            double? diem = null;

            if (noiDung.Length > 0)
            {
                if (!ThuDocDiem(noiDung, out double giaTri) || giaTri < 0 || giaTri > 10)
                {
                    MessageBox.Show(
                        "Điểm phải là số trong khoảng [0, 10]. Để trống nếu chưa có điểm.",
                        "Điểm không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    e.CancelEdit = true;
                    item.SubItems[cot].Text = DiemCuaCot(thongTin, cot);
                    return;
                }

                diem = giaTri;
            }

            //Cập nhật giá trị vừa sửa để tính lại các cột còn lại.
            switch (cot)
            {
                case CotDiemQT:
                    thongTin.DiemQT = diem;
                    break;
                case CotDiemGK:
                    thongTin.DiemGK = diem;
                    break;
                case CotDiemCK:
                    thongTin.DiemCK = diem;
                    break;
            }

            item.SubItems[cot].Text = DinhDangDiem(diem);

            CapNhatDong(item);
            CapNhatTongKet();
        }

        //Tìm chỉ số cột điểm vừa được sửa, -1 nếu không có thay đổi nào
        private static int TimCotVuaDong(ListViewItem item, ThongTinDong thongTin)
        {
            string[] giaTriCu =
            {
                DinhDangDiem(thongTin.DiemQT),
                DinhDangDiem(thongTin.DiemGK),
                DinhDangDiem(thongTin.DiemCK)
            };

            for (int i = 0; i < giaTriCu.Length; i++)
            {
                string hienTai = item.SubItems[CotDiemQT + i].Text.Trim();
                if (!string.Equals(hienTai, giaTriCu[i], StringComparison.Ordinal))
                {
                    return CotDiemQT + i;
                }
            }

            return -1;
        }

        private void lvDiem_SelectedIndexChanged(object? sender, EventArgs e)
        {
            _btnBangDiemCaNhan.Enabled = lvDiem.SelectedItems.Count > 0;
        }

        private void btnLuuDiem_Click(object sender, EventArgs e)
        {
            //Chốt ô đang sửa (nếu có) trước khi lưu.
            CommitEdit();

            if (!KiemTraChonLopMon(out Lop? lop, out MonHoc? mon))
            {
                return;
            }

            if (lvDiem.Items.Count == 0)
            {
                MessageBox.Show("Chưa có sinh viên nào để lưu điểm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string hocKy = HocKyHienTai();

                foreach (ListViewItem item in lvDiem.Items)
                {
                    if (item.Tag is not ThongTinDong thongTin)
                    {
                        continue;
                    }

                    //Chỉ ghi những sinh viên đã có điểm, tránh tạo đăng ký thừa.
                    if (!thongTin.DiemQT.HasValue && !thongTin.DiemGK.HasValue && !thongTin.DiemCK.HasValue)
                    {
                        continue;
                    }

                    _diemService.LuuDiem(
                        thongTin.MSSV,
                        mon.MaMon,
                        hocKy,
                        thongTin.DiemQT,
                        thongTin.DiemGK,
                        thongTin.DiemCK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi lưu điểm", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(
                $"Đã lưu điểm môn {mon.TenMon} ({lop.MaLop}) học kỳ {HocKyHienTai()}.",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void btnXuatCsv_Click(object sender, EventArgs e)
        {
            //Chốt ô đang sửa (nếu có) trước khi xuất.
            CommitEdit();

            if (!KiemTraChonLopMon(out Lop? lop, out MonHoc? mon))
            {
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Xuất bảng điểm ra tệp CSV",
                Filter = "Tệp CSV|*.csv|Tất cả tệp|*.*",
                DefaultExt = "csv",
                AddExtension = true,
                FileName = $"BangDiem_{lop.MaLop}_{mon.MaMon}_{HocKyHienTai()}.csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            try
            {
                _diemService.XuatBangDiemCsv(lop.MaLop, mon.MaMon, HocKyHienTai(), sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi xuất CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show(
                $"Đã xuất bảng điểm ra tệp:\n{sfd.FileName}",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        #region Bảng điểm cá nhân

        private void btnBangDiemCaNhan_Click(object? sender, EventArgs e)
        {
            if (lvDiem.SelectedItems.Count == 0)
            {
                MessageBox.Show("Hãy chọn sinh viên cần xem bảng điểm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (lvDiem.SelectedItems[0].Tag is not ThongTinDong thongTin)
            {
                return;
            }

            SinhVien? sv = _store.SinhViens.FirstOrDefault(s =>
                string.Equals(s.MSSV, thongTin.MSSV, StringComparison.OrdinalIgnoreCase));

            if (sv == null)
            {
                MessageBox.Show("Không tìm thấy thông tin sinh viên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<BangDiemCaNhanItem> bangDiem = _diemService.LayBangDiemCaNhan(sv.MSSV);
            double dtbTichLuy = _diemService.TinhDiemTrungBinhTichLuy(sv.MSSV);

            using FrmBangDiemCaNhan frm = new FrmBangDiemCaNhan(sv, bangDiem, dtbTichLuy);
            frm.ShowDialog(this);
        }

        #endregion

        #region Hàm dùng chung

        //Kiểm tra đã chọn lớp và môn học chưa
        private bool KiemTraChonLopMon([NotNullWhen(true)] out Lop? lop, [NotNullWhen(true)] out MonHoc? mon)
        {
            lop = cboLop.SelectedItem as Lop;
            mon = cboMon.SelectedItem as MonHoc;

            if (lop == null)
            {
                MessageBox.Show("Hãy chọn lớp.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLop.Focus();
                return false;
            }

            if (mon == null)
            {
                MessageBox.Show("Hãy chọn môn học.", "Thiếu dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboMon.Focus();
                return false;
            }

            return true;
        }

        //Học kỳ đang chọn trên ComboBox
        private string HocKyHienTai()
        {
            return cboHocKy.SelectedItem?.ToString() ?? Common.AppConfig.HocKyHienTai;
        }

        //Chuyển chuỗi thành điểm (chấp nhận cả dấu phẩy lẫn dấu chấm)
        private static bool ThuDocDiem(string chuoi, out double diem)
        {
            return double.TryParse(
                chuoi.Replace(',', '.'),
                NumberStyles.Float,
                CultureInfo.InvariantCulture,
                out diem);
        }

        //Định dạng điểm hiển thị trên ListView
        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }

        //Lấy điểm cũ của một cột để khôi phục khi nhập sai
        private static string DiemCuaCot(ThongTinDong thongTin, int cot)
        {
            switch (cot)
            {
                case CotDiemQT:
                    return DinhDangDiem(thongTin.DiemQT);
                case CotDiemGK:
                    return DinhDangDiem(thongTin.DiemGK);
                case CotDiemCK:
                    return DinhDangDiem(thongTin.DiemCK);
                default:
                    return string.Empty;
            }
        }

        #endregion

        /// <summary>Thông tin điểm gắn với một dòng của ListView.</summary>
        private class ThongTinDong
        {
            public string MSSV { get; set; } = string.Empty;
            public double? DiemQT { get; set; }
            public double? DiemGK { get; set; }
            public double? DiemCK { get; set; }
        }
    }

    /// <summary>
    /// Bảng điểm cá nhân của một sinh viên: các môn đã đăng ký, điểm tổng kết,
    /// xếp loại và điểm trung bình tích lũy.
    /// </summary>
    internal class FrmBangDiemCaNhan : Form
    {
        public FrmBangDiemCaNhan(SinhVien sinhVien, List<BangDiemCaNhanItem> bangDiem, double dtbTichLuy)
        {
            Text = $"Bảng điểm cá nhân - {sinhVien.MSSV} {sinhVien.HoTen}";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 480);
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(700, 400);

            ListView lv = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                MultiSelect = false
            };

            lv.Columns.Add("Mã môn", 90);
            lv.Columns.Add("Tên môn", 220);
            lv.Columns.Add("TC", 50, HorizontalAlignment.Center);
            lv.Columns.Add("Học kỳ", 80, HorizontalAlignment.Center);
            lv.Columns.Add("QT", 60, HorizontalAlignment.Center);
            lv.Columns.Add("GK", 60, HorizontalAlignment.Center);
            lv.Columns.Add("CK", 60, HorizontalAlignment.Center);
            lv.Columns.Add("Tổng kết", 80, HorizontalAlignment.Center);
            lv.Columns.Add("Xếp loại", 80, HorizontalAlignment.Center);

            foreach (BangDiemCaNhanItem mon in bangDiem)
            {
                ListViewItem item = new ListViewItem(mon.MaMon);
                item.SubItems.Add(mon.TenMon);
                item.SubItems.Add(mon.SoTC.ToString(CultureInfo.InvariantCulture));
                item.SubItems.Add(mon.HocKy);
                item.SubItems.Add(DinhDangDiem(mon.DiemQT));
                item.SubItems.Add(DinhDangDiem(mon.DiemGK));
                item.SubItems.Add(DinhDangDiem(mon.DiemCK));
                item.SubItems.Add(DinhDangDiem(mon.DiemTongKet));
                item.SubItems.Add(mon.XepLoai);
                item.ForeColor = mon.LaMonRot ? Color.Red : SystemColors.WindowText;
                lv.Items.Add(item);
            }

            Label lblTongKet = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 32,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
                Text = $"Điểm trung bình tích lũy: {dtbTichLuy.ToString("0.00", CultureInfo.InvariantCulture)}"
            };

            Controls.Add(lv);
            Controls.Add(lblTongKet);
        }

        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }
    }
}
