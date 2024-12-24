using WeatherWiseMAUI.Services;
using WeatherWiseMAUI.Models;

namespace WeatherWiseMAUI.Pages
{
    public partial class MapaTempoPage : ContentPage
    {
        private readonly ApiWeatherService _apiWeatherService;

        public MapaTempoPage(ApiWeatherService apiWeatherService)
        {
            InitializeComponent();
            _apiWeatherService = apiWeatherService;
            InitializeMap();
        }

        private void InitializeMap()
        {
            var htmlSource = new HtmlWebViewSource
            {
                Html = $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta name='viewport' content='initial-scale=1.0, user-scalable=no' />
                        <style type='text/css'>
                            html, body, #map {{ height: 100%; margin: 0; padding: 0; }}
                            .info-window-content {{ font-size: 16px; }}
                        </style>
                        <script src='https://maps.googleapis.com/maps/api/js?key=AIzaSyDnrR58VUp7Rvyseh3Ot1lr_YHBeULmLG8'></script>
                        <script type='text/javascript'>
                            function initialize() {{
                                var mapOptions = {{
                                    zoom: 5,
                                    center: new google.maps.LatLng(39.5, -8.0)
                                }};
                                window.map = new google.maps.Map(document.getElementById('map'), mapOptions);
                                window.markers = [];
                            }}
                            google.maps.event.addDomListener(window, 'load', initialize);
                        </script>
                    </head>
                    <body>
                        <div id='map'></div>
                    </body>
                    </html>"
            };

            WeatherMapWebView.Source = htmlSource;
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
                    var pinScript = $@"
                        for (var i = 0; i < window.markers.length; i++) {{
                            window.markers[i].setMap(null);
                        }}
                        window.markers = [];
                        var position = new google.maps.LatLng({weatherData.Coord.Lat}, {weatherData.Coord.Lon});
                        var marker = new google.maps.Marker({{
                            position: position,
                            map: window.map,
                            title: '{city}'
                        }});
                        window.markers.push(marker);
                        var infoWindow = new google.maps.InfoWindow({{
                            content: '<div class=""info-window-content""><strong>{city}</strong><br>Temperatura: {weatherData.Main.Temp}°C<br>Descrição: {weatherData.Weather[0].Description}<br>Umidade: {weatherData.Main.Humidity}%<br>Pressão: {weatherData.Main.Pressure} hPa<br>Vento: {weatherData.Wind.Speed} m/s</div>'
                        }});
                        infoWindow.open(window.map, marker);
                        window.map.setCenter(position);";

                    WeatherMapWebView.Eval(pinScript);
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
    }
}
