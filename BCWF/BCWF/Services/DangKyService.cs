using BCWF.Common;
using BCWF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BCWF.Services
{
    /// <summary>
    /// SV2 - Đăng ký học phần.
    /// Gồm quản lý môn học và đăng ký bằng cách chuyển môn giữa 2 ListBox,
    /// kiểm tra tổng tín chỉ trong [MinTC, MaxTC] và chống đăng ký trùng.
    /// </summary>
    public class DangKyService
    {
        private readonly DataStore _store = DataStore.Instance;
        private readonly DiemService _diemService = new DiemService();

        /// <summary>Điểm tổng kết tối thiểu để coi là đã đạt một môn.</summary>
        public const double DiemDat = 4.0;

        #region Quản lý môn học

        public List<MonHoc> LayTatCaMonHoc()
        {
            return _store.MonHocs.OrderBy(m => m.MaMon).ToList();
        }

        public void ThemMonHoc(MonHoc monHoc)
        {
            if (monHoc == null)
            {
                throw new ArgumentNullException(nameof(monHoc));
            }

            monHoc.MaMon = monHoc.MaMon.Trim();
            monHoc.TenMon = monHoc.TenMon.Trim();

            if (string.IsNullOrWhiteSpace(monHoc.MaMon))
            {
                throw new ArgumentException("Mã môn không được để trống.", nameof(monHoc));
            }

            if (string.IsNullOrWhiteSpace(monHoc.TenMon))
            {
                throw new ArgumentException("Tên môn không được để trống.", nameof(monHoc));
            }

            if (monHoc.SoTC < 1 || monHoc.SoTC > 10)
            {
                throw new ArgumentException("Số tín chỉ phải nằm trong khoảng 1 đến 10.", nameof(monHoc));
            }

            if (TonTaiMon(monHoc.MaMon))
            {
                throw new InvalidOperationException($"Mã môn '{monHoc.MaMon}' đã tồn tại.");
            }

            monHoc.MaMonTienQuyet = KiemTraMaTienQuyetHopLe(monHoc.MaMonTienQuyet, monHoc.MaMon);

            _store.MonHocs.Add(monHoc);
            _store.LuuMonHoc();
        }

        public void CapNhatMonHoc(MonHoc monHoc, string? maMonGoc = null)
        {
            if (monHoc == null)
            {
                throw new ArgumentNullException(nameof(monHoc));
            }

            monHoc.MaMon = monHoc.MaMon.Trim();
            monHoc.TenMon = monHoc.TenMon.Trim();

            if (string.IsNullOrWhiteSpace(monHoc.MaMon))
            {
                throw new ArgumentException("Mã môn không được để trống.", nameof(monHoc));
            }

            if (string.IsNullOrWhiteSpace(monHoc.TenMon))
            {
                throw new ArgumentException("Tên môn không được để trống.", nameof(monHoc));
            }

            if (monHoc.SoTC < 1 || monHoc.SoTC > 10)
            {
                throw new ArgumentException("Số tín chỉ phải nằm trong khoảng 1 đến 10.", nameof(monHoc));
            }

            string maGoc = string.IsNullOrWhiteSpace(maMonGoc) ? monHoc.MaMon : maMonGoc.Trim();

            int viTri = _store.MonHocs.FindIndex(m =>
                string.Equals(m.MaMon, maGoc, StringComparison.OrdinalIgnoreCase));
            if (viTri < 0)
            {
                throw new InvalidOperationException($"Không tìm thấy môn học '{maGoc}' để sửa.");
            }

            // Cho phép đổi mã, nhưng mã mới không được trùng với môn khác.
            if (!string.Equals(maGoc, monHoc.MaMon, StringComparison.OrdinalIgnoreCase) && TonTaiMon(monHoc.MaMon))
            {
                throw new InvalidOperationException($"Mã môn '{monHoc.MaMon}' đã tồn tại.");
            }

            monHoc.MaMonTienQuyet = KiemTraMaTienQuyetHopLe(monHoc.MaMonTienQuyet, monHoc.MaMon);

            bool doiMa = !string.Equals(maGoc, monHoc.MaMon, StringComparison.OrdinalIgnoreCase);

            // Nếu đổi mã thì cập nhật cả mã môn tiên quyết của các môn trỏ đến mã cũ,
            // đồng thời chuyển các đăng ký và lịch học sang mã mới để không mất liên kết.
            if (doiMa)
            {
                foreach (MonHoc monKhac in _store.MonHocs.Where(m =>
                    string.Equals(m.MaMonTienQuyet, maGoc, StringComparison.OrdinalIgnoreCase)))
                {
                    monKhac.MaMonTienQuyet = monHoc.MaMon;
                }

                foreach (DangKy dangKy in _store.DangKys.Where(dk =>
                    string.Equals(dk.MaMon, maGoc, StringComparison.OrdinalIgnoreCase)))
                {
                    dangKy.MaMon = monHoc.MaMon;
                }

                foreach (LichHoc lichHoc in _store.LichHocs.Where(lh =>
                    string.Equals(lh.MaMon, maGoc, StringComparison.OrdinalIgnoreCase)))
                {
                    lichHoc.MaMon = monHoc.MaMon;
                }
            }

            _store.MonHocs[viTri] = monHoc;
            _store.LuuMonHoc();

            if (doiMa)
            {
                _store.LuuDangKy();
                _store.LuuLichHoc();
            }
        }

        public void XoaMonHoc(string maMon)
        {
            maMon = (maMon ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(maMon))
            {
                return;
            }

            if (_store.DangKys.Any(dk => string.Equals(dk.MaMon, maMon, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    $"Không thể xóa môn '{maMon}' vì đang có sinh viên đăng ký (hoặc có điểm) trong dangky.txt.");
            }

            _store.MonHocs.RemoveAll(m => string.Equals(m.MaMon, maMon, StringComparison.OrdinalIgnoreCase));
            _store.LichHocs.RemoveAll(lh => string.Equals(lh.MaMon, maMon, StringComparison.OrdinalIgnoreCase));

            // Gỡ môn này khỏi cột tiên quyết của các môn khác.
            foreach (MonHoc mon in _store.MonHocs.Where(m =>
                string.Equals(m.MaMonTienQuyet, maMon, StringComparison.OrdinalIgnoreCase)))
            {
                mon.MaMonTienQuyet = string.Empty;
            }

            _store.LuuMonHoc();
            _store.LuuLichHoc();
        }

        #endregion

        #region Đăng ký học phần

        /// <summary>Các môn sinh viên đã đăng ký trong học kỳ.</summary>
        public List<MonHoc> LayMonDaDangKy(string mssv, string hocKy)
        {
            if (string.IsNullOrWhiteSpace(mssv))
            {
                return new List<MonHoc>();
            }

            return _store.DangKys
                .Where(dk => string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase)
                          && string.Equals(dk.HocKy, hocKy ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .Select(dk => TimMon(dk.MaMon))
                .Where(m => m != null)
                .Cast<MonHoc>()
                .OrderBy(m => m.MaMon)
                .ToList();
        }

        /// <summary>Tổng số tín chỉ của danh sách môn đang chọn.</summary>
        public int TinhTongTinChi(IEnumerable<string> dsMaMon)
        {
            if (dsMaMon == null)
            {
                return 0;
            }

            return dsMaMon.Sum(ma => LaySoTC(ma));
        }

        /// <summary>Kiểm tra tổng tín chỉ có nằm trong giới hạn MinTC - MaxTC (đọc từ App.config).</summary>
        public bool KiemTraGioiHanTinChi(IEnumerable<string> dsMaMon, out string thongBao)
        {
            int tong = TinhTongTinChi(dsMaMon);
            int min = AppConfig.MinTC;
            int max = AppConfig.MaxTC;

            if (tong < min)
            {
                thongBao = $"Tổng tín chỉ đăng ký là {tong}, dưới mức tối thiểu {min} tín chỉ của học kỳ.";
                return false;
            }

            if (tong > max)
            {
                thongBao = $"Tổng tín chỉ đăng ký là {tong}, vượt mức tối đa {max} tín chỉ của học kỳ.";
                return false;
            }

            thongBao = string.Empty;
            return true;
        }

        /// <summary>Môn này đã có trong danh sách đăng ký của sinh viên trong học kỳ hay chưa.</summary>
        public bool DaDangKy(string mssv, string maMon, string hocKy)
        {
            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(maMon))
            {
                return false;
            }

            return _store.DangKys.Any(dk =>
                string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dk.MaMon, maMon, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dk.HocKy, hocKy ?? string.Empty, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>Sinh viên đã đạt môn (tổng kết &gt;= 4) trong học kỳ nào đó chưa.</summary>
        public bool DaDatMon(string mssv, string maMon)
        {
            return _store.DangKys
                .Where(dk => string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase)
                          && string.Equals(dk.MaMon, maMon, StringComparison.OrdinalIgnoreCase)
                          && dk.DaNhapDiem)
                .Any(dk => _diemService.TinhDiemTongKet(dk) >= DiemDat);
        }

        /// <summary>Nâng cao: kiểm tra môn tiên quyết đã đạt (điểm tổng kết &gt;= 4).</summary>
        public bool KiemTraTienQuyet(string mssv, string maMon, out string thongBao)
        {
            thongBao = string.Empty;

            MonHoc? mon = TimMon(maMon);
            if (mon == null)
            {
                thongBao = $"Không tìm thấy môn học '{maMon}'.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(mon.MaMonTienQuyet))
            {
                return true;
            }

            MonHoc? monTienQuyet = TimMon(mon.MaMonTienQuyet);
            string tenMonTQ = monTienQuyet?.TenMon ?? mon.MaMonTienQuyet;

            if (DaDatMon(mssv, mon.MaMonTienQuyet))
            {
                return true;
            }

            thongBao = $"Không thể đăng ký '{mon.TenMon} ({mon.MaMon})': phải đạt môn tiên quyết "
                + $"'{tenMonTQ} ({mon.MaMonTienQuyet})' trước (điểm tổng kết từ {DiemDat:0.#} trở lên).";
            return false;
        }

        /// <summary>Nâng cao: kiểm tra trùng thứ/tiết giữa các môn đăng ký (dùng lichhoc.txt).</summary>
        public bool KiemTraTrungLich(IEnumerable<string> dsMaMon, out string thongBao)
        {
            thongBao = string.Empty;
            if (dsMaMon == null)
            {
                return true;
            }

            var danhSach = dsMaMon
                .Where(ma => !string.IsNullOrWhiteSpace(ma))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            for (int i = 0; i < danhSach.Count; i++)
            {
                for (int j = i + 1; j < danhSach.Count; j++)
                {
                    foreach (LichHoc lh1 in LayLich(danhSach[i]))
                    {
                        foreach (LichHoc lh2 in LayLich(danhSach[j]))
                        {
                            if (lh1.Thu == lh2.Thu && lh1.TietBatDau <= lh2.TietKetThuc && lh2.TietBatDau <= lh1.TietKetThuc)
                            {
                                thongBao = $"Môn '{danhSach[i]}' và '{danhSach[j]}' trùng lịch học "
                                    + $"(thứ {lh1.Thu}, tiết {Math.Max(lh1.TietBatDau, lh2.TietBatDau)}–{Math.Min(lh1.TietKetThuc, lh2.TietKetThuc)}).";
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>Ghi nhận đăng ký một môn cho sinh viên.</summary>
        public void DangKy(string mssv, string maMon, string hocKy)
        {
            mssv = (mssv ?? string.Empty).Trim();
            maMon = (maMon ?? string.Empty).Trim();
            hocKy = (hocKy ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(mssv) || string.IsNullOrWhiteSpace(maMon))
            {
                throw new ArgumentException("MSSV và mã môn không được để trống.");
            }

            if (TimMon(maMon) == null)
            {
                throw new InvalidOperationException($"Không tìm thấy môn học '{maMon}'.");
            }

            if (DaDangKy(mssv, maMon, hocKy))
            {
                throw new InvalidOperationException(
                    $"Sinh viên '{mssv}' đã đăng ký môn '{maMon}' trong học kỳ '{hocKy}' rồi.");
            }

            _store.DangKys.Add(new DangKy
            {
                MSSV = mssv,
                MaMon = maMon,
                HocKy = string.IsNullOrWhiteSpace(hocKy) ? AppConfig.HocKyHienTai : hocKy
            });
            _store.LuuDangKy();
        }

        /// <summary>Hủy đăng ký một môn.</summary>
        public void HuyDangKy(string mssv, string maMon, string hocKy)
        {
            mssv = (mssv ?? string.Empty).Trim();
            maMon = (maMon ?? string.Empty).Trim();

            int soXoa = _store.DangKys.RemoveAll(dk =>
                string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dk.MaMon, maMon, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(dk.HocKy, hocKy ?? string.Empty, StringComparison.OrdinalIgnoreCase));

            if (soXoa > 0)
            {
                _store.LuuDangKy();
            }
        }

        /// <summary>
        /// Lưu danh sách đăng ký của một sinh viên trong học kỳ: đăng ký các môn mới,
        /// hủy các môn không còn trong danh sách. Chỉ ghi tệp dangky.txt một lần.
        /// </summary>
        public int LuuDanhSachDangKy(string mssv, string hocKy, IEnumerable<string> dsMaMon)
        {
            mssv = (mssv ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(mssv))
            {
                throw new ArgumentException("Chưa chọn sinh viên.", nameof(mssv));
            }

            var dsMoi = (dsMaMon ?? Array.Empty<string>())
                .Where(ma => !string.IsNullOrWhiteSpace(ma))
                .Select(ma => ma.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            // Không âm thầm bỏ qua việc hủy môn đã có điểm: báo lỗi để người dùng biết
            // và chủ động xử lý bảng điểm trước khi thay đổi đăng ký.
            var monCoDiemBiHuy = _store.DangKys
                .Where(dk => string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase)
                          && string.Equals(dk.HocKy, hocKy ?? string.Empty, StringComparison.OrdinalIgnoreCase)
                          && (dk.DiemQT.HasValue || dk.DiemGK.HasValue || dk.DiemCK.HasValue)
                          && !dsMoi.Contains(dk.MaMon, StringComparer.OrdinalIgnoreCase))
                .Select(dk => dk.MaMon)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (monCoDiemBiHuy.Count > 0)
            {
                throw new InvalidOperationException(
                    "Không thể hủy các môn đã có điểm: " + string.Join(", ", monCoDiemBiHuy)
                    + ". Hãy xóa điểm của các môn này trước nếu thực sự muốn hủy đăng ký.");
            }

            // Các bản ghi cũ của học kỳ không còn trong danh sách mới thì hủy;
            // bản ghi đã có điểm (kể cả một phần) là dữ liệu bảng điểm nên giữ lại không xóa.
            var cacBanGhiCu = _store.DangKys
                .Where(dk => string.Equals(dk.MSSV, mssv, StringComparison.OrdinalIgnoreCase)
                          && string.Equals(dk.HocKy, hocKy ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (DangKy dk in cacBanGhiCu.Where(dk => !dk.DiemQT.HasValue && !dk.DiemGK.HasValue && !dk.DiemCK.HasValue
                                                       && !dsMoi.Contains(dk.MaMon, StringComparer.OrdinalIgnoreCase)))
            {
                _store.DangKys.Remove(dk);
            }

            // Danh sách mới chưa có bản ghi thì thêm.
            int soMoi = 0;
            foreach (string maMonMoi in dsMoi.Where(ma => !DaDangKy(mssv, ma, hocKy)))
            {
                if (TimMon(maMonMoi) == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy môn học '{maMonMoi}'.");
                }

                _store.DangKys.Add(new DangKy
                {
                    MSSV = mssv,
                    MaMon = maMonMoi,
                    HocKy = hocKy ?? string.Empty
                });
                soMoi++;
            }

            _store.LuuDangKy();
            return soMoi;
        }

        #endregion

        #region Truy vấn phụ trợ

        private MonHoc? TimMon(string maMon)
        {
            return _store.MonHocs.FirstOrDefault(m =>
                string.Equals(m.MaMon, (maMon ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private bool TonTaiMon(string maMon)
        {
            return TimMon(maMon) != null;
        }

        private int LaySoTC(string maMon)
        {
            return TimMon(maMon)?.SoTC ?? 0;
        }

        private List<LichHoc> LayLich(string maMon)
        {
            return _store.LichHocs
                .Where(lh => string.Equals(lh.MaMon, (maMon ?? string.Empty).Trim(), StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        /// <summary>Kiểm tra mã tiên quyết: để trống hoặc phải trỏ đến môn đã có, không trỏ đến chính nó.</summary>
        private string KiemTraMaTienQuyetHopLe(string maTienQuyet, string maMon)
        {
            maTienQuyet = (maTienQuyet ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(maTienQuyet) || string.Equals(maTienQuyet, "(không có)", StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            if (string.Equals(maTienQuyet, maMon, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("Môn tiên quyết không thể là chính môn này.");
            }

            if (!TonTaiMon(maTienQuyet))
            {
                throw new ArgumentException($"Môn tiên quyết '{maTienQuyet}' chưa tồn tại trong danh mục môn học.");
            }

            return maTienQuyet;
        }

        #endregion
    }
}
