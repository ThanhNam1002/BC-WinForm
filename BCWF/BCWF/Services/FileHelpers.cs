using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace BCWF.Services
{
    /// <summary>
    /// Các hàm đọc/ghi tệp dùng chung cho toàn bộ dự án.
    /// Dữ liệu chính dùng tệp văn bản phân tách bằng dấu ';', mỗi dòng một bản ghi.
    /// </summary>
    public static class FileHelpers
    {
        public const char DauPhanCach = ';';

        /// <summary>Đọc tất cả dòng của tệp, bỏ qua dòng trống (dùng StreamReader).</summary>
        public static List<string> DocDong(string duongDan)
        {
            var ketQua = new List<string>();
            if (!File.Exists(duongDan))
            {
                return ketQua;
            }

            using (var doc = new StreamReader(duongDan, Encoding.UTF8))
            {
                while (doc.ReadLine() is { } dong)
                {
                    if (!string.IsNullOrWhiteSpace(dong))
                    {
                        ketQua.Add(dong.Trim());
                    }
                }
            }

            return ketQua;
        }

        /// <summary>Ghi đè tệp bằng danh sách dòng cho trước (dùng StreamWriter, tự tạo thư mục nếu cần).</summary>
        public static void GhiDong(string duongDan, IEnumerable<string> cacDong)
        {
            string? thuMuc = Path.GetDirectoryName(duongDan);
            if (!string.IsNullOrEmpty(thuMuc) && !Directory.Exists(thuMuc))
            {
                Directory.CreateDirectory(thuMuc);
            }

            using (var ghi = new StreamWriter(duongDan, false, new UTF8Encoding(false)))
            {
                foreach (string dong in cacDong)
                {
                    ghi.WriteLine(dong);
                }
            }
        }

        /// <summary>Đọc tệp CSV (hỗ trợ ô có dấu ngoặc kép, dùng StreamReader).</summary>
        /// <param name="duongDan">Đường dẫn tệp CSV.</param>
        /// <param name="coDongTieuDe">Bỏ qua dòng đầu tiên hay không.</param>
        public static List<string[]> DocCsv(string duongDan, bool coDongTieuDe = true)
        {
            var ketQua = new List<string[]>();
            if (!File.Exists(duongDan))
            {
                return ketQua;
            }

            int i = 0;
            using (var doc = new StreamReader(duongDan, Encoding.UTF8))
            {
                while (doc.ReadLine() is { } dong)
                {
                    i++;
                    if (i == 1 && coDongTieuDe)
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(dong))
                    {
                        continue;
                    }

                    ketQua.Add(TachCsv(dong));
                }
            }

            return ketQua;
        }

        /// <summary>Ghi danh sách dòng CSV ra tệp (dùng StreamWriter, tự thêm BOM để Excel đọc đúng tiếng Việt).</summary>
        public static void GhiCsv(string duongDan, IEnumerable<string[]> cacDong)
        {
            string? thuMuc = Path.GetDirectoryName(duongDan);
            if (!string.IsNullOrEmpty(thuMuc) && !Directory.Exists(thuMuc))
            {
                Directory.CreateDirectory(thuMuc);
            }

            using (var ghi = new StreamWriter(duongDan, false, new UTF8Encoding(true)))
            {
                foreach (string[] dong in cacDong)
                {
                    ghi.WriteLine(string.Join(',', Array.ConvertAll(dong, BocNgoacKep)));
                }
            }
        }

        /// <summary>Tạo tên tệp an toàn từ chuỗi (dùng cho ảnh thẻ, tệp xuất...).</summary>
        public static string ChuanHoaTenTep(string ten)
        {
            foreach (char kyTu in Path.GetInvalidFileNameChars())
            {
                ten = ten.Replace(kyTu, '_');
            }

            return ten.Trim();
        }

        private static string BocNgoacKep(string giaTri)
        {
            if (giaTri.Contains(',') || giaTri.Contains('"') || giaTri.Contains('\n'))
            {
                return "\"" + giaTri.Replace("\"", "\"\"") + "\"";
            }

            return giaTri;
        }

        private static string[] TachCsv(string dong)
        {
            var cot = new List<string>();
            var hienTai = new StringBuilder();
            bool trongNgoac = false;

            for (int i = 0; i < dong.Length; i++)
            {
                char kyTu = dong[i];
                if (trongNgoac)
                {
                    if (kyTu == '"')
                    {
                        if (i + 1 < dong.Length && dong[i + 1] == '"')
                        {
                            hienTai.Append('"');
                            i++;
                        }
                        else
                        {
                            trongNgoac = false;
                        }
                    }
                    else
                    {
                        hienTai.Append(kyTu);
                    }
                }
                else if (kyTu == '"')
                {
                    trongNgoac = true;
                }
                else if (kyTu == ',')
                {
                    cot.Add(hienTai.ToString().Trim());
                    hienTai.Clear();
                }
                else
                {
                    hienTai.Append(kyTu);
                }
            }

            cot.Add(hienTai.ToString().Trim());
            return cot.ToArray();
        }

        /// <summary>Chuyển chuỗi sang số thực theo định dạng không phụ thuộc văn hóa.</summary>
        public static bool ThuDocSo(string? chuoi, out double giaTri)
        {
            return double.TryParse(chuoi, NumberStyles.Float, CultureInfo.InvariantCulture, out giaTri);
        }

        /// <summary>
        /// Ghi một dòng kèm thời gian vào tệp log.txt cạnh thư mục dữ liệu.
        /// Hàm này không ném lỗi để việc ghi log không làm gián đoạn chương trình.
        /// </summary>
        public static void GhiLog(string noiDung)
        {
            try
            {
                string thuMuc = DataStore.Instance.DataFolder;
                if (!Directory.Exists(thuMuc))
                {
                    Directory.CreateDirectory(thuMuc);
                }

                string duongDan = Path.Combine(thuMuc, "log.txt");
                using (var ghi = new StreamWriter(duongDan, true, Encoding.UTF8))
                {
                    ghi.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{noiDung}");
                }
            }
            catch
            {
                // Không thể ghi log thì bỏ qua, tránh lỗi dây chuyền.
            }
        }
    }
}
