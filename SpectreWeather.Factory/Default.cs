namespace SpectreWeather.Factory
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using Api;
    using PublicModel;

    public static class Default
    {
        public static Func<Coordinates, IEnumerable<ICurrentConditions>> GetCurrentConditions(Action<Exception> notifyOnError, string openWeatherMapKey, string wunderGroundKey)
        {
            var httpClient = new HttpClient();
            string HttpGet(Uri uri) => httpClient.GetStringAsync(uri).Result;

            var forecastApi = new ForecastApi(notifyOnError,
                ForecastSource.OpenWeatherMap.Factory.GetCurrentConditions(openWeatherMapKey, HttpGet),
                ForecastSource.WunderGround.Factory.GetCurrentConditions(wunderGroundKey, HttpGet));

            return coordinates => forecastApi.GetCurrentConditions(coordinates);
        }
    }
}

