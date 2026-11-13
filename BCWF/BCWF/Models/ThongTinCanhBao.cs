using System.Collections.Generic;

namespace BCWF.Models
{
    /// <summary>
    /// Thông tin cảnh báo học vụ của một sinh viên.
    /// </summary>
    public class ThongTinCanhBao
    {
        /// <summary>Mã sinh viên.</summary>
        public string MSSV { get; set; } = string.Empty;

        /// <summary>Họ và tên sinh viên.</summary>
        public string HoTen { get; set; } = string.Empty;

        /// <summary>Lớp của sinh viên.</summary>
        public string MaLop { get; set; } = string.Empty;

        /// <summary>Điểm trung bình tích lũy (tính theo tín chỉ các môn đã có điểm).</summary>
        public double DtbTichLuy { get; set; }

        /// <summary>Điểm trung bình của học kỳ đang lọc (null nếu không lọc học kỳ).</summary>
        public double? DtbHocKy { get; set; }

        /// <summary>Số môn bị rớt trong phạm vi đang lọc.</summary>
        public int SoMonRot { get; set; }

        /// <summary>Tổng số tín chỉ của các môn bị rớt.</summary>
        public int TongTinChiRot { get; set; }

        /// <summary>Mức cảnh báo: Cảnh báo / Cảnh báo nặng / Buộc nghỉ học.</summary>
        public string MucCanhBao { get; set; } = string.Empty;

        /// <summary>Danh sách môn bị rớt (tên môn + điểm tổng kết).</summary>
        public List<string> DanhSachMonRot { get; } = new List<string>();

        /// <summary>Danh sách môn bị rớt dạng chữ để hiển thị/xuất CSV.</summary>
        public string MoTaMonRot => string.Join(", ", DanhSachMonRot);
    }
}
