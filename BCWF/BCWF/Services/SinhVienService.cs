using BCWF.Models;
using System;
using System.Collections.Generic;

namespace BCWF.Services
{
    /// <summary>
    /// SV1 - Sinh viên &amp; lớp.
    /// Các hàm dưới đây là khung nghiệp vụ, cài đặt chi tiết sẽ bổ sung sau.
    /// Dữ liệu dùng chung lấy từ <see cref="DataStore.Instance"/>.
    /// </summary>
    public class SinhVienService
    {
        private readonly DataStore _store = DataStore.Instance;

        /// <summary>Lấy danh sách sinh viên thuộc một khoa (dùng khi chọn nút Khoa trên TreeView).</summary>
        public List<SinhVien> LayTheoKhoa(string maKhoa)
        {
            if (string.IsNullOrWhiteSpace(maKhoa))
            {
                return new List<SinhVien>();
            }

            var maLopCuaKhoa = _store.Lops
                .Where(lop => string.Equals(lop.MaKhoa, maKhoa, StringComparison.OrdinalIgnoreCase))
                .Select(lop => lop.MaLop)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return _store.SinhViens
                .Where(sv => maLopCuaKhoa.Contains(sv.MaLop))
                .OrderBy(sv => sv.MSSV)
                .ToList();
        }

        /// <summary>Lấy danh sách sinh viên thuộc một lớp (dùng khi chọn nút Lớp trên TreeView).</summary>
        public List<SinhVien> LayTheoMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                return new List<SinhVien>();
            }

            return _store.SinhViens
                .Where(sv => string.Equals(sv.MaLop, maLop, StringComparison.OrdinalIgnoreCase))
                .OrderBy(sv => sv.MSSV)
                .ToList();
        }

        /// <summary>Kiểm tra MSSV đã tồn tại hay chưa (bỏ qua chính sinh viên đang sửa).</summary>
        public bool KiemTraTrungMSSV(string mssv, string? mssvDangSua = null)
        {
            throw new NotImplementedException("TODO (SV1): chặn trùng MSSV.");
        }

        /// <summary>Thêm sinh viên mới (kiểm tra trùng MSSV, kiểm tra dữ liệu bắt buộc).</summary>
        public void Them(SinhVien sinhVien)
        {
            throw new NotImplementedException("TODO (SV1): thêm sinh viên.");
        }

        /// <summary>Cập nhật thông tin sinh viên đang chọn.</summary>
        public void CapNhat(SinhVien sinhVien)
        {
            throw new NotImplementedException("TODO (SV1): sửa sinh viên.");
        }

        /// <summary>Xóa sinh viên theo MSSV (kèm đăng ký học phần liên quan).</summary>
        public void Xoa(string mssv)
        {
            throw new NotImplementedException("TODO (SV1): xóa sinh viên.");
        }

        /// <summary>Nhập danh sách sinh viên từ tệp CSV.</summary>
        /// <returns>Số sinh viên nhập thành công.</returns>
        public int NhapTuCsv(string duongDanCsv)
        {
            throw new NotImplementedException("TODO (SV1): đọc CSV và thêm sinh viên.");
        }

        /// <summary>Chép ảnh thẻ vào thư mục Data/Images và trả về tên tệp đã lưu.</summary>
        public string LuuAnh(string mssv, string duongDanAnhNguon)
        {
            throw new NotImplementedException("TODO (SV1): sao chép ảnh thẻ vào Data/Images.");
        }
    }
}
