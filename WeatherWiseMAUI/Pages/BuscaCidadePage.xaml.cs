using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Models;
using System.Collections.ObjectModel;

namespace WeatherWiseMAUI.Pages
{
    public partial class BuscaCidadePage : ContentPage
    {
        private readonly ApiWeatherService _apiWeatherService;

        public ObservableCollection<WeatherData> WeatherDataList { get; set; }

        public BuscaCidadePage(ApiWeatherService apiWeatherService)
        {
            InitializeComponent();
            _apiWeatherService = apiWeatherService;
            WeatherDataList = new ObservableCollection<WeatherData>();
            WeatherCollectionView.ItemsSource = WeatherDataList;
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
                var weatherData = await _apiWeatherService.GetWeatherAsync(city);
                if (weatherData != null)
                {
                    WeatherDataList.Clear();
                    WeatherDataList.Add(weatherData);
                }
                else
                {
                    await DisplayAlert("Erro", "Não foi possível encontrar dados meteorológicos para a cidade informada.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro ao buscar os dados: {ex.Message}", "OK");
            }
        }

        private async void OnItemTapped(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is WeatherData weatherData)
            {
                try
                {
                    var cityImageUrl = await _apiWeatherService.GetCityImageAsync(weatherData.Name);
                    weatherData.CityImageUrl = cityImageUrl;
                    await Navigation.PushAsync(new DetalhesCidadePage(weatherData));
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Ocorreu um erro ao buscar a imagem da cidade: {ex.Message}", "OK");
                }
            }
        }
    }
}
