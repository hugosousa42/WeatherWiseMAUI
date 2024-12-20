using Microsoft.Maui.Controls.Maps;
using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Models;

namespace WeatherWiseMAUI.Pages
{
    public partial class MapaTempoPage : ContentPage
    {
        private readonly ApiWeatherService _apiWeatherService;

        public MapaTempoPage(ApiWeatherService apiWeatherService)
        {
            InitializeComponent();
            _apiWeatherService = apiWeatherService;
            LoadWeatherData();
        }

        private async void LoadWeatherData()
        {
            var cities = new List<string> { "Lisboa", "Porto", "Madrid", "Paris" };
            foreach (var city in cities)
            {
                var weatherData = await _apiWeatherService.GetWeatherAsync(city);
                if (weatherData != null)
                {
                    var position = new Location(weatherData.coord.lat, weatherData.coord.lon);
                    var pin = new Pin
                    {
                        Label = $"{city}: {weatherData.main.temp}°C",
                        Address = weatherData.weather[0].description,
                        Type = PinType.Place,
                        Location = position
                    };
                    WeatherMap.Pins.Add(pin);
                }
            }
        }
    }
}
