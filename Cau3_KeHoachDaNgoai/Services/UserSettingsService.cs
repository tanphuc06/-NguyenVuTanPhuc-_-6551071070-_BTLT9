using System;
using System.IO;
using System.Text.Json;
using Serilog;

namespace Cau3_KeHoachDaNgoai.Services
{
    public class UserSettings
    {
        public string Language { get; set; } = "vi-VN"; // "vi-VN" hoặc "en-US"
    }

    public static class UserSettingsService
    {
        private static readonly string SettingsFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), 
            "KeHoachDaNgoai"
        );

        private static readonly string SettingsFile = Path.Combine(SettingsFolder, "usersettings.json");

        public static UserSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    var settings = JsonSerializer.Deserialize<UserSettings>(json);
                    if (settings != null) return settings;
                }
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể đọc usersettings.json từ %LocalAppData%");
            }

            return new UserSettings();
        }

        public static void Save(UserSettings settings)
        {
            try
            {
                if (!Directory.Exists(SettingsFolder))
                {
                    Directory.CreateDirectory(SettingsFolder);
                }

                var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFile, json);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Không thể lưu usersettings.json vào %LocalAppData%");
            }
        }
    }
}
