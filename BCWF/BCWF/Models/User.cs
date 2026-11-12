namespace BCWF.Models
{
    /// <summary>
    /// Tài khoản đăng nhập của phần mềm.
    /// Định dạng tệp Data/users.txt: TenDangNhap;MatKhau;HoTen;VaiTro
    /// </summary>
    public class User
    {
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;

        public override string ToString() => $"{TenDangNhap} - {HoTen}";

        public string ToDataLine() => $"{TenDangNhap};{MatKhau};{HoTen};{VaiTro}";

        public static bool TryParse(string? dong, out User? user)
        {
            user = null;
            if (string.IsNullOrWhiteSpace(dong))
            {
                return false;
            }

            string[] cot = dong.Split(';');
            if (cot.Length < 2 || string.IsNullOrWhiteSpace(cot[0]))
            {
                return false;
            }

            user = new User
            {
                TenDangNhap = cot[0].Trim(),
                MatKhau = cot[1].Trim(),
                HoTen = cot.Length > 2 ? cot[2].Trim() : string.Empty,
                VaiTro = cot.Length > 3 ? cot[3].Trim() : string.Empty
            };
            return true;
        }
    }
}
