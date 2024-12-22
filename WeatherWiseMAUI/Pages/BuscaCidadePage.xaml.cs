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
            WeatherListView.ItemsSource = WeatherDataList;
        }

        private async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            var city = CityEntry.Text;
            if (!string.IsNullOrEmpty(city))
            {
                var weatherData = await _apiWeatherService.GetWeatherAsync(city);
                if (weatherData != null)
                {
                    WeatherDataList.Clear();
                    WeatherDataList.Add(weatherData);
                }
            }
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is WeatherData weatherData)
            {
                await Navigation.PushAsync(new DetalhesCidadePage(weatherData));
            }
        }
    }
}
