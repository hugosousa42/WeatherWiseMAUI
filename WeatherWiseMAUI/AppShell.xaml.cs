﻿using WeatherWiseMAUI.Pages;
using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Validations;

namespace WeatherWiseMAUI
{
    public partial class AppShell : Shell
    {
        private readonly ApiService _apiService;
        private readonly ApiWeatherService _apiWeatherService;
        private readonly IValidator _validator;

        public AppShell(ApiService apiService, ApiWeatherService apiWeatherService, IValidator validator)
        {
            InitializeComponent();
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _apiWeatherService = apiWeatherService ?? throw new ArgumentNullException(nameof(apiWeatherService));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            ConfigureShell();
        }

        private void ConfigureShell()
        {
            var homePage = new HomePage(_apiService, _apiWeatherService, _validator);
            var mapaTempoPage = new MapaTempoPage(_apiWeatherService);
            var buscaCidadePage = new BuscaCidadePage(_apiWeatherService);
            var previsaoPage = new PrevisaoPage(_apiWeatherService);
            var compararCidadesPage = new CompararCidadesPage(_apiWeatherService);
            var sobrePage = new SobrePage();

            Items.Add(new FlyoutItem
            {
                Title = "Home",
                Icon = "home.png",
                FlyoutDisplayOptions = FlyoutDisplayOptions.AsMultipleItems,
                Items =
                {
                    new ShellContent { Title = "Home", Icon = "home.png", Content = homePage },
                    new ShellContent { Title = "Mapa do Tempo", Icon = "map.png", Content = mapaTempoPage },
                    new ShellContent { Title = "Buscar Cidade", Icon = "search.png", Content = buscaCidadePage },
                    new ShellContent { Title = "Previsão", Icon = "forecast.png", Content = previsaoPage },
                    new ShellContent { Title = "Comparar Cidades", Icon = "compare.png", Content = compararCidadesPage },
                    new ShellContent { Title = "Sobre", Icon = "info.png", Content = sobrePage },
                }
            });

            // Adicionando o botão de logout no menu de navegação
            Items.Add(new MenuItem
            {
                Text = "Logout",
                IconImageSource = "logout.png",
                Command = new Command(OnLogoutClicked)
            });
        }

        private async void OnLogoutClicked()
        {
            // Limpar o token de acesso
            Preferences.Remove("accesstoken");

            // Redirecionar para a página de login
            Application.Current!.MainPage = new NavigationPage(new LoginPage(_apiService, _apiWeatherService, _validator));
        }
    }
}
