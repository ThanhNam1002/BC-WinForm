namespace BCWF.Models
{
    /// <summary>
    /// Lịch học của môn (phục vụ kiểm tra trùng lịch - chức năng nâng cao).
    /// Định dạng tệp Data/lichhoc.txt: MaMon;Thu;TietBatDau;SoTiet
    /// </summary>
    public class LichHoc
    {
        public string MaMon { get; set; } = string.Empty;

        /// <summary>Thứ trong tuần (2 = thứ 2 ... 7 = thứ 7, 8 = chủ nhật).</summary>
        public int Thu { get; set; }

        /// <summary>Tiết bắt đầu (1..12).</summary>
        public int TietBatDau { get; set; }

        public int SoTiet { get; set; }

        public int TietKetThuc => TietBatDau + SoTiet - 1;

        public string ToDataLine() => $"{MaMon};{Thu};{TietBatDau};{SoTiet}";

        public static bool TryParse(string? dong, out LichHoc? lichHoc)
        {
            lichHoc = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 4
                || !int.TryParse(cot[1].Trim(), out int thu)
                || !int.TryParse(cot[2].Trim(), out int tietBatDau)
                || !int.TryParse(cot[3].Trim(), out int soTiet))
            {
                return false;
            }

            lichHoc = new LichHoc
            {
                MaMon = cot[0].Trim(),
                Thu = thu,
                TietBatDau = tietBatDau,
                SoTiet = soTiet
            };
            return true;
        }
    }
}
