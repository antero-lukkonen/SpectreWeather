namespace SpectreWeather.ForecastSource.WunderGround.Spec
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PublicModel;
    using PublicModel.Spec.Fixtures;

    [TestClass]
    public class GetForecastSpec
    {
        [TestMethod]
        public void ForecastContainsPressure()
        {
            var expectedPressure = (double)Unique.Pressure;

            var getJson = ToFunc($"{{\"pressure_mb\":{expectedPressure}}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedPressure, forecast.Pressure);
        }       

        [TestMethod]
        public void ForecastContainsTemperature()
        {
            var expectedTemperature = Unique.Temperature;

            var getJson = ToFunc($"{{\"temp_f\":{expectedTemperature}}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedTemperature, forecast.Temperature);
        }

        [TestMethod]
        public void ForecastContainsHumidity()
        {
            var expectedHumidity = Unique.Humidity;

            var getJson = ToFunc($"{{\"relative_humidity\":\"{expectedHumidity}%\"}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedHumidity, forecast.Humidity);
        }

        [TestMethod]
        public void ForecastSourceIdentifierIsOpenWeatherMap()
        {
            var expectedSourceId = "WunderGround";

            var getJson = ToFunc($"{{}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedSourceId, forecast.SourceId);
        }

        private static IForecast GetForecast(Func<string> getJson)
        {
            return ForecastSource.Get(getJson)();
        }

        private static Func<string> ToFunc(string json)
        {
            var getStringMock = new Mock<Func<string>>();
            getStringMock.Setup(f => f()).Returns(json);
            return getStringMock.Object;
        }
    }
}
