using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Validations;

namespace WeatherWiseMAUI.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly ApiWeatherService _apiWeatherService;
    private readonly IValidator _validator;

    public LoginPage(ApiService apiService, ApiWeatherService apiWeatherService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _apiWeatherService = apiWeatherService;
        _validator = validator;
    }

    private async void BtnSignIn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntEmail.Text))
        {
            await DisplayAlert("Erro", "Informe o email", "Cancelar");
            return;
        }

        if (string.IsNullOrEmpty(EntPassword.Text))
        {
            await DisplayAlert("Erro", "Informe a senha", "Cancelar");
            return;
        }

        var response = await _apiService.Login(EntEmail.Text, EntPassword.Text);

        if (!response.HasError)
        {
            Application.Current!.MainPage = new AppShell(_apiService, _apiWeatherService, _validator);
        }
        else
        {
            await DisplayAlert("Erro", "Algo deu errado", "Cancelar");
        }

    }

    private async void TapRegister_Tapped(object sender, TappedEventArgs e)
    {

        await Navigation.PushAsync(new InscricaoPage(_apiService, _apiWeatherService, _validator));

    }
}