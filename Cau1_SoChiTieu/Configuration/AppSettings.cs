using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Cau1_SoChiTieu.Configuration
{
    public class ConnectionStringsConfig
    {
        public string DefaultConnection { get; set; } = string.Empty;
    }

    public class AppSettings
    {
        public ConnectionStringsConfig ConnectionStrings { get; set; } = new();

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

            // Fallback nếu rỗng
            if (string.IsNullOrWhiteSpace(settings.ConnectionStrings?.DefaultConnection))
            {
                settings.ConnectionStrings = new ConnectionStringsConfig
                {
                    DefaultConnection = config.GetConnectionString("DefaultConnection") 
                        ?? "Server=.;Database=QL_ChiTieu;Trusted_Connection=True;TrustServerCertificate=True;"
                };
            }

            return settings;
        }
    }
}
