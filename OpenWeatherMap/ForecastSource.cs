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
                    main.temp,
                    main.humidity);
            };
        }

        public class Forecast : IForecast
        {
            public Forecast(long pressure, double temperature, long humidity)
            {
                this.Pressure = pressure;
                this.Temperature = temperature;
                this.Humidity = humidity;
            }

            public long Pressure { get; }
            public double Temperature { get; }
            public long Humidity { get; }
        }
    }
}
