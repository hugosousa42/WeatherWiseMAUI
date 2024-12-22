using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Models;
using System.Collections.ObjectModel;

namespace WeatherWiseMAUI.Pages
{
    public partial class PrevisaoPage : ContentPage
    {
        private readonly ApiWeatherService _apiWeatherService;

        public ObservableCollection<Forecast> Forecasts { get; set; }

        public PrevisaoPage(ApiWeatherService apiWeatherService)
        {
            InitializeComponent();
            _apiWeatherService = apiWeatherService;
            Forecasts = new ObservableCollection<Forecast>();
            ForecastListView.ItemsSource = Forecasts;
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            var city = CityEntry.Text;
            if (!string.IsNullOrEmpty(city))
            {
                var forecastData = await _apiWeatherService.GetForecastAsync(city);
                if (forecastData != null)
                {
                    Forecasts.Clear();
                    foreach (var forecast in forecastData.List)
                    {
                        if (forecast.Main != null && forecast.Weather != null && forecast.Weather.Count > 0)
                        {
                            Forecasts.Add(new Forecast
                            {
                                Temperature = $"{forecast.Main.Temp}°C",
                                Description = forecast.Weather[0].Description
                            });
                        }
                    }
                }
            }
        }
    }

    public class Forecast
    {
        public string Temperature { get; set; }
        public string Description { get; set; }
    }
}
