using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Linq;

namespace BCWF.Common
{
    /// <summary>
    /// Đọc các khóa cấu hình trong App.config (phần &lt;appSettings&gt;).
    /// Dự án dùng .NET (SDK-style) nên đọc trực tiếp tệp App.config bằng XDocument,
    /// không cần thêm gói System.Configuration.
    /// </summary>
    public static class AppConfig
    {
        public const string TenTepCauHinh = "App.config";

        private static readonly Dictionary<string, string> CauHinh = NapCauHinh();

        /// <summary>Thư mục dữ liệu (mặc định "Data").</summary>
        public static string DataFolder => LayChuoi("DataFolder", "Data") ?? "Data";

        /// <summary>Số tín chỉ tối thiểu phải đăng ký trong một học kỳ.</summary>
        public static int MinTC => LaySoNguyen("MinTC", 12);

        /// <summary>Số tín chỉ tối đa được đăng ký trong một học kỳ.</summary>
        public static int MaxTC => LaySoNguyen("MaxTC", 25);

        /// <summary>Học kỳ đang thao tác mặc định.</summary>
        public static string HocKyHienTai => LayChuoi("HocKyHienTai", "HK1") ?? "HK1";

        /// <summary>Ngưỡng điểm trung bình tích lũy bị cảnh báo học vụ.</summary>
        public static double DiemCanhBao => LaySoThuc("DiemCanhBao", 1.0);

        /// <summary>Đọc một khóa cấu hình bất kỳ, trả về null nếu không có.</summary>
        public static string? LayChuoi(string khoa, string? macDinh = null)
        {
            return CauHinh.TryGetValue(khoa, out string? giaTri) && !string.IsNullOrWhiteSpace(giaTri)
                ? giaTri.Trim()
                : macDinh;
        }

        private static int LaySoNguyen(string khoa, int macDinh)
        {
            return int.TryParse(LayChuoi(khoa), NumberStyles.Integer, CultureInfo.InvariantCulture, out int ketQua)
                ? ketQua
                : macDinh;
        }

        private static double LaySoThuc(string khoa, double macDinh)
        {
            return double.TryParse(LayChuoi(khoa), NumberStyles.Float, CultureInfo.InvariantCulture, out double ketQua)
                ? ketQua
                : macDinh;
        }

        private static Dictionary<string, string> NapCauHinh()
        {
            var ketQua = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            string? duongDan = TimTepCauHinh();
            if (duongDan == null)
            {
                return ketQua;
            }

            try
            {
                foreach (XElement phanTu in XDocument.Load(duongDan).Descendants("appSettings").Elements("add"))
                {
                    string? khoa = phanTu.Attribute("key")?.Value;
                    string? giaTri = phanTu.Attribute("value")?.Value;
                    if (!string.IsNullOrWhiteSpace(khoa))
                    {
                        ketQua[khoa.Trim()] = giaTri ?? string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                // Cấu hình lỗi/không đọc được thì dùng giá trị mặc định.
            }

            return ketQua;
        }

        private static string? TimTepCauHinh()
        {
            // 1. Cạnh tệp thực thi (App.config được copy ra output).
            string cungThuMuc = Path.Combine(AppContext.BaseDirectory, TenTepCauHinh);
            if (File.Exists(cungThuMuc))
            {
                return cungThuMuc;
            }

            // 2. Đi ngược lên các thư mục cha để tìm App.config của dự án.
            var thuMuc = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 8 && thuMuc != null; i++, thuMuc = thuMuc.Parent)
            {
                string ungVien = Path.Combine(thuMuc.FullName, TenTepCauHinh);
                if (File.Exists(ungVien))
                {
                    return ungVien;
                }
            }

            return null;
        }
    }
}
