using System.Text.Encodings.Web;
using WeatherWiseMAUI.Services;

namespace WeatherWiseMAUI.Pages
{
    public partial class CompararCidadesPage : ContentPage
    {
        private readonly ApiWeatherService _apiWeatherService;

        public CompararCidadesPage(ApiWeatherService apiWeatherService)
        {
            InitializeComponent();
            _apiWeatherService = apiWeatherService ?? throw new ArgumentNullException(nameof(apiWeatherService));
        }

        private async void OnCompareButtonClicked(object sender, EventArgs e)
        {
            var city1 = CityEntry1.Text?.Trim();
            var city2 = CityEntry2.Text?.Trim();

            if (string.IsNullOrEmpty(city1) || string.IsNullOrEmpty(city2))
            {
                await DisplayAlert("Erro", "Por favor, insira o nome de ambas as cidades.", "OK");
                return;
            }

            if (city1.Equals(city2, StringComparison.OrdinalIgnoreCase))
            {
                await DisplayAlert("Aviso", "As cidades comparadas são iguais. Por favor, insira nomes de cidades diferentes.", "OK");
                return;
            }

            try
            {
                var weatherData1 = await _apiWeatherService.GetWeatherAsync(city1);
                var weatherData2 = await _apiWeatherService.GetWeatherAsync(city2);

                if (weatherData1 != null && weatherData2 != null)
                {
                    var city1Encoded = JavaScriptEncoder.Default.Encode(weatherData1.Name);
                    var city2Encoded = JavaScriptEncoder.Default.Encode(weatherData2.Name);

                    City1NameLabel.Text = city1Encoded;
                    City2NameLabel.Text = city2Encoded;

                    City1TempLabel.Text = $"Temperatura: {weatherData1.Main.Temp}°C";
                    City2TempLabel.Text = $"Temperatura: {weatherData2.Main.Temp}°C";

                    City1HumidityLabel.Text = $"Umidade: {weatherData1.Main.Humidity}%";
                    City2HumidityLabel.Text = $"Umidade: {weatherData2.Main.Humidity}%";

                    City1PressureLabel.Text = $"Pressão: {weatherData1.Main.Pressure} hPa";
                    City2PressureLabel.Text = $"Pressão: {weatherData2.Main.Pressure} hPa";

                    City1WindLabel.Text = $"Vento: {weatherData1.Wind.Speed} m/s, {weatherData1.Wind.Deg}°";
                    City2WindLabel.Text = $"Vento: {weatherData2.Wind.Speed} m/s, {weatherData2.Wind.Deg}°";

                    City1CloudsLabel.Text = $"Nuvens: {weatherData1.Clouds.All}%";
                    City2CloudsLabel.Text = $"Nuvens: {weatherData2.Clouds.All}%";

                    City1SunriseLabel.Text = $"Nascer do Sol: {UnixTimeStampToDateTime(weatherData1.Sys.Sunrise).ToString("HH:mm")}";
                    City2SunriseLabel.Text = $"Nascer do Sol: {UnixTimeStampToDateTime(weatherData2.Sys.Sunrise).ToString("HH:mm")}";

                    City1SunsetLabel.Text = $"Pôr do Sol: {UnixTimeStampToDateTime(weatherData1.Sys.Sunset).ToString("HH:mm")}";
                    City2SunsetLabel.Text = $"Pôr do Sol: {UnixTimeStampToDateTime(weatherData2.Sys.Sunset).ToString("HH:mm")}";
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível encontrar dados meteorológicos para uma ou ambas as cidades.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao buscar os dados: {ex.Message}", "OK");
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
}
