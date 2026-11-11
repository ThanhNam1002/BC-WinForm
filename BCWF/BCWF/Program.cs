using BCWF.Forms;
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

            // Hien thi form dang nhap truoc. Chi mo FrmMain khi dang nhap thanh cong.
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