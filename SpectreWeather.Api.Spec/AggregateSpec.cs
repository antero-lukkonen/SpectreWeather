namespace SpectreWeather.Api.Spec
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PublicModel;
    using PublicModel.Spec.Fixtures;

    [TestClass]
    public class AggregateAverageSpec
    {
        [TestMethod]
        public void SetsSourceIdToAggregateAverage()
        {
            var aggregate = Aggregate.Average(new[]
            {
                new CurrentConditions(Unique.Pressure, Unique.Temperature, Unique.Humidity, Unique.SourceId)
            });

            Assert.AreEqual("AggregateAverage", aggregate.SourceId);
        }

        [TestMethod]
        public void AveragesTemperature()
        {
            var t1 = Unique.Temperature;
            var t2 = Unique.Temperature;

            var aggregate = Aggregate.Average(new[]
            {
                new CurrentConditions(Unique.Pressure, t1, Unique.Humidity, Unique.SourceId),
                new CurrentConditions(Unique.Pressure, t2, Unique.Humidity, Unique.SourceId)
            });

            Assert.AreEqual(AverageTwoValues(t1.Value, t2.Value), aggregate.Temperature.Value);
        }

        [TestMethod]
        public void AveragesHumidity()
        {
            var h1 = Unique.Humidity;
            var h2 = Unique.Humidity;

            var aggregate = Aggregate.Average(new[]
            {
                new CurrentConditions(Unique.Pressure, Unique.Temperature, h1, Unique.SourceId),
                new CurrentConditions(Unique.Pressure, Unique.Temperature, h2, Unique.SourceId)
            });

            Assert.AreEqual(AverageTwoValues(h1, h2), aggregate.Humidity);
        }

        [TestMethod]
        public void AveragesPressure()
        {
            var p1 = Unique.Pressure;
            var p2 = Unique.Pressure;

            var aggregate = Aggregate.Average(new[]
            {
                new CurrentConditions(p1, Unique.Temperature, Unique.Humidity, Unique.SourceId),
                new CurrentConditions(p2, Unique.Temperature, Unique.Humidity, Unique.SourceId)
            });

            Assert.AreEqual(AverageTwoValues(p1, p2), aggregate.Pressure);
        }

        private static double AverageTwoValues(long h1, long h2)
        {
            return (h1 + h2) / 2;
        }

        private static double AverageTwoValues(double h1, double h2)
        {
            return (h1 + h2) / 2;
        }
    }
}
