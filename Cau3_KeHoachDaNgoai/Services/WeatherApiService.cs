using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace Cau3_KeHoachDaNgoai.Services
{
    public class GeocodingResult
    {
        [JsonPropertyName("results")]
        public List<GeoLocationItem>? Results { get; set; }
    }

    public class GeoLocationItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
    }

    public class ForecastDailyData
    {
        [JsonPropertyName("time")]
        public List<string>? Time { get; set; }

        [JsonPropertyName("weather_code")]
        public List<int>? WeatherCode { get; set; }

        [JsonPropertyName("temperature_2m_max")]
        public List<double>? Temperature2mMax { get; set; }

        [JsonPropertyName("temperature_2m_min")]
        public List<double>? Temperature2mMin { get; set; }

        [JsonPropertyName("precipitation_probability_max")]
        public List<int>? PrecipitationProbabilityMax { get; set; }
    }

    public class ForecastResponse
    {
        [JsonPropertyName("daily")]
        public ForecastDailyData? Daily { get; set; }
    }

    public class WeatherInfo
    {
        public bool HasForecast { get; set; }
        public string Message { get; set; } = string.Empty;
        public int WeatherCode { get; set; }
        public string WeatherDescriptionVi { get; set; } = string.Empty;
        public string WeatherDescriptionEn { get; set; } = string.Empty;
        public string WeatherIcon { get; set; } = "☀️";
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int RainProbability { get; set; }
        public bool IsHighRainRisk => RainProbability >= 60;
    }

    public static class WeatherApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(8)
        };

        public static async Task<WeatherInfo> GetWeatherForecastAsync(string diaDiem, DateOnly ngayDi)
        {
            // Bước 1: Geocoding
            string geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(diaDiem)}&count=1&language=vi";
            var geoResponseStr = await _httpClient.GetStringAsync(geoUrl);
            var geoResult = JsonSerializer.Deserialize<GeocodingResult>(geoResponseStr);

            if (geoResult?.Results == null || geoResult.Results.Count == 0)
            {
                // Thử tìm lại không dấu hoặc báo gợi ý
                throw new KeyNotFoundException($"Không tìm thấy tọa độ cho địa danh '{diaDiem}'. Hãy thử nhập tên không dấu (ví dụ: 'Da Lat', 'Sa Pa')!");
            }

            var loc = geoResult.Results[0];

            // Bước 2: Forecast
            string forecastUrl = $"https://api.open-meteo.com/v1/forecast?latitude={loc.Latitude}&longitude={loc.Longitude}&daily=weather_code,temperature_2m_max,temperature_2m_min,precipitation_probability_max&timezone=auto&forecast_days=16";
            var forecastStr = await _httpClient.GetStringAsync(forecastUrl);
            var forecast = JsonSerializer.Deserialize<ForecastResponse>(forecastStr);

            if (forecast?.Daily?.Time == null)
            {
                return new WeatherInfo
                {
                    HasForecast = false,
                    Message = "Không có dữ liệu dự báo cho địa điểm này."
                };
            }

            string targetDateStr = ngayDi.ToString("yyyy-MM-dd");
            int index = forecast.Daily.Time.IndexOf(targetDateStr);

            if (index == -1)
            {
                return new WeatherInfo
                {
                    HasForecast = false,
                    Message = "Chưa có dữ liệu dự báo (vượt quá 16 ngày)."
                };
            }

            int code = forecast.Daily.WeatherCode != null && index < forecast.Daily.WeatherCode.Count
                ? forecast.Daily.WeatherCode[index] : 0;
            double tMin = forecast.Daily.Temperature2mMin != null && index < forecast.Daily.Temperature2mMin.Count
                ? forecast.Daily.Temperature2mMin[index] : 0;
            double tMax = forecast.Daily.Temperature2mMax != null && index < forecast.Daily.Temperature2mMax.Count
                ? forecast.Daily.Temperature2mMax[index] : 0;
            int rainProb = forecast.Daily.PrecipitationProbabilityMax != null && index < forecast.Daily.PrecipitationProbabilityMax.Count
                ? forecast.Daily.PrecipitationProbabilityMax[index] : 0;

            var (descVi, descEn, icon) = ParseWeatherCode(code);

            return new WeatherInfo
            {
                HasForecast = true,
                WeatherCode = code,
                WeatherDescriptionVi = descVi,
                WeatherDescriptionEn = descEn,
                WeatherIcon = icon,
                TempMin = tMin,
                TempMax = tMax,
                RainProbability = rainProb
            };
        }

        private static (string vi, string en, string icon) ParseWeatherCode(int code)
        {
            if (code == 0)
                return ("Trời quang đãng", "Clear sky", "☀️");
            if (code >= 1 && code <= 3)
                return ("Có mây rải rác", "Partly cloudy", "⛅");
            if (code == 45 || code == 48)
                return ("Sương mù", "Foggy", "🌫️");
            if ((code >= 51 && code <= 67) || (code >= 80 && code <= 82))
                return ("Có mưa", "Rainy", "🌧️");
            if (code >= 95 && code <= 99)
                return ("Có dông, sấm sét", "Thunderstorm", "⛈️");

            return ("Thời tiết biến đổi", "Variable", "🌤️");
        }
    }
}
