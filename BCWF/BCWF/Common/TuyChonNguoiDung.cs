using Microsoft.Win32;
using System;

namespace BCWF.Common
{
    /// <summary>
    /// Lưu và đọc các tùy chọn người dùng trong Registry (HKCU\Software\BCWF):
    /// ghi nhớ đăng nhập, vị trí/kích thước form, màu/font đã chọn...
    /// Mọi thao tác đều có try-catch nên lỗi Registry không làm chương trình dừng.
    /// </summary>
    public static class TuyChonNguoiDung
    {
        private const string KhoaGoc = @"Software\BCWF";

        /// <summary>Đọc một giá trị chuỗi, trả về null nếu chưa có hoặc lỗi.</summary>
        public static string? DocChuoi(string ten)
        {
            try
            {
                using RegistryKey? khoa = Registry.CurrentUser.OpenSubKey(KhoaGoc);
                return khoa?.GetValue(ten) as string;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Ghi một giá trị chuỗi.</summary>
        public static void GhiChuoi(string ten, string giaTri)
        {
            try
            {
                using RegistryKey khoa = Registry.CurrentUser.CreateSubKey(KhoaGoc);
                khoa.SetValue(ten, giaTri);
            }
            catch
            {
                // Không ghi được Registry thì bỏ qua (môi trường hạn chế quyền).
            }
        }

        /// <summary>Đọc một giá trị kiểu bool ("1"/"0").</summary>
        public static bool DocBool(string ten, bool macDinh = false)
        {
            string? giaTri = DocChuoi(ten);
            return giaTri switch
            {
                "1" => true,
                "0" => false,
                _ => macDinh
            };
        }

        /// <summary>Ghi một giá trị kiểu bool ("1"/"0").</summary>
        public static void GhiBool(string ten, bool giaTri)
        {
            GhiChuoi(ten, giaTri ? "1" : "0");
        }

        /// <summary>Xóa một tùy chọn.</summary>
        public static void Xoa(string ten)
        {
            try
            {
                using RegistryKey? khoa = Registry.CurrentUser.OpenSubKey(KhoaGoc, true);
                khoa?.DeleteValue(ten, false);
            }
            catch
            {
                // Bỏ qua lỗi.
            }
        }
    }
}
