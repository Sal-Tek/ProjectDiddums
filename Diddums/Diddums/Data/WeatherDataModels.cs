using Newtonsoft.Json;

namespace Diddums.Data
{
    internal class Climate
    {
        /// <summary>
        /// City coordinates.
        /// </summary>
        [JsonProperty("coord")]
        public Coordinates Coordinates { get; set; }

        /// <summary>
        /// The weather.
        /// </summary>
        [JsonProperty("weather")]
        public Weather[] Weather { get; set; }

        /// <summary>
        /// Internal parameter.
        /// </summary>
        [JsonProperty("base")]
        public string Base { get; set; }

        /// <summary>
        /// Main weather information.
        /// </summary>
        [JsonProperty("main")]
        public Main Main { get; set; }

        /// <summary>
        /// Wind information.
        /// </summary>
        [JsonProperty("wind")]
        public Wind Wind { get; set; }

        /// <summary>
        /// Cloud information.
        /// </summary>
        [JsonProperty("clouds")]
        public Clouds Clouds { get; set; }

        /// <summary>
        /// Rain information.
        /// </summary>
        [JsonProperty("rain")]
        public Rain Rain { get; set; }

        /// <summary>
        /// Snow information.
        /// </summary>
        [JsonProperty("snow")]
        public Snow Snow { get; set; }

        /// <summary>
        /// Time of data calculate, unix, UTC.
        /// </summary>
        [JsonProperty("dt")]
        public long Time { get; set; }

        [JsonProperty("sys")]
        public Sys System { get; set; }

        /// <summary>
        /// City ID.
        /// </summary>
        [JsonProperty("id")]
        public int CityId { get; set; }

        /// <summary>
        /// City name.
        /// </summary>
        [JsonProperty("name")]
        public string CityName { get; set; }

        /// <summary>
        /// Internal parameter.
        /// </summary>
        [JsonProperty("cod")]
        public string Code { get; set; }
    }

    /* DATA MODELS THAT ARE USED IN THE MODEL ABOVE
    --------------------------------------------------*/
    
    internal class Weather
    {
        /// <summary>
        /// Weather condition ID.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Group of weather parameters. (Rain, Snow, Extreme etc.)
        /// </summary>
        [JsonProperty("main")]
        public string Main { get; set; }

        /// <summary>
        /// Weather condition within the group.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Weather icon ID.
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }

    internal class Main
    {
        /// <summary>
        /// Temperature in Celcius by default.
        /// </summary>
        [JsonProperty("temp")]
        public float Temperature { get; set; }

        /// <summary>
        /// Atmospheric pressure.
        /// </summary>
        [JsonProperty("pressure")]
        public float Pressure { get; set; }

        /// <summary>
        /// Humidity percentage.
        /// </summary>
        [JsonProperty("humidity")]
        public int Humidity { get; set; }

        /// <summary>
        /// Minimum temperature at the moment.
        /// </summary>
        [JsonProperty("temp_min")]
        public float MinimumTemperature { get; set; }

        /// <summary>
        /// Maximum temperature at the moment.
        /// </summary>
        [JsonProperty("temp_max")]
        public float MaximumTemperature { get; set; }
    }

    internal class Wind
    {
        /// <summary>
        /// Wind speed in meters/second by default.
        /// </summary>
        [JsonProperty("speed")]
        public float Speed { get; set; }

        /// <summary>
        /// Wind direction in meteorological degrees.
        /// </summary>
        [JsonProperty("deg")]
        public float Degrees { get; set; }
    }

    internal class Clouds
    {
        /// <summary>
        /// Cloudliness percentage.
        /// </summary>
        [JsonProperty("all")]
        public int All { get; set; }
    }

    internal class Rain
    {
        /// <summary>
        /// Rain volume for the last 3 hours.
        /// </summary>
        [JsonProperty("3h")]
        public int RainVolume { get; set; }
    }

    internal class Snow
    {
        /// <summary>
        /// Snow volume for the last 3 hours.
        /// </summary>
        [JsonProperty("3h")]
        public int SnowVolume { get; set; }
    }
    
    // Had to use "sys" because "System" would have clashed with the "System" namespace.
    internal class Sys
    {
        /// <summary>
        /// Internal parameter.
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Internal parameter.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Internal parameter.
        /// </summary>
        [JsonProperty("message")]
        public float Message { get; set; }

        /// <summary>
        /// Country code. (GB, JP etc.)
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// Sunrise time, unix, UTC.
        /// </summary>
        [JsonProperty("sunrise")]
        public long Sunrise { get; set; }

        /// <summary>
        /// Sunset time, unix, UTC.
        /// </summary>
        [JsonProperty("sunset")]
        public long Sunset { get; set; }
    }
}