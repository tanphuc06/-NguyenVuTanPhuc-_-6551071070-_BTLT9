using System;
using System.IO;
using System.Windows.Forms;
using Serilog;

namespace Cau1_SoChiTieu
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Thiết lập đường dẫn thư mục Logs cạnh file exe
            var logsDir = Path.Combine(AppContext.BaseDirectory, "Logs");
            if (!Directory.Exists(logsDir))
            {
                Directory.CreateDirectory(logsDir);
            }

            var logPath = Path.Combine(logsDir, "log-.txt");

            // Cấu hình Serilog ghi theo ngày: log-yyyyMMdd.txt
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File(
                    path: logPath,
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .CreateLogger();

            try
            {
                Log.Information("Ứng dụng Sổ chi tiêu cá nhân khởi động.");
                ApplicationConfiguration.Initialize();
                Application.Run(new FormChiTieu());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Ứng dụng gặp sự cố không mong muốn.");
                MessageBox.Show("Ứng dụng gặp sự cố và cần đóng: " + ex.Message, "Lỗi nghiêm trọng", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.Information("Ứng dụng Sổ chi tiêu cá nhân kết thúc.");
                Log.CloseAndFlush();
            }
        }
    }
}