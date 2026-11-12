using BCWF.Common;
using BCWF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace BCWF.Services
{
    /// <summary>//
    /// SV3 - Điểm.
    /// Tính điểm tổng kết theo trọng số 30% quá trình + 30% giữa kỳ + 40% cuối kỳ,
    /// xếp loại, tô đỏ môn rớt, điểm trung bình tích lũy và xuất bảng điểm ra CSV.
    /// </summary>
    public class DiemService
    {
        /// <summary>Trọng số điểm quá trình.</summary>
        public const double TrongSoQT = 0.3;

        /// <summary>Trọng số điểm giữa kỳ.</summary>
        public const double TrongSoGK = 0.3;

        /// <summary>Trọng số điểm cuối kỳ.</summary>
        public const double TrongSoCK = 0.4;

        /// <summary>Điểm tổng kết dưới ngưỡng này thì bị rớt môn.</summary>
        public const double DiemRot = 4.0;

        private readonly DataStore _store = DataStore.Instance;

        /// <summary>Điểm tổng kết = 0.3*QT + 0.3*GK + 0.4*CK, null nếu chưa đủ 3 đầu điểm.</summary>
        public double? TinhDiemTongKet(DangKy dangKy)
        {
            if (dangKy == null || !dangKy.DaNhapDiem)
            {
                return null;
            }

            return TrongSoQT * dangKy.DiemQT!.Value
                 + TrongSoGK * dangKy.DiemGK!.Value
                 + TrongSoCK * dangKy.DiemCK!.Value;
        }

        /// <summary>Xếp loại theo điểm tổng kết (A/B/C/D/F hoặc Xuất sắc/Giỏi/...).</summary>
        public string XepLoai(double diemTongKet)
        {
            if (diemTongKet >= 9.0)
            {
                return "A";
            }

            if (diemTongKet >= 8.0)
            {
                return "B";
            }

            if (diemTongKet >= 7.0)
            {
                return "C";
            }

            if (diemTongKet >= 6.0)
            {
                return "D";
            }

            return "F";
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
            return TinhDiemTrungBinhTichLuy(mssv, null);
        }

        /// <summary>
        /// Điểm trung bình tích lũy, trong đó <paramref name="banThayThe"/> (nếu có) được dùng
        /// thay cho bản ghi đăng ký tương ứng trong bộ nhớ. Dùng khi sinh viên vừa sửa điểm
        /// trên ListView mà chưa bấm "Lưu điểm".
        /// </summary>
        public double TinhDiemTrungBinhTichLuy(string mssv, DangKy? banThayThe)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return 0;
            }

            var cacDangKy = _store.DangKys
                .Where(d => string.Equals(d.MSSV, mssv, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (banThayThe != null)
            {
                cacDangKy.RemoveAll(d =>
                    string.Equals(d.MaMon, banThayThe.MaMon, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(d.HocKy, banThayThe.HocKy, StringComparison.OrdinalIgnoreCase));

                cacDangKy.Add(banThayThe);
            }

            double tongDiem = 0;
            int tongTinChi = 0;

            foreach (DangKy dangKy in cacDangKy)
            {
                double? tongKet = TinhDiemTongKet(dangKy);
                if (!tongKet.HasValue)
                {
                    continue;
                }

                int soTC = _store.MonHocs.FirstOrDefault(m =>
                    string.Equals(m.MaMon, dangKy.MaMon, StringComparison.OrdinalIgnoreCase))?.SoTC ?? 0;

                if (soTC <= 0)
                {
                    // Môn không khai báo tín chỉ thì tính như 1 tín chỉ.
                    soTC = 1;
                }

                tongDiem += tongKet.Value * soTC;
                tongTinChi += soTC;
            }

            return tongTinChi == 0 ? 0 : tongDiem / tongTinChi;
        }

        /// <summary>Nâng cao: danh sách sinh viên bị cảnh báo học vụ (ĐTB tích lũy dưới ngưỡng trong App.config).</summary>
        public List<SinhVien> LayDanhSachCanhBao()
        {
            double nguong = AppConfig.DiemCanhBao;

            return _store.SinhViens
                .Where(sv => TinhDiemTrungBinhTichLuy(sv.MSSV) < nguong)
                .ToList();
        }

        /// <summary>Lưu 3 đầu điểm vừa sửa trực tiếp trên ListView.</summary>
        public void LuuDiem(string mssv, string maMon, string hocKy, double? diemQT, double? diemGK, double? diemCK)
        {
            CapNhatDiemTrongBoNho(mssv, maMon, hocKy, diemQT, diemGK, diemCK);
            _store.LuuDangKy();
        }

        /// <summary>
        /// Cập nhật điểm trong bộ nhớ (chưa ghi xuống tệp), dùng để tính lại
        /// tổng kết/xếp loại/điểm tích lũy ngay khi sửa trên ListView.
        /// </summary>
        public DangKy CapNhatDiemTrongBoNho(string mssv, string maMon, string hocKy, double? diemQT, double? diemGK, double? diemCK)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                throw new ArgumentException("MSSV không được để trống.", nameof(mssv));
            }

            if (string.IsNullOrWhiteSpace(maMon))
            {
                throw new ArgumentException("Mã môn không được để trống.", nameof(maMon));
            }

            DangKy? dangKy = TimDangKy(mssv, maMon, hocKy);
            if (dangKy == null)
            {
                dangKy = new DangKy { MSSV = mssv, MaMon = maMon, HocKy = hocKy };
                _store.DangKys.Add(dangKy);
            }

            dangKy.DiemQT = diemQT;
            dangKy.DiemGK = diemGK;
            dangKy.DiemCK = diemCK;

            return dangKy;
        }

        /// <summary>Xuất bảng điểm của lớp theo môn ra tệp CSV.</summary>
        public void XuatBangDiemCsv(string maLop, string maMon, string hocKy, string duongDanCsv)
        {
            if (string.IsNullOrWhiteSpace(duongDanCsv))
            {
                throw new ArgumentException("Đường dẫn tệp CSV không được để trống.", nameof(duongDanCsv));
            }

            var dong = new List<string[]>
            {
                new[] { "MSSV", "HoTen", "MaLop", "DiemQT", "DiemGK", "DiemCK", "TongKet", "XepLoai", "DTBTichLuy", "GhiChu" }
            };

            foreach (SinhVien sv in LaySinhVienTrongLop(maLop))
            {
                DangKy? dangKy = TimDangKy(sv.MSSV, maMon, hocKy);
                double? tongKet = dangKy == null ? null : TinhDiemTongKet(dangKy);
                double dtbTichLuy = TinhDiemTrungBinhTichLuy(sv.MSSV);

                string ghiChu = string.Empty;
                if (dangKy == null)
                {
                    ghiChu = "Chưa đăng ký";
                }
                else if (LaMonRot(dangKy))
                {
                    ghiChu = "Rớt môn";
                }

                dong.Add(new[]
                {
                    sv.MSSV,
                    sv.HoTen,
                    sv.MaLop,
                    DinhDangDiem(dangKy?.DiemQT),
                    DinhDangDiem(dangKy?.DiemGK),
                    DinhDangDiem(dangKy?.DiemCK),
                    tongKet.HasValue ? DinhDangDiem(tongKet) : string.Empty,
                    tongKet.HasValue ? XepLoai(tongKet.Value) : string.Empty,
                    DinhDangDiem(dtbTichLuy),
                    ghiChu
                });
            }

            FileHelpers.GhiCsv(duongDanCsv, dong);
        }

        #region Truy vấn dữ liệu phục vụ màn hình nhập điểm

        /// <summary>Sinh viên thuộc lớp (đã sắp xếp theo MSSV).</summary>
        public List<SinhVien> LaySinhVienTrongLop(string maLop)
        {
            if (string.IsNullOrWhiteSpace(maLop))
            {
                return new List<SinhVien>();
            }

            return _store.SinhViens
                .Where(sv => string.Equals(sv.MaLop, maLop, StringComparison.OrdinalIgnoreCase))
                .OrderBy(sv => sv.MSSV, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        /// <summary>Tìm bản ghi đăng ký của một sinh viên theo môn và học kỳ (null nếu chưa đăng ký).</summary>
        public DangKy? TimDangKy(string mssv, string maMon, string hocKy)
        {
            return _store.DangKys.FirstOrDefault(d =>
                string.Equals(d.MSSV, mssv, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.MaMon, maMon, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.HocKy, hocKy, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Các bản ghi đăng ký của lớp đối với môn và học kỳ đang chọn.</summary>
        public List<DangKy> LayDangKyLopMonHocKy(string maLop, string maMon, string hocKy)
        {
            var maLops = _store.SinhViens
                .Where(sv => string.Equals(sv.MaLop, maLop, StringComparison.OrdinalIgnoreCase))
                .Select(sv => sv.MSSV)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return _store.DangKys
                .Where(d => maLops.Contains(d.MSSV) &&
                            string.Equals(d.MaMon, maMon, StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(d.HocKy, hocKy, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>Bảng điểm cá nhân của một sinh viên (mọi môn đã đăng ký và đã có đủ điểm).</summary>
        public List<BangDiemCaNhanItem> LayBangDiemCaNhan(string mssv)
        {
            var ketQua = new List<BangDiemCaNhanItem>();

            foreach (DangKy dangKy in _store.DangKys.Where(d =>
                string.Equals(d.MSSV, mssv, StringComparison.OrdinalIgnoreCase)))
            {
                MonHoc? mon = _store.MonHocs.FirstOrDefault(m =>
                    string.Equals(m.MaMon, dangKy.MaMon, StringComparison.OrdinalIgnoreCase));

                double? tongKet = TinhDiemTongKet(dangKy);

                ketQua.Add(new BangDiemCaNhanItem
                {
                    MaMon = dangKy.MaMon,
                    TenMon = mon?.TenMon ?? string.Empty,
                    SoTC = mon?.SoTC ?? 0,
                    HocKy = dangKy.HocKy,
                    DiemQT = dangKy.DiemQT,
                    DiemGK = dangKy.DiemGK,
                    DiemCK = dangKy.DiemCK,
                    DiemTongKet = tongKet,
                    XepLoai = tongKet.HasValue ? XepLoai(tongKet.Value) : string.Empty,
                    LaMonRot = LaMonRot(dangKy)
                });
            }

            return ketQua
                .OrderBy(i => i.HocKy, StringComparer.OrdinalIgnoreCase)
                .ThenBy(i => i.MaMon, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        #endregion

        private static string DinhDangDiem(double? diem)
        {
            return diem.HasValue ? diem.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
        }
    }

    /// <summary>Một dòng trong bảng điểm cá nhân của sinh viên.</summary>
    public class BangDiemCaNhanItem
    {
        public string MaMon { get; set; } = string.Empty;
        public string TenMon { get; set; } = string.Empty;
        public int SoTC { get; set; }
        public string HocKy { get; set; } = string.Empty;
        public double? DiemQT { get; set; }
        public double? DiemGK { get; set; }
        public double? DiemCK { get; set; }
        public double? DiemTongKet { get; set; }
        public string XepLoai { get; set; } = string.Empty;
        public bool LaMonRot { get; set; }
    }
}
