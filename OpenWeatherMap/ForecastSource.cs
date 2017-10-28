namespace SpectreWeather.ForecastSource.OpenWeatherMap
{
    using System;
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
                    main = new
                    {
                        pressure = (long)0,
                        temp = (double)0,
                        humidity = (long)0
                    }
                };
                var deserialized = JsonConvert.DeserializeAnonymousType(getJson(), schema);
                var main = deserialized.main;
                return new Forecast(
                    main.pressure, 
                    new Fahrenheit(main.temp),
                    main.humidity,
                    "OpenWeatherMap");
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

        public static Func<Coordinates, string> GetClient(Func<Uri, string> httpGet, string openWeatherMapKey)
        {
            return c =>
                httpGet(new Uri(
                    $"http://api.openweathermap.org/data/2.5/weather?lat={c.Lat}&lon={c.Lon}&APPID={openWeatherMapKey}"));
        }
    }
}
