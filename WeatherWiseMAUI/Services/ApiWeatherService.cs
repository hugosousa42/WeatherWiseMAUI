using System.Text.Json;
using Microsoft.Extensions.Logging;
using WeatherWiseMAUI.Models;

namespace WeatherWiseMAUI.Services
{
    public class ApiWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "yourOpenWeatherAPIKey";
        private readonly string _pexelsApiKey = "yourPexelsAPIKey";
        private readonly ILogger<ApiWeatherService> _logger;

        JsonSerializerOptions _serializerOptions;

        public ApiWeatherService(HttpClient httpClient, ILogger<ApiWeatherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping 
            };
        }

        public async Task<WeatherData> GetWeatherAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=pt";
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

        public async Task<ForecastData> GetForecastAsync(string city)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&appid={_apiKey}&units=metric&lang=pt";
            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return JsonSerializer.Deserialize<ForecastData>(response, _serializerOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter dados de previsão: {ex.Message}");
                return null;
            }
        }

        public async Task<string> GetCityImageAsync(string city)
        {
            var url = $"https://api.pexels.com/v1/search?query={city}+cityscape&per_page=1";

            // Remover o cabeçalho Authorization se já estiver presente
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
            }

            // Adicionar o cabeçalho Authorization
            _httpClient.DefaultRequestHeaders.Add("Authorization", _pexelsApiKey);

            var response = await _httpClient.GetStringAsync(url);
            var jsonDoc = JsonDocument.Parse(response);
            var imageUrl = jsonDoc.RootElement.GetProperty("photos")[0].GetProperty("src").GetProperty("large").GetString();
            return imageUrl;
        }
    }
}
