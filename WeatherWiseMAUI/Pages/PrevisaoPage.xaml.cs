using System.Collections.ObjectModel;
using System.Text.Encodings.Web;
using WeatherWiseMAUI.Services;

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
            var city = CityEntry.Text?.Trim();
            if (string.IsNullOrEmpty(city))
            {
                await DisplayAlert("Erro", "Por favor, insira o nome de uma cidade.", "OK");
                return;
            }

            try
            {
                var forecastData = await _apiWeatherService.GetForecastAsync(city);
                if (forecastData?.List?.Count > 0)
                {
                    Forecasts.Clear();
                    foreach (var forecast in forecastData.List)
                    {
                        if (forecast.Main != null && forecast.Weather != null && forecast.Weather.Count > 0)
                        {
                            var descriptionEncoded = JavaScriptEncoder.Default.Encode(forecast.Weather[0].Description);

                            Forecasts.Add(new Forecast
                            {
                                Temperature = $"{forecast.Main.Temp}°C",
                                Description = descriptionEncoded,
                                Time = UnixTimeStampToDateTime(forecast.Dt).ToString("HH:mm")
                            });
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Aviso", "Nenhuma previsão encontrada para a cidade informada.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao buscar a previsão: {ex.Message}", "OK");
            }
        }

        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }

    public class Forecast
    {
        public string Temperature { get; set; }
        public string Description { get; set; }
        public string Time { get; set; }
    }
}
