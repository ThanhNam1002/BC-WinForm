using System;
using System.IO;

namespace BCWF.Services
{
    /// <summary>
    /// Sao lưu / khôi phục thư mục dữ liệu: chép toàn bộ tệp .txt trong thư mục Data
    /// vào một thư mục con Backup\&lt;thời điểm&gt;, và sao chép ngược lại khi khôi phục.
    /// </summary>
    public class BackupService
    {
        private readonly DataStore _store = DataStore.Instance;

        /// <summary>Thư mục gốc chứa các lần sao lưu (con của thư mục Data).</summary>
        public string ThuMucSaoLuu => Path.Combine(_store.DataFolder, "Backup");

        /// <summary>
        /// Sao lưu toàn bộ tệp .txt của thư mục Data vào Backup\yyyy-MM-dd_HHmmss.
        /// Trả về thư mục sao lưu vừa tạo.
        /// </summary>
        public string SaoLuu()
        {
            string thuMucDich = Path.Combine(ThuMucSaoLuu, DateTime.Now.ToString("yyyy-MM-dd_HHmmss"));
            Directory.CreateDirectory(thuMucDich);

            foreach (string tep in Directory.GetFiles(_store.DataFolder, "*.txt"))
            {
                File.Copy(tep, Path.Combine(thuMucDich, Path.GetFileName(tep)), true);
            }

            return thuMucDich;
        }

        /// <summary>
        /// Khôi phục dữ liệu từ một thư mục sao lưu: chép các tệp .txt trở lại thư mục Data
        /// (ghi đè). Sau khi gọi, cần <see cref="DataStore.LoadAll"/> để nạp lại dữ liệu mới.
        /// </summary>
        public void KhoiPhuc(string thuMucNguon)
        {
            if (string.IsNullOrWhiteSpace(thuMucNguon) || !Directory.Exists(thuMucNguon))
            {
                throw new DirectoryNotFoundException("Thư mục sao lưu không tồn tại.");
            }

            int soTep = 0;
            foreach (string tep in Directory.GetFiles(thuMucNguon, "*.txt"))
            {
                File.Copy(tep, Path.Combine(_store.DataFolder, Path.GetFileName(tep)), true);
                soTep++;
            }

            if (soTep == 0)
            {
                throw new InvalidOperationException("Thư mục được chọn không chứa tệp dữ liệu .txt nào.");
            }
        }
    }
}
