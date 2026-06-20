using System.Collections.Generic;

namespace WebApplication2.Models
{
    /// <summary>
    /// Data shown on the weather page.
    /// </summary>
    public class WeatherViewModel
    {
        public string SearchCity { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public List<DailyForecast> DailyForecasts { get; set; }
        public string ErrorMessage { get; set; }

        public bool HasWeatherData
        {
            get { return string.IsNullOrEmpty(ErrorMessage) && !string.IsNullOrEmpty(CityName); }
        }
    }

    /// <summary>
    /// One day in the 7-day forecast.
    /// </summary>
    public class DailyForecast
    {
        public string DayName { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}
