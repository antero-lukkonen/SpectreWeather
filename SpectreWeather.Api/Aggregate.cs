namespace SpectreWeather.Api
{
    using System.Collections.Generic;
    using System.Linq;
    using PublicModel;

    public class Aggregate
    {
        public static ICurrentConditions Average(IEnumerable<ICurrentConditions> enumerable)
        {
            var conditions = enumerable.ToArray();
            return new CurrentConditions(
                (long)conditions.Average(x => x.Pressure), 
                new Kelvin(conditions.Average(x => x.Temperature.Value)), 
                (long)conditions.Average(x => x.Humidity), 
                "AggregateAverage");
        }


    }
}
