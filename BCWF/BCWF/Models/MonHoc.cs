namespace BCWF.Models
{
    /// <summary>
    /// Một môn học.
    /// Định dạng tệp Data/monhoc.txt: MaMon;TenMon;SoTC;MaMonTienQuyet
    /// (cột MaMonTienQuyet là phần bổ sung cho chức năng nâng cao, có thể để trống).
    /// </summary>
    public class MonHoc
    {
        public string MaMon { get; set; } = string.Empty;
        public string TenMon { get; set; } = string.Empty;
        public int SoTC { get; set; }

        /// <summary>Mã môn phải đạt trước khi được đăng ký môn này (rỗng nếu không có).</summary>
        public string MaMonTienQuyet { get; set; } = string.Empty;

        public override string ToString() => $"{MaMon} - {TenMon} ({SoTC} TC)";

        public string ToDataLine() => $"{MaMon};{TenMon};{SoTC};{MaMonTienQuyet}";

        public static bool TryParse(string? dong, out MonHoc? monHoc)
        {
            monHoc = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 3 || string.IsNullOrWhiteSpace(cot[0]))
            {
                return false;
            }

            monHoc = new MonHoc
            {
                MaMon = cot[0].Trim(),
                TenMon = cot[1].Trim(),
                SoTC = int.TryParse(cot[2].Trim(), out int soTC) ? soTC : 0,
                MaMonTienQuyet = cot.Length > 3 ? cot[3].Trim() : string.Empty
            };
            return true;
        }
    }
}
