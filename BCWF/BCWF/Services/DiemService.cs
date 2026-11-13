using BCWF.Common;
using BCWF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BCWF.Services
{
    /// <summary>
    /// SV3 - Điểm.
    /// Tính điểm tổng kết theo trọng số 30% quá trình + 30% giữa kỳ + 40% cuối kỳ,
    /// xếp loại, tô đỏ môn rớt, điểm trung bình tích lũy và xuất bảng điểm ra CSV.
    /// </summary>
    public class DiemService
    {
        private readonly DataStore _store = DataStore.Instance;

        /// <summary>Trọng số điểm quá trình (30%).</summary>
        public const double TrongSoQT = 0.3;

        /// <summary>Trọng số điểm giữa kỳ (30%).</summary>
        public const double TrongSoGK = 0.3;

        /// <summary>Trọng số điểm cuối kỳ (40%).</summary>
        public const double TrongSoCK = 0.4;

        /// <summary>Điểm tổng kết dưới ngưỡng này thì bị rớt môn.</summary>
        public const double DiemRot = 4.0;

        #region Tính điểm

        /// <summary>Điểm tổng kết = 0.3*QT + 0.3*GK + 0.4*CK, null nếu chưa đủ 3 đầu điểm.</summary>
        public double? TinhDiemTongKet(DangKy dangKy)
        {
            if (dangKy == null || !dangKy.DaNhapDiem)
            {
                return null;
            }

            double tongKet = dangKy.DiemQT!.Value * TrongSoQT
                          + dangKy.DiemGK!.Value * TrongSoGK
                          + dangKy.DiemCK!.Value * TrongSoCK;

            return Math.Round(tongKet, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>Xếp loại theo điểm tổng kết (A/B/C/D/F hoặc Xuất sắc/Giỏi/...).</summary>
        public string XepLoai(double diemTongKet)
        {
            if (diemTongKet >= 8.5)
            {
                return "A";
            }

            if (diemTongKet >= 7.0)
            {
                return "B";
            }

            if (diemTongKet >= 5.5)
            {
                return "C";
            }

            if (diemTongKet >= DiemRot)
            {
                return "D";
            }

            return "F";
        }

        /// <summary>Mô tả tiếng Việt của xếp loại (dùng khi xuất CSV/báo cáo).</summary>
        public string MoTaXepLoai(double diemTongKet)
        {
            switch (XepLoai(diemTongKet))
            {
                case "A":
                    return "Xuất sắc";
                case "B":
                    return "Giỏi";
                case "C":
                    return "Trung bình";
                case "D":
                    return "Yếu";
                default:
                    return "Kém";
            }
        }

        /// <summary>Môn bị rớt (điểm tổng kết &lt; 4) để tô đỏ trên ListView.</summary>
        public bool LaMonRot(DangKy dangKy)
        {
            double? tongKet = TinhDiemTongKet(dangKy);
            return tongKet.HasValue && tongKet.Value < DiemRot;
        }

        /// <summary>Điểm trung bình tích lũy của sinh viên (tính theo tín chỉ các môn đã có điểm).</summary>
        public double TinhDiemTrungBinhTichLuy(string mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return 0;
            }

            double tongDiem = 0;
            int tongTinChi = 0;

            foreach (DangKy dangKy in _store.DangKys.Where(dk =>
                string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase)))
            {
                double? tongKet = TinhDiemTongKet(dangKy);
                if (!tongKet.HasValue)
                {
                    continue;
                }

                int soTC = LaySoTinChi(dangKy.MaMon);
                tongDiem += tongKet.Value * soTC;
                tongTinChi += soTC;
            }

            if (tongTinChi == 0)
            {
                return 0;
            }

            return Math.Round(tongDiem / tongTinChi, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Sinh viên đã có ít nhất một môn nhập đủ 3 đầu điểm hay chưa
        /// (chưa có thì chưa có ĐTB tích lũy để đánh giá).
        /// </summary>
        public bool CoDiemTichLuy(string mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return false;
            }

            return _store.DangKys.Any(dk =>
                string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase) && dk.DaNhapDiem);
        }

        /// <summary>
        /// Nội dung cột "Ghi chú" của bảng điểm:
        /// rớt môn / chưa đăng ký / "Đạt" khi ĐTB tích lũu đạt yêu cầu / "Chưa đạt".
        /// </summary>
        public string LayGhiChu(DangKy? dangKy, double dtbTichLuy, bool coDiemTichLuy)
        {
            if (dangKy == null)
            {
                return "Chưa đăng ký";
            }

            if (LaMonRot(dangKy))
            {
                return "Rớt môn";
            }

            if (!coDiemTichLuy)
            {
                return string.Empty;
            }

            return dtbTichLuy >= AppConfig.DiemDatYeuCau ? "Đạt" : "Chưa đạt";
        }

        #endregion

        #region Cảnh báo học vụ

        /// <summary>Nâng cao: danh sách sinh viên bị cảnh báo học vụ (ĐTB tích lũy dưới ngưỡng trong App.config).</summary>
        public List<SinhVien> LayDanhSachCanhBao()
        {
            double nguong = AppConfig.DiemCanhBao;

            // Chỉ xét sinh viên đã có ít nhất một môn nhập đủ 3 đầu điểm.
            // Sinh viên chưa có điểm (hoặc mới nhập một phần) chưa đủ căn cứ để cảnh báo.
            var coDiem = new HashSet<string>(
                _store.DangKys.Where(dk => dk.DaNhapDiem).Select(dk => dk.MSSV),
                StringComparer.OrdinalIgnoreCase);

            return _store.SinhViens
                .Where(sv => coDiem.Contains(sv.MSSV)
                          && TinhDiemTrungBinhTichLuy(sv.MSSV) < nguong)
                .OrderBy(sv => sv.MSSV)
                .ToList();
        }

        #endregion

        #region Lưu điểm

        /// <summary>Lưu 3 đầu điểm vừa sửa trực tiếp trên ListView.</summary>
        public void LuuDiem(string mssv, string maMon, string hocKy, double? diemQT, double? diemGK, double? diemCK)
        {
            LuuDiem(new[] { new DiemCanNap(mssv, maMon, hocKy, diemQT, diemGK, diemCK) });
        }

        /// <summary>Lưu nhiều bản ghi điểm rồi ghi tệp dangky.txt một lần.</summary>
        public void LuuDiem(IEnumerable<DiemCanNap> danhSach)
        {
            if (danhSach == null)
            {
                throw new ArgumentNullException(nameof(danhSach));
            }

            bool coThayDoi = false;

            foreach (DiemCanNap diem in danhSach)
            {
                if (diem == null)
                {
                    continue;
                }

                KiemTraDiem(diem.DiemQT, nameof(diem.DiemQT));
                KiemTraDiem(diem.DiemGK, nameof(diem.DiemGK));
                KiemTraDiem(diem.DiemCK, nameof(diem.DiemCK));

                DangKy dangKy = LayOrTaoDangKy(diem.MSSV, diem.MaMon, diem.HocKy);
                dangKy.DiemQT = diem.DiemQT;
                dangKy.DiemGK = diem.DiemGK;
                dangKy.DiemCK = diem.DiemCK;
                coThayDoi = true;
            }

            if (coThayDoi)
            {
                _store.LuuDangKy();
            }
        }

        /// <summary>
        /// Lấy bản ghi đăng ký của sinh viên theo môn/học kỳ, tạo mới nếu chưa có
        /// (bản ghi mới chỉ nằm trong bộ nhớ, chỉ ghi xuống tệp khi gọi <see cref="LuuDiem"/>).
        /// </summary>
        public DangKy LayOrTaoDangKy(string mssv, string maMon, string hocKy)
        {
            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(maMon))
            {
                throw new ArgumentException("MSSV và mã môn không được để trống.");
            }

            string hk = string.IsNullOrWhiteSpace(hocKy) ? "HK1" : hocKy.Trim();

            DangKy? dangKy = TimDangKy(mssv, maMon, hk);
            if (dangKy != null)
            {
                return dangKy;
            }

            dangKy = new DangKy
            {
                MSSV = mssv.Trim(),
                MaMon = maMon.Trim(),
                HocKy = hk
            };

            _store.DangKys.Add(dangKy);
            return dangKy;
        }

        #endregion

        #region Xuất CSV

        /// <summary>Xuất bảng điểm của lớp theo môn ra tệp CSV.</summary>
        public void XuatBangDiemCsv(string maLop, string maMon, string hocKy, string duongDanCsv)
        {
            if (string.IsNullOrWhiteSpace(duongDanCsv))
            {
                throw new ArgumentException("Đường dẫn tệp CSV không được để trống.");
            }

            var cacDong = new List<string[]>
            {
                new[]
                {
                    "MSSV", "Họ tên", "Lớp", "Môn học", "Học kỳ",
                    "Điểm QT", "Điểm GK", "Điểm CK", "Tổng kết", "Xếp loại", "ĐTB tích lũy", "Ghi chú"
                }
            };

            foreach (SinhVien sv in LaySinhVienLop(maLop))
            {
                DangKy? dangKy = TimDangKy(sv.MSSV, maMon, hocKy);
                double? tongKet = dangKy == null ? null : TinhDiemTongKet(dangKy);
                double dtbTichLuy = TinhDiemTrungBinhTichLuy(sv.MSSV);
                bool coDiemTichLuy = CoDiemTichLuy(sv.MSSV);

                cacDong.Add(new[]
                {
                    sv.MSSV,
                    sv.HoTen,
                    sv.MaLop,
                    LayTenMon(maMon),
                    hocKy,
                    DinhDangDiem(dangKy?.DiemQT),
                    DinhDangDiem(dangKy?.DiemGK),
                    DinhDangDiem(dangKy?.DiemCK),
                    tongKet.HasValue ? tongKet.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty,
                    tongKet.HasValue ? XepLoai(tongKet.Value) : string.Empty,
                    coDiemTichLuy ? dtbTichLuy.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty,
                    LayGhiChu(dangKy, dtbTichLuy, coDiemTichLuy)
                });
            }

            FileHelpers.GhiCsv(duongDanCsv, cacDong);
        }

        /// <summary>Xuất bảng điểm cá nhân (toàn bộ môn đã đăng ký) của một sinh viên ra tệp CSV.</summary>
        public void XuatBangDiemCaNhanCsv(string mssv, string duongDanCsv)
        {
            if (string.IsNullOrWhiteSpace(duongDanCsv))
            {
                throw new ArgumentException("Đường dẫn tệp CSV không được để trống.");
            }

            var cacDong = new List<string[]>
            {
                new[]
                {
                    "MSSV", "Họ tên", "Môn học", "Tín chỉ", "Học kỳ",
                    "Điểm QT", "Điểm GK", "Điểm CK", "Tổng kết", "Xếp loại", "Ghi chú"
                }
            };

            foreach (DangKy dangKy in LayBangDiemCaNhan(mssv))
            {
                double? tongKet = TinhDiemTongKet(dangKy);
                bool rot = LaMonRot(dangKy);

                cacDong.Add(new[]
                {
                    dangKy.MSSV,
                    LayTenSinhVien(dangKy.MSSV),
                    LayTenMon(dangKy.MaMon),
                    LaySoTinChi(dangKy.MaMon).ToString(CultureInfo.InvariantCulture),
                    dangKy.HocKy,
                    DinhDangDiem(dangKy.DiemQT),
                    DinhDangDiem(dangKy.DiemGK),
                    DinhDangDiem(dangKy.DiemCK),
                    tongKet.HasValue ? tongKet.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty,
                    tongKet.HasValue ? XepLoai(tongKet.Value) : string.Empty,
                    rot ? "Rớt môn" : string.Empty
                });
            }

            FileHelpers.GhiCsv(duongDanCsv, cacDong);
        }

        #endregion

        #region Truy vấn dữ liệu

        /// <summary>Các sinh viên của một lớp (sắp xếp theo MSSV).</summary>
        public List<SinhVien> LaySinhVienLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                return new List<SinhVien>();
            }

            return _store.SinhViens
                .Where(sv => string.Equals(sv.MaLop, maLop, StringComparison.OrdinalIgnoreCase))
                .OrderBy(sv => sv.MSSV)
                .ToList();
        }

        /// <summary>Tìm bản ghi đăng ký của sinh viên theo môn và học kỳ.</summary>
        public DangKy? TimDangKy(string mssv, string maMon, string hocKy)
        {
            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(maMon))
            {
                return null;
            }

            return _store.DangKys.FirstOrDefault(dk =>
                string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dk.MaMon, maMon, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dk.HocKy, hocKy ?? string.Empty, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Bảng điểm cá nhân: toàn bộ môn sinh viên đã đăng ký (có điểm hoặc chưa).</summary>
        public List<DangKy> LayBangDiemCaNhan(string mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return new List<DangKy>();
            }

            return _store.DangKys
                .Where(dk => string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase))
                .OrderBy(dk => dk.HocKy)
                .ThenBy(dk => dk.MaMon)
                .ToList();
        }

        /// <summary>Số tín chỉ của một môn học (0 nếu không tìm thấy).</summary>
        public int LaySoTinChi(string maMon)
        {
            if (string.IsNullOrWhiteSpace(maMon))
            {
                return 0;
            }

            MonHoc? monHoc = _store.MonHocs.FirstOrDefault(m =>
                string.Equals(m.MaMon, maMon, StringComparison.OrdinalIgnoreCase));

            return monHoc?.SoTC ?? 0;
        }

        /// <summary>Tên môn học theo mã (trả về chuỗi rỗng nếu không tìm thấy).</summary>
        public string LayTenMon(string maMon)
        {
            if (string.IsNullOrWhiteSpace(maMon))
            {
                return string.Empty;
            }

            MonHoc? monHoc = _store.MonHocs.FirstOrDefault(m =>
                string.Equals(m.MaMon, maMon, StringComparison.OrdinalIgnoreCase));

            return monHoc?.TenMon ?? string.Empty;
        }

        /// <summary>Tên sinh viên theo MSSV (trả về chuỗi rỗng nếu không tìm thấy).</summary>
        public string LayTenSinhVien(string mssv)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return string.Empty;
            }

            SinhVien? sv = _store.SinhViens.FirstOrDefault(s =>
                string.Equals(s.MSSV, mssv, StringComparison.OrdinalIgnoreCase));

            return sv?.HoTen ?? string.Empty;
        }

        #endregion

        private static void KiemTraDiem(double? diem, string tenThamSo)
        {
            if (diem.HasValue && (diem.Value < 0 || diem.Value > 10))
            {
                throw new ArgumentOutOfRangeException(tenThamSo, "Điểm phải nằm trong khoảng [0, 10].");
            }
        }

        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>Một bản ghi điểm cần lưu (MSSV + môn + học kỳ + 3 đầu điểm).</summary>
        public class DiemCanNap
        {
            public DiemCanNap(string mssv, string maMon, string hocKy, double? diemQT, double? diemGK, double? diemCK)
            {
                MSSV = mssv;
                MaMon = maMon;
                HocKy = hocKy;
                DiemQT = diemQT;
                DiemGK = diemGK;
                DiemCK = diemCK;
            }

            public string MSSV { get; }
            public string MaMon { get; }
            public string HocKy { get; }
            public double? DiemQT { get; }
            public double? DiemGK { get; }
            public double? DiemCK { get; }
        }
    }
}
