using BCWF.Models;
using System;
using System.Collections.Generic;

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

        /// <summary>Điểm tổng kết = 0.3*QT + 0.3*GK + 0.4*CK, null nếu chưa đủ 3 đầu điểm.</summary>
        public double? TinhDiemTongKet(DangKy dangKy)
        {
            throw new NotImplementedException("TODO (SV3): tính điểm tổng kết 30/30/40.");
        }

        /// <summary>Xếp loại theo điểm tổng kết (A/B/C/D/F hoặc Xuất sắc/Giỏi/...).</summary>
        public string XepLoai(double diemTongKet)
        {
            throw new NotImplementedException("TODO (SV3): xếp loại học lực.");
        }

        /// <summary>Môn bị rớt (điểm tổng kết &lt; 4) để tô đỏ trên ListView.</summary>
        public bool LaMonRot(DangKy dangKy)
        {
            throw new NotImplementedException("TODO (SV3): xác định môn rớt.");
        }

        /// <summary>Điểm trung bình tích lũy của sinh viên (tính theo tín chỉ các môn đã có điểm).</summary>
        public double TinhDiemTrungBinhTichLuy(string mssv)
        {
            throw new NotImplementedException("TODO (SV3): tính điểm trung bình tích lũy.");
        }

        /// <summary>Nâng cao: danh sách sinh viên bị cảnh báo học vụ (ĐTB tích lũy dưới ngưỡng trong App.config).</summary>
        public List<SinhVien> LayDanhSachCanhBao()
        {
            throw new NotImplementedException("TODO (nâng cao): lập danh sách cảnh báo học vụ.");
        }

        /// <summary>Lưu 3 đầu điểm vừa sửa trực tiếp trên ListView.</summary>
        public void LuuDiem(string mssv, string maMon, string hocKy, double? diemQT, double? diemGK, double? diemCK)
        {
            throw new NotImplementedException("TODO (SV3): cập nhật điểm vào dangky.txt.");
        }

        /// <summary>Xuất bảng điểm của lớp theo môn ra tệp CSV.</summary>
        public void XuatBangDiemCsv(string maLop, string maMon, string hocKy, string duongDanCsv)
        {
            throw new NotImplementedException("TODO (SV3): xuất bảng điểm lớp ra CSV.");
        }
    }
}
