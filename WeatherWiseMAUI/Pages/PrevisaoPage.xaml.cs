using WeatherWiseMAUI.Services;

namespace WeatherWiseMAUI.Pages;

public partial class PrevisaoPage : ContentPage
{
    private ApiWeatherService apiWeatherService;

    public PrevisaoPage()
	{
		InitializeComponent();
	}

    public PrevisaoPage(ApiWeatherService apiWeatherService)
    {
        this.apiWeatherService = apiWeatherService;
    }
}