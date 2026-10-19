using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Serilog;

namespace Cau2_QuanLyNhaTro.Services
{
    public class ProvinceDto
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("wards")]
        public List<WardDto>? Wards { get; set; }

        [JsonPropertyName("districts")]
        public List<DistrictDto>? Districts { get; set; }

        public override string ToString() => Name;
    }

    public class DistrictDto
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("wards")]
        public List<WardDto>? Wards { get; set; }
    }

    public class WardDto
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        public override string ToString() => Name;
    }

    public static class AddressApiService
    {
        private static readonly HttpClient _httpClient = new HttpClient(new SocketsHttpHandler
        {
            AllowAutoRedirect = true
        })
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private static List<ProvinceDto>? _provincesCache;
        private static readonly Dictionary<int, List<WardDto>> _wardsCache = new();

        public static async Task<List<ProvinceDto>> GetProvincesAsync()
        {
            if (_provincesCache != null && _provincesCache.Count > 0)
                return _provincesCache;

            string[] endpoints = {
                "https://provinces.open-api.vn/api/v2/p/",
                "https://provinces.open-api.vn/api/p/"
            };

            foreach (var url in endpoints)
            {
                try
                {
                    var response = await _httpClient.GetStringAsync(url);
                    var list = JsonSerializer.Deserialize<List<ProvinceDto>>(response);
                    if (list != null && list.Count > 0)
                    {
                        _provincesCache = list;
                        return _provincesCache;
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể kết nối đến {Url}", url);
                }
            }

            throw new HttpRequestException("Không thể tải danh sách tỉnh/thành từ API.");
        }

        public static async Task<List<WardDto>> GetWardsAsync(int provinceCode)
        {
            if (_wardsCache.TryGetValue(provinceCode, out var cached))
                return cached;

            string[] endpoints = {
                $"https://provinces.open-api.vn/api/v2/p/{provinceCode}?depth=2",
                $"https://provinces.open-api.vn/api/p/{provinceCode}?depth=2"
            };

            foreach (var url in endpoints)
            {
                try
                {
                    var response = await _httpClient.GetStringAsync(url);
                    var province = JsonSerializer.Deserialize<ProvinceDto>(response);
                    var result = new List<WardDto>();

                    if (province?.Wards != null && province.Wards.Count > 0)
                    {
                        result.AddRange(province.Wards);
                    }
                    else if (province?.Districts != null && province.Districts.Count > 0)
                    {
                        foreach (var d in province.Districts)
                        {
                            if (d.Wards != null && d.Wards.Count > 0)
                            {
                                result.AddRange(d.Wards);
                            }
                            else
                            {
                                // Nếu quận/huyện không có danh sách xã thì đưa tên quận vào
                                result.Add(new WardDto { Code = d.Code, Name = d.Name });
                            }
                        }
                    }

                    if (result.Count > 0)
                    {
                        _wardsCache[provinceCode] = result;
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "Không thể lấy danh sách phường/xã từ {Url}", url);
                }
            }

            throw new HttpRequestException($"Không thể tải danh sách phường/xã cho tỉnh mã {provinceCode}");
        }
    }
}
