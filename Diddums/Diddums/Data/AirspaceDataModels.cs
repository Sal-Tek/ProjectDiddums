using Newtonsoft.Json;

namespace Diddums.Data
{
    internal class Airspace
    {
        /// <summary>
        /// All the NOTAM zones.
        /// </summary>
        public NOTAMZone[] NOTAMZones { get; set; }

        /// <summary>
        /// All the no fly zones.
        /// </summary>
        public NoFlyZone[] NoFlyZones { get; set; }
    }
    
    internal class Zone
    {
        /// <summary>
        /// The distance from the input coordinates.
        /// </summary>
        [JsonProperty("distance")]
        public decimal Distance { get; set; }

        /// <summary>
        /// Polypoints for the zone.
        /// </summary>
        [JsonProperty("polypoints")]
        public Coordinates[] Polypoints { get; set; }

        /// <summary>
        /// Zone ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }

    internal class NoFlyZone : Zone
    {
        /// <summary>
        /// The category of the zone. (A, B or C)
        /// </summary>
        [JsonProperty("airCategory")]
        public string Category { get; set; }

        /// <summary>
        /// Name of no fly zone.
        /// </summary>
        [JsonProperty("airName")]
        public string Name { get; set; }

        /// <summary>
        /// Type of no fly zone.
        /// </summary>
        [JsonProperty("airType")]
        public string Type { get; set; }

        /// <summary>
        /// Lower altitude of where the no fly zone is in effect.
        /// </summary>
        [JsonProperty("flLower")]
        public int FlightLower { get; set; }

        /// <summary>
        /// Upper altitude of where the no fly zone is in effect.
        /// </summary>
        [JsonProperty("flUpper")]
        public int FlightUpper { get; set; }
    }

    internal class NOTAMZone : Zone
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lat")]
        public double Latitude { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("lon")]
        public double Longitude { get; set; }

        /// <summary>
        /// NOTAM reference.
        /// </summary>
        [JsonProperty("ntReference")]
        public string Reference { get; set; }
        
        /// <summary>
        /// NOTAM meaning.
        /// </summary>
        [JsonProperty("ntMeaning")]
        public string Meaning { get; set; }

        /// <summary>
        /// NOTAM suffix.
        /// </summary>
        [JsonProperty("ntSuffix")]
        public string Suffix { get; set; }
    }

    //internal enum AirCategory
    //{
    //    A,
    //    B,
    //    C,
    //    I,
    //    T,
    //    W
    //}
}