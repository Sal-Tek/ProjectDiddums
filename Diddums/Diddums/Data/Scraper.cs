using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Diddums.Data
{
    internal abstract class Scraper<T>
    {
        protected virtual string httpUserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:46.0) Gecko/20100101 Firefox/46.0";

        /// <summary>
        /// The coordinates of the desired location.
        /// </summary>
        public virtual Coordinates Coordinates { get; set; }
        
        /// <summary>
        /// The desired radius.
        /// </summary>
        public virtual float Radius { get; set; }

        public Scraper(float longitude, float latitude, short radius)
        {
            this.Coordinates = new Coordinates()
            {
                Longitude = longitude,
                Latitude = latitude
            };
            this.Radius = radius;
        }

        /// <summary>
        /// Retrieve the source of the given URL.
        /// </summary>
        /// <param name="url">The URL which the source should be retrieved from.</param>
        /// <returns>String with the source.</returns>
        protected virtual async Task<string> GetSourceAsync(string url)
        {
            WebClient client = new WebClient();
            client.Headers.Add("UserAgent", httpUserAgent);
            return await client.DownloadStringTaskAsync(url);
        }

        /// <summary>
        /// Append the parameters to the URL.
        /// </summary>
        /// <param name="url">The base URL.</param>
        /// <returns>Base URL with appended parameters.</returns>
        protected virtual string AppendParameters(string url, Dictionary<string, object> parameters)
        {
            // #ReadabilityForTheWin
            return $"{url}?{string.Join<string>("&", parameters.Select(pair => $"{pair.Key}={pair.Value}"))}";
        }

        public abstract Task<T> ScrapeAsync();
    }
}