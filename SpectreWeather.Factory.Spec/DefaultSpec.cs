namespace SpectreWeather.Factory.Spec
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PublicModel;

    [TestClass]
    public class DefaultSpec
    {
        [TestMethod]
        public void Queries2WeatherSources()
        {
            var coordinatesForTallinn = new Coordinates(59.436962, 24.753574);

            var func = Default.GetCurrentConditions(o => throw o, "1cea90c6e3f4c93a70d5af7ca4da8419", "1dd56b419fad8b01");
            
            var response = func(coordinatesForTallinn);

            Assert.AreEqual(2, response.Count());
        }
    }
}
