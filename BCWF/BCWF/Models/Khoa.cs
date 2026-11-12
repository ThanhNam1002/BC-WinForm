namespace BCWF.Models
{
    /// <summary>
    /// Một khoa trong danh mục khoa.
    /// Định dạng tệp Data/khoa.txt: MaKhoa;TenKhoa
    /// </summary>
    public class Khoa
    {
        public string MaKhoa { get; set; } = string.Empty;
        public string TenKhoa { get; set; } = string.Empty;

        public override string ToString() => $"{MaKhoa} - {TenKhoa}";

        public string ToDataLine() => $"{MaKhoa};{TenKhoa}";

        public static bool TryParse(string? dong, out Khoa? khoa)
        {
            khoa = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 2 || string.IsNullOrWhiteSpace(cot[0]))
            {
                return false;
            }

            khoa = new Khoa { MaKhoa = cot[0].Trim(), TenKhoa = cot[1].Trim() };
            return true;
        }
    }
}
