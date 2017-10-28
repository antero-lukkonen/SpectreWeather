using System;

namespace SpectreWeather.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using PublicModel;

    public class ForecastApi
    {
        private readonly IEnumerable<Func<IForecast>> getForecast;
        private readonly Action<Exception> notifyAboutError;

        public ForecastApi(IEnumerable<Func<IForecast>> getForecast, Action<Exception> notifyAboutError)
        {
            if (!getForecast.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(getForecast), getForecast, "At least one forecast source is required.");
            }
            this.notifyAboutError = notifyAboutError;
            this.getForecast = getForecast;
        }

        public IEnumerable<IForecast> GetCurrentConditions()
        {
            var result = new List<IForecast>();
            foreach (var func in this.getForecast)
            {
                IForecast forecast = null;
                try
                {
                    forecast = func();
                }
                catch (Exception e)
                {
                    this.notifyAboutError(e);
                }

                if (forecast == null)
                {
                    continue;
                }
                result.Add(forecast);

            }
            return result;
        }
    }
}