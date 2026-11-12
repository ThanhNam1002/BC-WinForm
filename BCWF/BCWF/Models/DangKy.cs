using System.Globalization;

namespace BCWF.Models
{
    /// <summary>
    /// Một lượt đăng ký học phần kèm điểm của sinh viên.
    /// Định dạng tệp Data/dangky.txt: MSSV;MaMon;HocKy;DiemQT;DiemGK;DiemCK
    /// Điểm để trống khi chưa nhập.
    /// </summary>
    public class DangKy
    {
        public string MSSV { get; set; } = string.Empty;
        public string MaMon { get; set; } = string.Empty;
        public string HocKy { get; set; } = string.Empty;

        public double? DiemQT { get; set; }
        public double? DiemGK { get; set; }
        public double? DiemCK { get; set; }

        /// <summary>Đã nhập đủ cả 3 đầu điểm hay chưa.</summary>
        public bool DaNhapDiem => DiemQT.HasValue && DiemGK.HasValue && DiemCK.HasValue;

        public override string ToString() => $"{MSSV} - {MaMon} - {HocKy}";

        public string ToDataLine()
        {
            return string.Join(';',
                MSSV,
                MaMon,
                HocKy,
                DinhDangDiem(DiemQT),
                DinhDangDiem(DiemGK),
                DinhDangDiem(DiemCK));
        }

        public static bool TryParse(string? dong, out DangKy? dangKy)
        {
            dangKy = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 3 || string.IsNullOrWhiteSpace(cot[0]) || string.IsNullOrWhiteSpace(cot[1]))
            {
                return false;
            }

            dangKy = new DangKy
            {
                MSSV = cot[0].Trim(),
                MaMon = cot[1].Trim(),
                HocKy = cot[2].Trim(),
                DiemQT = DocDiem(cot.Length > 3 ? cot[3] : null),
                DiemGK = DocDiem(cot.Length > 4 ? cot[4] : null),
                DiemCK = DocDiem(cot.Length > 5 ? cot[5] : null)
            };
            return true;
        }

        private static double? DocDiem(string? giaTri)
        {
            if (string.IsNullOrWhiteSpace(giaTri))
            {
                return null;
            }

            return double.TryParse(giaTri.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double diem)
                ? diem
                : null;
        }

        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }
    }
}
