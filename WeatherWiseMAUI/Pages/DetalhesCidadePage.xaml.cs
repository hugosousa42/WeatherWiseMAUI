using WeatherWiseMAUI.Models;
using System;

namespace WeatherWiseMAUI.Pages
{
    public partial class DetalhesCidadePage : ContentPage
    {
        public DetalhesCidadePage(WeatherData weatherData)
        {
            InitializeComponent();
            // Convertendo os timestamps Unix para DateTime e armazenando como strings
            weatherData.Sys.SunriseTime = ConvertUnixTimestampToDateTime(weatherData.Sys.Sunrise);
            weatherData.Sys.SunsetTime = ConvertUnixTimestampToDateTime(weatherData.Sys.Sunset);
            BindingContext = weatherData;
        }

        private string ConvertUnixTimestampToDateTime(int unixTimestamp)
        {
            // Converte o timestamp Unix para DateTime e retorna como string
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            return dateTimeOffset.LocalDateTime.ToString("HH:mm");
        }

        private void OnShowMoreButtonClicked(object sender, EventArgs e)
        {
            MoreInfoStackLayout.IsVisible = !MoreInfoStackLayout.IsVisible;
            ((Button)sender).Text = MoreInfoStackLayout.IsVisible ? "Mostrar Menos" : "Mostrar Mais";
        }
    }
}
