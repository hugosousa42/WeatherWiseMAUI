using System.Text.Json;
using Microsoft.Extensions.Logging;
using WeatherWiseMAUI.Models;

namespace WeatherWiseMAUI.Services
{
    public class ApiWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "4c324ec18c2d583aa51ffcedd83c981e"; 
        private readonly ILogger<ApiWeatherService> _logger;

        JsonSerializerOptions _serializerOptions;

        public ApiWeatherService(HttpClient httpClient, ILogger<ApiWeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<WeatherData>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter dados meteorológicos: {ex.Message}");
                return null;
            }
        }
    }
}
