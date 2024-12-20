using WeatherWiseMAUI.Models;

namespace WeatherWiseMAUI.Pages
{
    public partial class DetalhesCidadePage : ContentPage
    {
        public DetalhesCidadePage(WeatherData weatherData)
        {
            InitializeComponent();
            BindingContext = weatherData;
        }
    }
}
