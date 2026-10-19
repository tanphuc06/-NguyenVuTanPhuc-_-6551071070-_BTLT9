using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Serilog;

namespace Cau2_QuanLyNhaTro
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var logsDir = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    path: Path.Combine(logsDir, "log-.txt"),
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            try
            {
                // Cấu hình bắt lỗi toàn cục (Global Exception Handling)
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                
                // Bắt ngoại lệ trên luồng UI (ThreadException)
                Application.ThreadException += Application_ThreadException;
                
                // Bắt ngoại lệ trên các luồng non-UI (UnhandledException)
                AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

                Log.Information("Ứng dụng Quản lý nhà trọ khởi động.");
                ApplicationConfiguration.Initialize();
                Application.Run(new FormKhachThue());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Lỗi nghiêm trọng cấp ứng dụng.");
                MessageBox.Show("Đã xảy ra lỗi, thông tin đã được ghi lại.", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.Information("Ứng dụng Quản lý nhà trọ kết thúc.");
                Log.CloseAndFlush();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Error(e.Exception, "Ngoại lệ toàn cục (ThreadException): {Message}", e.Exception.Message);
            MessageBox.Show("Đã xảy ra lỗi, thông tin đã được ghi lại.", "Thông báo lỗi", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Fatal(ex, "Ngoại lệ toàn cục (DomainUnhandledException - IsTerminating={IsTerminating}): {Message}", 
                    e.IsTerminating, ex.Message);
            }
            else
            {
                Log.Fatal("Ngoại lệ toàn cục không xác định: {Obj}", e.ExceptionObject);
            }

            MessageBox.Show("Đã xảy ra lỗi, thông tin đã được ghi lại.", "Thông báo lỗi hệ thống", 
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}