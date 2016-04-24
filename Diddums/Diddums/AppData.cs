namespace Diddums
{
    internal static class AppData
    {
        public static string OpenWeatherMapAPI { get; } = "http://api.openweathermap.org/data/2.5/weather";
        public static string OpenWeatherMapAPIKey { get; } = "07f51e15a32773294e3510d6424da6b0";

        public static string AirspaceAPI { get; } = "http://213.152.162.69:57434/query_nofly.php";
    }
}