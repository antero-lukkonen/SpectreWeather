namespace SpectreWeather.ForecastSource.WunderGround
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using PublicModel;

    public static partial class Factory
    {
        public static Func<Coordinates, ICurrentConditions> GetCurrentConditions(string wunderGroundKey, Func<Uri, string> HttpGet)
        {
            var getJson = GetClient(HttpGet, wunderGroundKey);
            var parse = Getparser();
            return coordinates => parse(() => getJson(coordinates));
        }

        public static Func<Func<string>, ICurrentConditions> Getparser()
        {
            return getJson =>
            {
                var schema = new
                {
                    current_observation = new {
                        pressure_mb = (double)0,
                        temp_c = (double)0,
                        relative_humidity = ""                    
                    }
                };
                var value = getJson();
                var deserialized = JsonConvert.DeserializeAnonymousType(value, schema);
                var main = deserialized.current_observation;
                var humidity = main.relative_humidity;
                return new CurrentConditions(
                    Convert.ToInt64(main.pressure_mb), 
                    new Kelvin(main.temp_c * 274.15),
                    long.Parse(humidity?.TrimEnd('%') ?? "0"),
                    "WunderGround");
            };
        }

        public static Func<Coordinates, string> GetClient(Func<Uri, string> get, string wunderGroundKey)
        {
            return c =>
                get(new Uri($"http://api.wunderground.com/api/{wunderGroundKey}/conditions/q/{c.Lat.ToString(CultureInfo.InvariantCulture)},{c.Lon.ToString(CultureInfo.InvariantCulture)}.json"));
        }
    }
}
