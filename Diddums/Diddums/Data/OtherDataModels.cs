using Newtonsoft.Json;

namespace Diddums.Data
{
    internal struct Coordinates
    {
        /// <summary>
        /// City longitude.
        /// </summary>
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// City latitude.
        /// </summary>
        [JsonProperty("lon")]
        public double Longitude { get; set; }
    }
}