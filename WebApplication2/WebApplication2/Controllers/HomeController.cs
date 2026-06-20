using System.Web.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly WeatherService _weatherService = new WeatherService();

        // GET: Home - search by city name or GPS coordinates
        public ActionResult Index(string city, double? lat, double? lon)
        {
            if (lat.HasValue && lon.HasValue)
            {
                var model = _weatherService.GetWeatherByCoordinates(
                    lat.Value,
                    lon.Value,
                    "Your Location",
                    "",
                    city ?? "");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                return View(new WeatherViewModel());
            }

            var cityModel = _weatherService.GetWeatherByCity(city);
            return View(cityModel);
        }
    }
}
