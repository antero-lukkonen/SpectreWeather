using System;

namespace SpectreWeather.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using PublicModel;

    public class ForecastApi
    {
        private readonly IEnumerable<Func<Coordinates, ICurrentConditions>> getForecast;
        private readonly Action<Exception> notifyAboutError;

        public ForecastApi(Action<Exception> notifyAboutError, params Func<Coordinates, ICurrentConditions>[] getForecast)
        {
            if (!getForecast.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(getForecast), getForecast, "At least one forecast source is required.");
            }
            this.notifyAboutError = notifyAboutError;
            this.getForecast = getForecast;
        }

        public IEnumerable<ICurrentConditions> GetCurrentConditions(Coordinates coordinates)
        {
            var result = new List<ICurrentConditions>();
            foreach (var func in this.getForecast)
            {
                ICurrentConditions currentConditions = null;
                try
                {
                    currentConditions = func(coordinates);
                }
                catch (Exception e)
                {
                    this.notifyAboutError(e);
                }

                if (currentConditions == null)
                {
                    continue;
                }
                result.Add(currentConditions);

            }
            return result;
        }
    }
}