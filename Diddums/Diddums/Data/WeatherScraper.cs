using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Diddums.Data
{
    internal sealed class WeatherScraper : Scraper<Climate>
    {
        /// <summary>
        /// Create an instance of the WeatherScraper class.
        /// </summary>
        /// <param name="longitude">The desired longitude.</param>
        /// <param name="latitude">The desired latitude.</param>
        /// <param name="radius">The desired radius.</param>
        public WeatherScraper(float longitude, float latitude, short radius) : base(longitude, latitude, radius) { }

        /// <summary>
        /// Scrape weather data.
        /// </summary>
        /// <returns>Weather class.</returns>
        public override async Task<Climate> ScrapeAsync()
        {
            // Construct API call.
            string apiCall = AppData.OpenWeatherMapAPI;
            apiCall = AppendParameters(apiCall, new System.Collections.Generic.Dictionary<string, object>()
            {
                { "lat", base.Coordinates.Latitude },
                { "lon", base.Coordinates.Longitude },
                { "units", "metric" },
                { "appid", AppData.OpenWeatherMapAPIKey }
            });

            string data = await GetSourceAsync(apiCall);

            return JsonConvert.DeserializeObject<Climate>(data);
        }
    }

    // Had to use "UnitSystem" instead of "System" because it would have clashed with the "System" namespace.
    internal enum UnitSystem
    {
        Imperial,
        Metric
    }
}