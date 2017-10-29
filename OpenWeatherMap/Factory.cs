namespace SpectreWeather.ForecastSource.OpenWeatherMap
{
    using System;
    using Newtonsoft.Json;
    using PublicModel;

    public static class Factory
    {
        public static Func<Coordinates, ICurrentConditions> GetCurrentConditions(string openWeatherMapKey, Func<Uri, string> HttpGet)
        {
            var getJson = GetClient(HttpGet, openWeatherMapKey);
            var parse = GetParser();
            return coordinates => parse(() => getJson(coordinates));
        }

        public static Func<Func<string>, ICurrentConditions> GetParser()
        {
            return getJson =>
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
                return new CurrentConditions(
                    main.pressure, 
                    new Fahrenheit(main.temp),
                    main.humidity,
                    "OpenWeatherMap");
            };
        }

        public static Func<Coordinates, string> GetClient(Func<Uri, string> httpGet, string openWeatherMapKey)
        {
            return c =>
                httpGet(new Uri(
                    $"http://api.openweathermap.org/data/2.5/weather?lat={c.Lat}&lon={c.Lon}&APPID={openWeatherMapKey}"));
        }  
    }
}
