namespace BCWF.Models
{
    /// <summary>
    /// Một lớp thuộc một khoa.
    /// Định dạng tệp Data/lop.txt: MaLop;TenLop;MaKhoa
    /// </summary>
    public class Lop
    {
        public string MaLop { get; set; } = string.Empty;
        public string TenLop { get; set; } = string.Empty;
        public string MaKhoa { get; set; } = string.Empty;

        public override string ToString() => $"{MaLop} - {TenLop}";

        public string ToDataLine() => $"{MaLop};{TenLop};{MaKhoa}";

        public static bool TryParse(string? dong, out Lop? lop)
        {
            lop = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 3 || string.IsNullOrWhiteSpace(cot[0]))
            {
                return false;
            }

            lop = new Lop { MaLop = cot[0].Trim(), TenLop = cot[1].Trim(), MaKhoa = cot[2].Trim() };
            return true;
        }
    }
}
