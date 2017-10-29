namespace SpectreWeather.Factory.Spec
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PublicModel;

    [TestClass]
    public class DefaultSpec
    {
        private static IEnumerable<ICurrentConditions> currentConditions;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            var coordinatesForTallinn = new Coordinates(59.436962, 24.753574);

            var func = Default.GetCurrentConditions(o => throw o, "1cea90c6e3f4c93a70d5af7ca4da8419", "1dd56b419fad8b01");

            currentConditions = func(coordinatesForTallinn);
        }

        [TestMethod]
        public void Queries2WeatherSourcesAndAddsAggregate()
        {         
            Assert.AreEqual(3, currentConditions.Count());            
        }

        [TestMethod]
        public void QueriesOpenWeatherMap()
        {
            Assert.IsTrue(currentConditions.Single(c => c.SourceId == "OpenWeatherMap") != null);
        }

        [TestMethod]
        public void QueriesWunderGround()
        {
            Assert.IsTrue(currentConditions.Single(c => c.SourceId == "WunderGround") != null);
        }
    }
}
