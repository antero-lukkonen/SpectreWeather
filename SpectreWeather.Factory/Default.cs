namespace SpectreWeather.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Api;
    using PublicModel;

    public static class Default
    {
        public static Func<Coordinates, IEnumerable<IForecast>> GetCurrentConditions(Action<Exception> notifyOnError)
        {
            var httpClient = new HttpClient();
            string HttpGet(Uri uri) => httpClient.GetStringAsync(uri).Result;

            var openWeatherMapKey = "1cea90c6e3f4c93a70d5af7ca4da8419";           
            var GetOpenWeatherMap = ForecastSource.OpenWeatherMap.ForecastSource.GetClient(HttpGet, openWeatherMapKey);
            var GetWunderGround = ForecastSource.WunderGround.ForecastSource.GetClient(HttpGet, "1dd56b419fad8b01");

            return coordinates => new ForecastApi(new[]
            {
                ForecastSource.OpenWeatherMap.ForecastSource.Get(() => GetOpenWeatherMap(coordinates)),
                ForecastSource.WunderGround.ForecastSource.Get(() => GetWunderGround(coordinates))
            }, notifyOnError)
            .GetCurrentConditions();
        }
    }
}
