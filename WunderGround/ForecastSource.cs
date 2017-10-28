namespace SpectreWeather.ForecastSource.WunderGround
{
    using System;
    using System.Globalization;
    using Newtonsoft.Json;
    using PublicModel;

    public static class ForecastSource
    {             
        public static Func<IForecast> Get(Func<string> getJson)
        {
            return () =>
            {
                var schema = new
                {
                    current_observation = new {
                        pressure_mb = (double)0,
                        temp_f = (double)0,
                        relative_humidity = ""                    
                    }
                };
                var value = getJson();
                var deserialized = JsonConvert.DeserializeAnonymousType(value, schema);
                var main = deserialized.current_observation;
                var humidity = main.relative_humidity;
                return new Forecast(
                    Convert.ToInt64(main.pressure_mb), 
                    new Fahrenheit(main.temp_f),
                    long.Parse(humidity?.TrimEnd('%') ?? "0"),
                    "WunderGround");
            };
        }

        public class Forecast : IForecast
        {
            public Forecast(long pressure, Fahrenheit temperature, long humidity, string sourceId)
            {
                this.Pressure = pressure;
                this.Temperature = temperature;
                this.Humidity = humidity;
                this.SourceId = sourceId;
            }

            public long Pressure { get; }
            public Fahrenheit Temperature { get; }
            public long Humidity { get; }
            public string SourceId { get; }
        }

        public static Func<Coordinates, string> GetClient(Func<Uri, string> get, string wunderGroundKey)
        {
            return c => 
                get(new Uri($"http://api.wunderground.com/api/{wunderGroundKey}/conditions/q/{c.Lat.ToString(CultureInfo.InvariantCulture)},{c.Lon.ToString(CultureInfo.InvariantCulture)}.json"));
        }
    }
}
