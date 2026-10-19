using System;
using System.IO;
using System.Windows.Forms;
using Serilog;

namespace Cau3_KeHoachDaNgoai
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
                Log.Information("Ứng dụng Kế hoạch dã ngoại khởi động.");
                ApplicationConfiguration.Initialize();
                Application.Run(new FormChuyenDi());
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Lỗi nghiêm trọng ứng dụng Kế hoạch dã ngoại.");
                MessageBox.Show("Đã xảy ra lỗi nghiêm trọng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.Information("Ứng dụng Kế hoạch dã ngoại kết thúc.");
                Log.CloseAndFlush();
            }
        }
    }
}