using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Diddums.Data
{
    internal sealed class AirspaceScraper : Scraper<Airspace>
    {
        /// <summary>
        /// Create an instance of the NoFlyZoneScraper class.
        /// </summary>
        /// <param name="longitude">The desired longitude.</param>
        /// <param name="latitude">The desired latitude.</param>
        /// <param name="radius">The desired radius.</param>
        public AirspaceScraper(float longitude, float latitude, short radius) : base(longitude, latitude, radius) { }

        /// <summary>
        /// Scrape airspace data.
        /// </summary>
        /// <returns>Airspace class.</returns>
        public override async Task<Airspace> ScrapeAsync()
        {
            // Construct API call.
            string apiCall = AppData.AirspaceAPI;
            apiCall = AppendParameters(apiCall, new System.Collections.Generic.Dictionary<string, object>()
            {
                { "lat", base.Coordinates.Latitude },
                { "lng", base.Coordinates.Longitude },
                { "rad", Radius }
            });

            // Get data and separate it.
            string data = await GetSourceAsync(apiCall);

            int center = data.IndexOf("][");
            string notams = data.Substring(0, center + 1);
            string noFlyZones = data.Substring(center + 1);
            
            return new Airspace()
            {
                NOTAMZones = JsonConvert.DeserializeObject<NOTAMZone[]>(notams),
                NoFlyZones = JsonConvert.DeserializeObject<NoFlyZone[]>(noFlyZones)
            };
        }
    }
}