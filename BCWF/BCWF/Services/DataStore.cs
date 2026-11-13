using BCWF.Common;
using BCWF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BCWF.Services
{
    /// <summary>
    /// Kho dữ liệu dùng chung của ứng dụng: giữ dữ liệu trong bộ nhớ và
    /// đọc/ghi các tệp .txt trong thư mục Data (đường dẫn lấy từ App.config).
    /// Tất cả các form thao tác trên <see cref="Instance"/> rồi gọi hàm Luu... tương ứng.
    /// </summary>
    public sealed class DataStore
    {
        public const string TepUsers = "users.txt";
        public const string TepKhoa = "khoa.txt";
        public const string TepLop = "lop.txt";
        public const string TepSinhVien = "sinhvien.txt";
        public const string TepMonHoc = "monhoc.txt";
        public const string TepDangKy = "dangky.txt";
        public const string TepLichHoc = "lichhoc.txt";

        private const string TieuDeUsers = "TenDangNhap;MatKhau;HoTen;VaiTro";
        private const string TieuDeKhoa = "MaKhoa;TenKhoa";
        private const string TieuDeLop = "MaLop;TenLop;MaKhoa";
        private const string TieuDeSinhVien = "MSSV;HoTen;NgaySinh;GioiTinh;MaLop;Anh";
        private const string TieuDeMonHoc = "MaMon;TenMon;SoTC;MaMonTienQuyet";
        private const string TieuDeDangKy = "MSSV;MaMon;HocKy;DiemQT;DiemGK;DiemCK";
        private const string TieuDeLichHoc = "MaMon;Thu;TietBatDau;SoTiet";

        private delegate bool HamDoc<T>(string? dong, out T? ketQua) where T : class;

        public static DataStore Instance { get; } = new DataStore();

        private DataStore()
        {
            DataFolder = XacDinhThuMucDuLieu();
        }

        /// <summary>Thư mục chứa dữ liệu (mặc định là ...\BCWF\BCWF\Data).</summary>
        public string DataFolder { get; }

        /// <summary>Thư mục ảnh thẻ sinh viên.</summary>
        public string ImagesFolder => Path.Combine(DataFolder, "Images");

        public List<User> Users { get; } = new List<User>();
        public List<Khoa> Khoas { get; } = new List<Khoa>();
        public List<Lop> Lops { get; } = new List<Lop>();
        public List<SinhVien> SinhViens { get; } = new List<SinhVien>();
        public List<MonHoc> MonHocs { get; } = new List<MonHoc>();
        public List<DangKy> DangKys { get; } = new List<DangKy>();
        public List<LichHoc> LichHocs { get; } = new List<LichHoc>();

        /// <summary>Đường dẫn đầy đủ của một tệp dữ liệu.</summary>
        public string LayDuongDan(string tenTep) => Path.Combine(DataFolder, tenTep);

        /// <summary>Đường dẫn ảnh thẻ theo tên tệp lưu trong sinhvien.txt.</summary>
        public string LayDuongDanAnh(string tenAnh)
        {
            return string.IsNullOrWhiteSpace(tenAnh)
                ? string.Empty
                : Path.Combine(ImagesFolder, tenAnh);
        }

        /// <summary>Nạp toàn bộ dữ liệu từ thư mục Data vào bộ nhớ. Gọi một lần khi mở form chính.</summary>
        public void LoadAll()
        {
            Directory.CreateDirectory(DataFolder);
            Directory.CreateDirectory(ImagesFolder);

            Nap(TepUsers, TieuDeUsers, Users, User.TryParse);
            Nap(TepKhoa, TieuDeKhoa, Khoas, Khoa.TryParse);
            Nap(TepLop, TieuDeLop, Lops, Lop.TryParse);
            Nap(TepSinhVien, TieuDeSinhVien, SinhViens, SinhVien.TryParse);
            Nap(TepMonHoc, TieuDeMonHoc, MonHocs, MonHoc.TryParse);
            Nap(TepDangKy, TieuDeDangKy, DangKys, DangKy.TryParse);
            Nap(TepLichHoc, TieuDeLichHoc, LichHocs, LichHoc.TryParse);
        }

        public void LuuUsers() => Luu(TepUsers, TieuDeUsers, Users, u => u.ToDataLine());
        public void LuuKhoa() => Luu(TepKhoa, TieuDeKhoa, Khoas, k => k.ToDataLine());
        public void LuuLop() => Luu(TepLop, TieuDeLop, Lops, l => l.ToDataLine());
        public void LuuSinhVien() => Luu(TepSinhVien, TieuDeSinhVien, SinhViens, sv => sv.ToDataLine());
        public void LuuMonHoc() => Luu(TepMonHoc, TieuDeMonHoc, MonHocs, m => m.ToDataLine());
        public void LuuDangKy() => Luu(TepDangKy, TieuDeDangKy, DangKys, dk => dk.ToDataLine());
        public void LuuLichHoc() => Luu(TepLichHoc, TieuDeLichHoc, LichHocs, lh => lh.ToDataLine());

        /// <summary>Ghi lại toàn bộ dữ liệu xuống tệp.</summary>
        public void SaveAll()
        {
            LuuUsers();
            LuuKhoa();
            LuuLop();
            LuuSinhVien();
            LuuMonHoc();
            LuuDangKy();
            LuuLichHoc();
        }

        private void Nap<T>(string tenTep, string tieuDe, List<T> dich, HamDoc<T> doc) where T : class
        {
            dich.Clear();
            string duongDan = LayDuongDan(tenTep);
            try
            {
                // Tự tạo tệp (kèm dòng tiêu đề) nếu chưa có để chương trình chạy được ngay.
                if (!File.Exists(duongDan))
                {
                    FileHelpers.GhiDong(duongDan, new[] { tieuDe });
                }

                int soDong = 0;
                int soDongLoi = 0;

                foreach (string dong in FileHelpers.DocDong(duongDan))
                {
                    soDong++;

                    // Bỏ qua dòng tiêu đề, không nạp thành bản ghi.
                    if (string.Equals(dong.Trim(), tieuDe, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (doc(dong, out T? banGhi) && banGhi != null)
                    {
                        dich.Add(banGhi);
                    }
                    else
                    {
                        soDongLoi++;
                    }
                }

                // Dòng sai định dạng bị bỏ qua nhưng vẫn ghi nhận vào log.txt.
                if (soDongLoi > 0)
                {
                    FileHelpers.GhiLog($"Bỏ qua {soDongLoi}/{soDong} dòng sai định dạng trong {tenTep}.");
                }
            }
            catch (Exception loi)
            {
                FileHelpers.GhiLog($"Lỗi đọc tệp {tenTep}: {loi.Message}");
                throw;
            }
        }

        private void Luu<T>(string tenTep, string tieuDe, IEnumerable<T> nguon, Func<T, string> sangDong)
        {
            var cacDong = new List<string> { tieuDe };
            cacDong.AddRange(nguon.Select(sangDong));
            try
            {
                FileHelpers.GhiDong(LayDuongDan(tenTep), cacDong);
            }
            catch (Exception loi)
            {
                FileHelpers.GhiLog($"Lỗi ghi tệp {tenTep}: {loi.Message}");
                throw;
            }
        }

        private static string XacDinhThuMucDuLieu()
        {
            string cauHinh = AppConfig.DataFolder;

            // 1. Đường dẫn tuyệt đối trong App.config.
            if (Path.IsPathRooted(cauHinh))
            {
                return cauHinh;
            }

            // 2. Tìm thư mục dữ liệu ở các cấp cha của thư mục chạy (bin\Debug\net10.0-windows -> ...\BCWF\BCWF\Data).
            var thuMuc = new DirectoryInfo(AppContext.BaseDirectory);
            for (int i = 0; i < 8 && thuMuc != null; i++, thuMuc = thuMuc.Parent)
            {
                string ungVien = Path.Combine(thuMuc.FullName, cauHinh);
                if (Directory.Exists(ungVien) && Directory.GetFiles(thuMuc.FullName, "*.csproj").Length > 0)
                {
                    return ungVien;
                }
            }

            // 3. Không tìm thấy thì dùng thư mục cạnh tệp chạy (khi phát hành ứng dụng).
            return Path.Combine(AppContext.BaseDirectory, cauHinh);
        }
    }
}
