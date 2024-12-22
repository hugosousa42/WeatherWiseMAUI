using WeatherWiseMAUI.Models;
using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Validations;
using System.Collections.ObjectModel;

namespace WeatherWiseMAUI.Pages
{
    public partial class HomePage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly ApiWeatherService _apiWeatherService;
        private readonly IValidator _validator;

        public ObservableCollection<Cidade> CidadesPortuguesas { get; set; }
        public ObservableCollection<Cidade> CidadesEuropeias { get; set; }
        public ObservableCollection<Cidade> CidadesMundo { get; set; }

        public HomePage(ApiService apiService, ApiWeatherService apiWeatherService, IValidator validator)
        {
            InitializeComponent();
            _apiService = apiService;
            _apiWeatherService = apiWeatherService;
            _validator = validator;

            // Dados de exemplo com caminhos de imagens estáticas
            CidadesPortuguesas = new ObservableCollection<Cidade>
            {
                new Cidade { Nome = "Lisboa", CaminhoImagem = "lisboa.png" },
                new Cidade { Nome = "Porto", CaminhoImagem = "porto.png" },
                new Cidade { Nome = "Coimbra", CaminhoImagem = "coimbra.png" },
                new Cidade { Nome = "Braga", CaminhoImagem = "braga.png" },
                new Cidade { Nome = "Faro", CaminhoImagem = "faro.png" },
                new Cidade { Nome = "Aveiro", CaminhoImagem = "aveiro.png" },
                new Cidade { Nome = "Évora", CaminhoImagem = "evora.png" },
                new Cidade { Nome = "Leiria", CaminhoImagem = "leiria.png" },
                new Cidade { Nome = "Viseu", CaminhoImagem = "viseu.png" },
                new Cidade { Nome = "Setúbal", CaminhoImagem = "setubal.png" }
            };

            CidadesEuropeias = new ObservableCollection<Cidade>
            {
                new Cidade { Nome = "Madrid", CaminhoImagem = "madrid.png" },
                new Cidade { Nome = "Paris", CaminhoImagem = "paris.png" },
                new Cidade { Nome = "Londres", CaminhoImagem = "londres.png" },
                new Cidade { Nome = "Berlim", CaminhoImagem = "berlim.png" },
                new Cidade { Nome = "Milão", CaminhoImagem = "milao.png" },
                new Cidade { Nome = "Amsterdã", CaminhoImagem = "amsterda.png" },
                new Cidade { Nome = "Bruxelas", CaminhoImagem = "bruxelas.png" },
                new Cidade { Nome = "Viena", CaminhoImagem = "viena.png" },
                new Cidade { Nome = "Praga", CaminhoImagem = "praga.png" },
                new Cidade { Nome = "Atenas", CaminhoImagem = "atenas.png" }
            };

            CidadesMundo = new ObservableCollection<Cidade>
            {
                new Cidade { Nome = "Nova York", CaminhoImagem = "nova_york.png" },
                new Cidade { Nome = "Bangkok", CaminhoImagem = "bangkok.png" },
                new Cidade { Nome = "Sydney", CaminhoImagem = "sydney.png" },
                new Cidade { Nome = "Rio de Janeiro", CaminhoImagem = "rio_de_janeiro.png" },
                new Cidade { Nome = "Cidade do Cabo", CaminhoImagem = "cidade_do_cabo.png" },
                new Cidade { Nome = "Moscou", CaminhoImagem = "moscou.png" },
                new Cidade { Nome = "Dubai", CaminhoImagem = "dubai.png" },
                new Cidade { Nome = "Mumbai", CaminhoImagem = "mumbai.png" },
                new Cidade { Nome = "Buenos Aires", CaminhoImagem = "buenos_aires.png" }
            };

            BindingContext = this;
        }

        private async void OnCidadeSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count > 0)
            {
                var selectedCidade = e.CurrentSelection[0] as Cidade;
                if (selectedCidade != null)
                {
                    var weatherData = await _apiWeatherService.GetWeatherAsync(selectedCidade.Nome);
                    var cityImageUrl = await _apiWeatherService.GetCityImageAsync(selectedCidade.Nome);
                    if (weatherData != null)
                    {
                        weatherData.CityImageUrl = cityImageUrl;
                        await Navigation.PushAsync(new DetalhesCidadePage(weatherData));
                    }
                }
            }
        }
    }

    public class Cidade
    {
        public string Nome { get; set; }
        public string CaminhoImagem { get; set; }
    }
}
