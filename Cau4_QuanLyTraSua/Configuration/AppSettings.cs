using System;
using Microsoft.Extensions.Configuration;

namespace Cau4_QuanLyTraSua.Configuration
{
    public class ConnectionStringsConfig
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class PaymentConfig
    {
        public string BankId { get; set; } = "MB";
        public string AccountNo { get; set; } = "0987654321";
        public string AccountName { get; set; } = "QUAN TRA SUA ANTIGRAVITY";
        public string Template { get; set; } = "compact2";
        public string ShopName { get; set; } = "Trà Sữa Antigravity";
    }

    public class AppSettings
    {
        public ConnectionStringsConfig ConnectionStrings { get; set; } = new();
        public PaymentConfig Payment { get; set; } = new();

        public static AppSettings Load()
        {
            var env = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);

            var config = builder.Build();
            var settings = new AppSettings();
            config.Bind(settings);

            if (string.IsNullOrWhiteSpace(settings.ConnectionStrings?.DefaultConnection))
            {
                settings.ConnectionStrings = new ConnectionStringsConfig
                {
                    DefaultConnection = config.GetConnectionString("DefaultConnection") 
                        ?? "Server=.;Database=QL_TraSua;Trusted_Connection=True;TrustServerCertificate=True;"
                };
            }

            return settings;
        }
    }
}
