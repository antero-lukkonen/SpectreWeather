using System;

namespace SpectreWeather.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using PublicModel;

    public class ForecastApi
    {
        private readonly IEnumerable<Func<Coordinates, ICurrentConditions>> getForecast;
        private Func<IEnumerable<ICurrentConditions>, ICurrentConditions> aggregate;
        private readonly Action<Exception> notifyAboutError;

        public ForecastApi(
            Action<Exception> notifyAboutError, 
            Func<IEnumerable<ICurrentConditions>, ICurrentConditions> aggregate,
            params Func<Coordinates, ICurrentConditions>[] getForecast)
        {
            if (!getForecast.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(getForecast), getForecast, "At least one forecast source is required.");
            }
            this.notifyAboutError = notifyAboutError;
            this.getForecast = getForecast;
            this.aggregate = aggregate;
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
            if (result.Count > 1)
            {
                try
                {
                    var aggregated = this.aggregate(result);
                    if (aggregated != null)
                    {
                        result.Add(aggregated);
                    }
                }
                catch (Exception e)
                {
                    this.notifyAboutError(e);
                }
            }
            return result;
        }
    }
}