using BCWF.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BCWF.Services
{
    /// <summary>
    /// SV1 - Sinh viên &amp; lớp.
    /// </summary>
    public class SinhVienService
    {
        private readonly DataStore _store = DataStore.Instance;

        public List<SinhVien> LayTheoKhoa(string maKhoa)
        {
            if (string.IsNullOrWhiteSpace(maKhoa))
            {
                return _store.SinhViens.ToList();
            }

            var maLops = _store.Lops
                .Where(l => string.Equals(l.MaKhoa, maKhoa, StringComparison.OrdinalIgnoreCase))
                .Select(l => l.MaLop)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return _store.SinhViens
                .Where(sv => maLops.Contains(sv.MaLop))
                .ToList();
        }

        public List<SinhVien> LayTheoMaLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                return new List<SinhVien>();
            }

            return _store.SinhViens
                .Where(sv => string.Equals(sv.MaLop, maLop, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public bool KiemTraTrungMSSV(string mssv, string? mssvDangSua = null)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return false;
            }

            return _store.SinhViens.Any(sv =>
                string.Equals(sv.MSSV, mssv, StringComparison.OrdinalIgnoreCase) &&
                (string.IsNullOrWhiteSpace(mssvDangSua) ||
                 !string.Equals(sv.MSSV, mssvDangSua, StringComparison.OrdinalIgnoreCase)));
        }

        public void Them(SinhVien sinhVien)
        {
            if (sinhVien == null)
            {
                throw new ArgumentNullException(nameof(sinhVien));
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MSSV))
            {
                throw new ArgumentException("MSSV không được để trống.", nameof(sinhVien));
            }

            if (string.IsNullOrWhiteSpace(sinhVien.HoTen))
            {
                throw new ArgumentException("Họ tên không được để trống.", nameof(sinhVien));
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MaLop))
            {
                throw new ArgumentException("Mã lớp không được để trống.", nameof(sinhVien));
            }

            if (KiemTraTrungMSSV(sinhVien.MSSV))
            {
                throw new InvalidOperationException($"MSSV '{sinhVien.MSSV}' đã tồn tại.");
            }

            _store.SinhViens.Add(sinhVien);
            _store.LuuSinhVien();
        }

        public void CapNhat(SinhVien sinhVien)
        {
            if (sinhVien == null)
            {
                throw new ArgumentNullException(nameof(sinhVien));
            }

            int viTri = _store.SinhViens.FindIndex(sv =>
                string.Equals(sv.MSSV, sinhVien.MSSV, StringComparison.OrdinalIgnoreCase));

            if (viTri < 0)
            {
                throw new InvalidOperationException($"Không tìm thấy sinh viên có MSSV '{sinhVien.MSSV}'.");
            }

            _store.SinhViens[viTri] = sinhVien;
            _store.LuuSinhVien();
        }

        /// <summary>
        /// Đổi MSSV của một sinh viên và cập nhật toàn bộ đăng ký liên quan.
        /// Thực hiện trên bộ nhớ rồi mới ghi tệp, tránh để lại cả bản ghi cũ lẫn mới.
        /// </summary>
        public void DoiMSSV(string mssvCu, SinhVien sinhVien)
        {
            if (sinhVien == null)
            {
                throw new ArgumentNullException(nameof(sinhVien));
            }

            if (string.IsNullOrWhiteSpace(mssvCu))
            {
                throw new ArgumentException("MSSV cũ không được để trống.", nameof(mssvCu));
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MSSV))
            {
                throw new ArgumentException("MSSV không được để trống.", nameof(sinhVien));
            }

            if (string.IsNullOrWhiteSpace(sinhVien.HoTen))
            {
                throw new ArgumentException("Họ tên không được để trống.", nameof(sinhVien));
            }

            if (string.IsNullOrWhiteSpace(sinhVien.MaLop))
            {
                throw new ArgumentException("Mã lớp không được để trống.", nameof(sinhVien));
            }

            int viTri = _store.SinhViens.FindIndex(sv =>
                string.Equals(sv.MSSV, mssvCu, StringComparison.OrdinalIgnoreCase));

            if (viTri < 0)
            {
                throw new InvalidOperationException($"Không tìm thấy sinh viên có MSSV '{mssvCu}'.");
            }

            if (KiemTraTrungMSSV(sinhVien.MSSV, mssvCu))
            {
                throw new InvalidOperationException($"MSSV '{sinhVien.MSSV}' đã tồn tại.");
            }

            _store.SinhViens[viTri] = sinhVien;

            foreach (DangKy dangKy in _store.DangKys.Where(dk =>
                string.Equals(dk.MSSV, mssvCu, StringComparison.OrdinalIgnoreCase)))
            {
                dangKy.MSSV = sinhVien.MSSV;
            }

            _store.LuuSinhVien();
            _store.LuuDangKy();
        }

        public void Xoa(string mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return;
            }

            _store.SinhViens.RemoveAll(sv =>
                string.Equals(sv.MSSV, mssv, StringComparison.OrdinalIgnoreCase));

            _store.DangKys.RemoveAll(dk =>
                string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase));

            _store.LuuSinhVien();
            _store.LuuDangKy();
        }

        public int NhapTuCsv(string duongDanCsv)
        {
            if (string.IsNullOrWhiteSpace(duongDanCsv) || !File.Exists(duongDanCsv))
            {
                return 0;
            }

            var cacDong = FileHelpers.DocCsv(duongDanCsv, false);
            int batDau = 0;
            if (cacDong.Count > 0 && cacDong[0].Length > 0 &&
                string.Equals(cacDong[0][0].Trim(), "MSSV", StringComparison.OrdinalIgnoreCase))
            {
                batDau = 1;
            }

            int soThanhCong = 0;

            for (int i = batDau; i < cacDong.Count; i++)
            {
                string[] cot = cacDong[i];
                int soDong = i + 1;

                if (cot.Length < 5)
                {
                    FileHelpers.GhiLog($"Bỏ qua dòng {soDong} trong tệp CSV nhập sinh viên: không đủ 5 cột.");
                    continue;
                }

                string mssv = cot[0].Trim();
                string hoTen = cot[1].Trim();
                string maLop = cot[4].Trim();

                if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(maLop))
                {
                    FileHelpers.GhiLog($"Bỏ qua dòng {soDong} trong tệp CSV nhập sinh viên: thiếu MSSV/họ tên/lớp.");
                    continue;
                }

                // MSSV sai định dạng sẽ không đọc lại được ở lần chạy sau, nên bỏ qua ngay từ lúc nhập.
                if (!SinhVien.IsValidMSSV(mssv))
                {
                    FileHelpers.GhiLog($"Bỏ qua dòng {soDong} trong tệp CSV nhập sinh viên: MSSV '{mssv}' sai định dạng.");
                    continue;
                }

                if (KiemTraTrungMSSV(mssv))
                {
                    FileHelpers.GhiLog($"Bỏ qua dòng {soDong} trong tệp CSV nhập sinh viên: MSSV '{mssv}' đã tồn tại.");
                    continue;
                }

                DateTime ngaySinh = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(cot[2]))
                {
                    DateTime.TryParseExact(cot[2].Trim(), SinhVien.DinhDangNgay,
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out ngaySinh);
                }

                string anh = cot.Length > 5 ? cot[5].Trim() : string.Empty;

                var sinhVien = new SinhVien
                {
                    MSSV = mssv,
                    HoTen = hoTen,
                    NgaySinh = ngaySinh,
                    GioiTinh = cot[3].Trim(),
                    MaLop = maLop,
                    Anh = anh
                };

                _store.SinhViens.Add(sinhVien);
                soThanhCong++;
            }

            if (soThanhCong > 0)
            {
                _store.LuuSinhVien();
            }

            return soThanhCong;
        }

        public string LuuAnh(string mssv, string duongDanAnhNguon)
        {
            if (string.IsNullOrWhiteSpace(duongDanAnhNguon) || !File.Exists(duongDanAnhNguon))
            {
                throw new FileNotFoundException("Không tìm thấy tệp ảnh.", duongDanAnhNguon);
            }

            Directory.CreateDirectory(_store.ImagesFolder);

            string ma = string.IsNullOrWhiteSpace(mssv)
                ? Guid.NewGuid().ToString("N")
                : FileHelpers.ChuanHoaTenTep(mssv);

            string phanMoRong = Path.GetExtension(duongDanAnhNguon);
            if (string.IsNullOrWhiteSpace(phanMoRong))
            {
                phanMoRong = ".png";
            }

            string tenAnh = ma + phanMoRong;
            string dich = Path.Combine(_store.ImagesFolder, tenAnh);

            File.Copy(duongDanAnhNguon, dich, true);
            return tenAnh;
        }
    }
}