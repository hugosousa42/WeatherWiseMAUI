namespace WeatherWiseMAUI.Models
{
    public class ForecastData
    {
        public List<ForecastItem> List { get; set; }

        public class ForecastItem
        {
            public Main Main { get; set; }
            public List<Weather> Weather { get; set; }
            public string DtTxt { get; set; }
            public int Dt { get; set; } 
        }

        public class Main
        {
            public double Temp { get; set; }
        }

        public class Weather
        {
            public string Description { get; set; }
        }
    }
}
