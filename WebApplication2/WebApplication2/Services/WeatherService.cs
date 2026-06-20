using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Newtonsoft.Json.Linq;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    /// <summary>
    /// Gets weather data from the free Open-Meteo API (no API key needed).
    /// </summary>
    public class WeatherService
    {
        public WeatherViewModel GetWeatherByCity(string cityName)
        {
            var result = new WeatherViewModel { SearchCity = cityName };

            if (string.IsNullOrWhiteSpace(cityName))
            {
                result.ErrorMessage = "Please enter a city name.";
                return result;
            }

            try
            {
                var location = FindCity(cityName.Trim());
                if (location == null)
                {
                    result.ErrorMessage = "City not found. Try another name (example: London, Mumbai, New York).";
                    return result;
                }

                return GetWeatherByCoordinates(location.Latitude, location.Longitude, location.Name, location.Country, cityName);
            }
            catch (WebException)
            {
                result.ErrorMessage = "Could not connect to weather service. Check your internet connection.";
                return result;
            }
            catch (Exception)
            {
                result.ErrorMessage = "Something went wrong. Please try again.";
                return result;
            }
        }

        public WeatherViewModel GetWeatherByCoordinates(double latitude, double longitude, string cityName, string country, string searchText)
        {
            var result = new WeatherViewModel { SearchCity = searchText };

            try
            {
                var url = string.Format(
                    CultureInfo.InvariantCulture,
                    "https://api.open-meteo.com/v1/forecast?latitude={0}&longitude={1}&current=temperature_2m,relative_humidity_2m,wind_speed_10m,weather_code&daily=weather_code,temperature_2m_max,temperature_2m_min&timezone=auto&forecast_days=7",
                    latitude,
                    longitude);

                var json = DownloadJson(url);
                var data = JObject.Parse(json);

                var current = data["current"];
                int weatherCode = current["weather_code"].Value<int>();

                result.CityName = cityName;
                result.Country = country;
                result.Temperature = current["temperature_2m"].Value<double>();
                result.Humidity = current["relative_humidity_2m"].Value<double>();
                result.WindSpeed = current["wind_speed_10m"].Value<double>();
                result.WeatherDescription = WeatherHelper.GetDescription(weatherCode);
                result.WeatherIcon = WeatherHelper.GetIcon(weatherCode);
                result.DailyForecasts = BuildDailyForecast(data["daily"]);
            }
            catch (WebException)
            {
                result.ErrorMessage = "Could not connect to weather service. Check your internet connection.";
            }
            catch (Exception)
            {
                result.ErrorMessage = "Something went wrong. Please try again.";
            }

            return result;
        }

        private static LocationInfo FindCity(string cityName)
        {
            var url = "https://geocoding-api.open-meteo.com/v1/search?name=" + Uri.EscapeDataString(cityName) + "&count=1&language=en&format=json";
            var json = DownloadJson(url);
            var data = JObject.Parse(json);

            var results = data["results"];
            if (results == null || !results.HasValues)
            {
                return null;
            }

            var first = results[0];
            return new LocationInfo
            {
                Name = first["name"].Value<string>(),
                Country = first["country"].Value<string>(),
                Latitude = first["latitude"].Value<double>(),
                Longitude = first["longitude"].Value<double>()
            };
        }

        private static List<DailyForecast> BuildDailyForecast(JToken daily)
        {
            var list = new List<DailyForecast>();
            var dates = daily["time"];
            var maxTemps = daily["temperature_2m_max"];
            var minTemps = daily["temperature_2m_min"];
            var codes = daily["weather_code"];

            for (int i = 0; i < dates.Count(); i++)
            {
                var date = DateTime.Parse(dates[i].Value<string>());
                int code = codes[i].Value<int>();

                list.Add(new DailyForecast
                {
                    DayName = i == 0 ? "Today" : date.ToString("ddd"),
                    MaxTemp = maxTemps[i].Value<double>(),
                    MinTemp = minTemps[i].Value<double>(),
                    Description = WeatherHelper.GetDescription(code),
                    Icon = WeatherHelper.GetIcon(code)
                });
            }

            return list;
        }

        private static string DownloadJson(string url)
        {
            // Required for HTTPS on some Windows setups
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            using (var client = new WebClient())
            {
                client.Headers.Add("User-Agent", "WeatherForecastApp/1.0");
                return client.DownloadString(url);
            }
        }

        private class LocationInfo
        {
            public string Name { get; set; }
            public string Country { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
    }
}
