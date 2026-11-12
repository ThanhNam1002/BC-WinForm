using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BCWF.Models
{
    /// <summary>
    /// Một sinh viên.
    /// Định dạng tệp Data/sinhvien.txt: MSSV;HoTen;NgaySinh;GioiTinh;MaLop;Anh
    /// (cột Anh là phần bổ sung để lưu tên tệp ảnh thẻ trong thư mục Data/Images).
    /// </summary>
    public class SinhVien
    {
        public const string DinhDangNgay = "dd/MM/yyyy";

        public string MSSV { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime NgaySinh { get; set; }
        public string GioiTinh { get; set; } = string.Empty;
        public string MaLop { get; set; } = string.Empty;

        /// <summary>Tên tệp ảnh thẻ (ví dụ SV001.png), để trống nếu chưa có ảnh.</summary>
        public string Anh { get; set; } = string.Empty;

        public static bool IsValidMSSV(string? mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return false;
            }

            return Regex.IsMatch(mssv.Trim(), @"^SV[0-9]+$", RegexOptions.IgnoreCase);
        }

        public override string ToString() => $"{MSSV} - {HoTen}";

        public string ToDataLine()
        {
            return string.Join(';',
                MSSV,
                HoTen,
                NgaySinh == DateTime.MinValue ? string.Empty : NgaySinh.ToString(DinhDangNgay, CultureInfo.InvariantCulture),
                GioiTinh,
                MaLop,
                Anh);
        }

        public static bool TryParse(string? dong, out SinhVien? sinhVien)
        {
            sinhVien = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 5 || string.IsNullOrWhiteSpace(cot[0]))
            {
                return false;
            }

            // Bỏ qua các dòng tiêu đề / dữ liệu không hợp lệ (MSSV phải đúng định dạng SV...).
            if (!IsValidMSSV(cot[0].Trim()))
            {
                return false;
            }

            DateTime ngaySinh = DateTime.MinValue;
            if (!string.IsNullOrWhiteSpace(cot[2]))
            {
                DateTime.TryParseExact(cot[2].Trim(), DinhDangNgay, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out ngaySinh);
            }

            sinhVien = new SinhVien
            {
                MSSV = cot[0].Trim(),
                HoTen = cot[1].Trim(),
                NgaySinh = ngaySinh,
                GioiTinh = cot[3].Trim(),
                MaLop = cot[4].Trim(),
                Anh = cot.Length > 5 ? cot[5].Trim() : string.Empty
            };
            return true;
        }
    }
}
