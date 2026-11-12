using BCWF.Forms;
using BCWF.Services;
using System;
using System.Windows.Forms;

namespace BCWF
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            try
            {
                // Nạp toàn bộ dữ liệu trong thư mục Data vào bộ nhớ dùng chung.
                DataStore.Instance.LoadAll();
            }
            catch (Exception loi)
            {
                MessageBox.Show($"Không nạp được dữ liệu: {loi.Message}",
                    "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Hiển thị form đăng nhập trước. Chỉ mở FrmMain khi đăng nhập thành công.
            using (FrmDangNhap loginForm = new FrmDangNhap())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new FrmMain());
                }
            }
        }
    }
}
