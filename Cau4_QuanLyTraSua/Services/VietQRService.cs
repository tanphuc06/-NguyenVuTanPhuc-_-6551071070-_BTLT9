using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Cau4_QuanLyTraSua.Configuration;
using Serilog;

namespace Cau4_QuanLyTraSua.Services
{
    public class VietQRResult
    {
        public bool Success { get; set; }
        public Image? QrImage { get; set; }
        public string BankId { get; set; } = string.Empty;
        public string AccountNo { get; set; } = string.Empty;
        public string MaskedAccountNo { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public static class VietQRService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(8)
        };

        public static async Task<VietQRResult> GenerateVietQRAsync(PaymentConfig config, decimal amount, string noiDung, int maHd)
        {
            string masked = config.AccountNo.Length > 4 
                ? new string('*', config.AccountNo.Length - 4) + config.AccountNo.Substring(config.AccountNo.Length - 4)
                : "****";

            var result = new VietQRResult
            {
                BankId = config.BankId,
                AccountNo = config.AccountNo,
                MaskedAccountNo = masked,
                AccountName = config.AccountName,
                Amount = amount,
                Content = noiDung
            };

            // Log Information: mã HD, số tiền - không ghi số tài khoản đầy đủ (theo yêu cầu 7)
            Log.Information("Tạo yêu cầu thanh toán QR: MaHD={MaHD}, SoTien={SoTien}, NganHang={BankId}, STK={MaskedSTK}",
                maHd, amount, config.BankId, masked);

            string encodedAmount = Uri.EscapeDataString(amount.ToString("0"));
            string encodedInfo = Uri.EscapeDataString(noiDung);
            string encodedName = Uri.EscapeDataString(config.AccountName);

            string url = $"https://img.vietqr.io/image/{config.BankId}-{config.AccountNo}-{config.Template}.png?amount={encodedAmount}&addInfo={encodedInfo}&accountName={encodedName}";

            try
            {
                byte[] imageBytes = await _httpClient.GetByteArrayAsync(url);
                using var ms = new MemoryStream(imageBytes);
                result.QrImage = Image.FromStream(ms);
                result.Success = true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Lỗi khi tải ảnh mã QR từ VietQR.io cho MaHD={MaHD}", maHd);
                result.Success = false;
                result.ErrorMessage = "Không thể tải mã QR trực tuyến (vui lòng chuyển khoản thủ công hoặc chọn tiền mặt).";
                // Tạo ảnh placeholder offline hiển thị thông tin chuyển khoản rõ ràng
                result.QrImage = CreateFallbackImage(config, amount, noiDung);
            }

            return result;
        }

        private static Image CreateFallbackImage(PaymentConfig config, decimal amount, string noiDung)
        {
            var bmp = new Bitmap(280, 280);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            using var borderPen = new Pen(Color.FromArgb(200, 200, 200), 2);
            g.DrawRectangle(borderPen, 5, 5, 270, 270);

            using var titleFont = new Font("Segoe UI", 11, FontStyle.Bold);
            using var subFont = new Font("Segoe UI", 9, FontStyle.Regular);
            using var boldFont = new Font("Segoe UI", 9, FontStyle.Bold);
            using var headerBrush = new SolidBrush(Color.DarkBlue);
            using var textBrush = new SolidBrush(Color.Black);
            using var redBrush = new SolidBrush(Color.DarkRed);

            g.DrawString("CHUYỂN KHOẢN THỦ CÔNG", titleFont, headerBrush, new PointF(30, 20));
            g.DrawString("(Mất kết nối API VietQR)", subFont, Brushes.Gray, new PointF(65, 45));

            int y = 80;
            g.DrawString("Ngân hàng:", boldFont, textBrush, new PointF(20, y));
            g.DrawString(config.BankId, subFont, textBrush, new PointF(110, y));

            y += 30;
            g.DrawString("Số TK:", boldFont, textBrush, new PointF(20, y));
            g.DrawString(config.AccountNo, boldFont, redBrush, new PointF(110, y));

            y += 30;
            g.DrawString("Chủ TK:", boldFont, textBrush, new PointF(20, y));
            g.DrawString(config.AccountName, subFont, textBrush, new PointF(110, y));

            y += 30;
            g.DrawString("Số tiền:", boldFont, textBrush, new PointF(20, y));
            g.DrawString($"{amount:N0} đ", boldFont, redBrush, new PointF(110, y));

            y += 30;
            g.DrawString("Nội dung:", boldFont, textBrush, new PointF(20, y));
            g.DrawString(noiDung, boldFont, headerBrush, new PointF(110, y));

            return bmp;
        }
    }
}
