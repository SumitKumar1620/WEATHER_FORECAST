namespace WebApplication2.Services
{
    /// <summary>
    /// Converts weather codes from the API into simple text and emoji icons.
    /// </summary>
    public static class WeatherHelper
    {
        public static string GetDescription(int code)
        {
            if (code == 0) return "Clear sky";
            if (code <= 3) return "Partly cloudy";
            if (code <= 48) return "Foggy";
            if (code <= 55) return "Drizzle";
            if (code <= 65) return "Rain";
            if (code <= 77) return "Snow";
            if (code <= 82) return "Rain showers";
            if (code <= 86) return "Snow showers";
            if (code <= 99) return "Thunderstorm";
            return "Unknown";
        }

        public static string GetIcon(int code)
        {
            if (code == 0) return "☀️";
            if (code <= 3) return "⛅";
            if (code <= 48) return "🌫️";
            if (code <= 55) return "🌦️";
            if (code <= 65) return "🌧️";
            if (code <= 77) return "❄️";
            if (code <= 82) return "🌧️";
            if (code <= 86) return "🌨️";
            if (code <= 99) return "⛈️";
            return "🌡️";
        }
    }
}
