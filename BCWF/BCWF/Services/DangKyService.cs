using BCWF.Models;
using System;
using System.Collections.Generic;

namespace BCWF.Services
{
    /// <summary>
    /// SV2 - Đăng ký học phần.
    /// Gồm quản lý môn học và đăng ký bằng cách chuyển môn giữa 2 ListBox,
    /// kiểm tra tổng tín chỉ trong [MinTC, MaxTC] và chống đăng ký trùng.
    /// </summary>
    public class DangKyService
    {
        private readonly DataStore _store = DataStore.Instance;

        #region Quản lý môn học

        public List<MonHoc> LayTatCaMonHoc()
        {
            throw new NotImplementedException("TODO (SV2): trả về danh sách môn học.");
        }

        public void ThemMonHoc(MonHoc monHoc)
        {
            throw new NotImplementedException("TODO (SV2): thêm môn học, chặn trùng mã môn.");
        }

        public void CapNhatMonHoc(MonHoc monHoc)
        {
            throw new NotImplementedException("TODO (SV2): sửa môn học.");
        }

        public void XoaMonHoc(string maMon)
        {
            throw new NotImplementedException("TODO (SV2): xóa môn học (kiểm tra ràng buộc đăng ký).");
        }

        #endregion

        #region Đăng ký học phần

        /// <summary>Các môn sinh viên đã đăng ký trong học kỳ.</summary>
        public List<MonHoc> LayMonDaDangKy(string mssv, string hocKy)
        {
            throw new NotImplementedException("TODO (SV2): lấy môn đã đăng ký.");
        }

        /// <summary>Tổng số tín chỉ của danh sách môn đang chọn.</summary>
        public int TinhTongTinChi(IEnumerable<string> dsMaMon)
        {
            throw new NotImplementedException("TODO (SV2): cộng số tín chỉ.");
        }

        /// <summary>Kiểm tra tổng tín chỉ có nằm trong giới hạn MinTC - MaxTC (đọc từ App.config).</summary>
        public bool KiemTraGioiHanTinChi(IEnumerable<string> dsMaMon, out string thongBao)
        {
            throw new NotImplementedException("TODO (SV2): so sánh với MinTC/MaxTC trong App.config.");
        }

        /// <summary>Môn này đã có trong danh sách đăng ký của sinh viên trong học kỳ hay chưa.</summary>
        public bool DaDangKy(string mssv, string maMon, string hocKy)
        {
            throw new NotImplementedException("TODO (SV2): chống đăng ký trùng môn.");
        }

        /// <summary>Nâng cao: kiểm tra môn tiên quyết đã đạt (điểm tổng kết &gt;= 4).</summary>
        public bool KiemTraTienQuyet(string mssv, string maMon, out string thongBao)
        {
            throw new NotImplementedException("TODO (nâng cao): kiểm tra môn tiên quyết.");
        }

        /// <summary>Nâng cao: kiểm tra trùng thứ/tiết giữa các môn đăng ký (dùng lichhoc.txt).</summary>
        public bool KiemTraTrungLich(IEnumerable<string> dsMaMon, out string thongBao)
        {
            throw new NotImplementedException("TODO (nâng cao): kiểm tra trùng lịch học.");
        }

        /// <summary>Ghi nhận đăng ký một môn cho sinh viên.</summary>
        public void DangKy(string mssv, string maMon, string hocKy)
        {
            throw new NotImplementedException("TODO (SV2): thêm bản ghi vào dangky.txt.");
        }

        /// <summary>Hủy đăng ký một môn.</summary>
        public void HuyDangKy(string mssv, string maMon, string hocKy)
        {
            throw new NotImplementedException("TODO (SV2): xóa bản ghi khỏi dangky.txt.");
        }

        #endregion
    }
}
