using WeatherWiseMAUI.Pages;
using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Validations;

namespace WeatherWiseMAUI
{
    public partial class App : Application
    {
        private readonly ApiService _apiService;
        private readonly ApiWeatherService _apiWeatherService;
        private readonly IValidator _validator;

        public App(ApiService apiService, ApiWeatherService apiWeatherService, IValidator validator)
        {
            InitializeComponent();

#if DEBUG
            System.Diagnostics.Debug.WriteLine("Debug mode is active.");
#endif

            _apiService = apiService;
            _apiWeatherService = apiWeatherService;
            _validator = validator;
            SetMainPage();
        }

        private void SetMainPage()
        {
            var accessToken = Preferences.Get("accesstoken", string.Empty);

            if (string.IsNullOrEmpty(accessToken))
            {
                MainPage = new NavigationPage(new LoginPage(_apiService, _apiWeatherService, _validator));
                return;
            }

            MainPage = new AppShell(_apiService, _apiWeatherService, _validator);
        }

    }
}
