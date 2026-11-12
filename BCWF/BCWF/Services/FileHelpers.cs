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

        /// <summary>Đọc tất cả dòng của tệp, bỏ qua dòng trống.</summary>
        public static List<string> DocDong(string duongDan)
        {
            var ketQua = new List<string>();
            if (!File.Exists(duongDan))
            {
                return ketQua;
            }

            foreach (string dong in File.ReadAllLines(duongDan, Encoding.UTF8))
            {
                if (!string.IsNullOrWhiteSpace(dong))
                {
                    ketQua.Add(dong.Trim());
                }
            }

            return ketQua;
        }

        /// <summary>Ghi đè tệp bằng danh sách dòng cho trước (tự tạo thư mục nếu cần).</summary>
        public static void GhiDong(string duongDan, IEnumerable<string> cacDong)
        {
            string? thuMuc = Path.GetDirectoryName(duongDan);
            if (!string.IsNullOrEmpty(thuMuc) && !Directory.Exists(thuMuc))
            {
                Directory.CreateDirectory(thuMuc);
            }

            File.WriteAllLines(duongDan, cacDong, new UTF8Encoding(false));
        }

        /// <summary>Đọc tệp CSV (hỗ trợ ô có dấu ngoặc kép).</summary>
        /// <param name="duongDan">Đường dẫn tệp CSV.</param>
        /// <param name="coDongTieuDe">Bỏ qua dòng đầu tiên hay không.</param>
        public static List<string[]> DocCsv(string duongDan, bool coDongTieuDe = true)
        {
            var ketQua = new List<string[]>();
            if (!File.Exists(duongDan))
            {
                return ketQua;
            }

            string[] cacDong = File.ReadAllLines(duongDan, Encoding.UTF8);
            int batDau = coDongTieuDe ? 1 : 0;
            for (int i = batDau; i < cacDong.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(cacDong[i]))
                {
                    continue;
                }

                ketQua.Add(TachCsv(cacDong[i]));
            }

            return ketQua;
        }

        /// <summary>Ghi danh sách dòng CSV ra tệp (tự thêm BOM để Excel đọc đúng tiếng Việt).</summary>
        public static void GhiCsv(string duongDan, IEnumerable<string[]> cacDong)
        {
            string? thuMuc = Path.GetDirectoryName(duongDan);
            if (!string.IsNullOrEmpty(thuMuc) && !Directory.Exists(thuMuc))
            {
                Directory.CreateDirectory(thuMuc);
            }

            var noiDung = new StringBuilder();
            foreach (string[] dong in cacDong)
            {
                noiDung.AppendLine(string.Join(',', Array.ConvertAll(dong, BocNgoacKep)));
            }

            File.WriteAllText(duongDan, noiDung.ToString(), new UTF8Encoding(true));
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
    }
}
