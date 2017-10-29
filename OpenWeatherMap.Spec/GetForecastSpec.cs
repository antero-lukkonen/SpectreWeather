namespace SpectreWeather.ForecastSource.OpenWeatherMap.Spec
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using OpenWeatherMap;
    using PublicModel;
    using PublicModel.Spec.Fixtures;

    [TestClass]
    public class GetForecastSpec
    {
        [TestMethod]
        public void ForecastContainsPressure()
        {
            var expectedPressure = Unique.Pressure;

            var getJson = ToFunc($"{{\"main\":{{\"pressure\":{expectedPressure}}}}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedPressure, forecast.Pressure);
        }       

        [TestMethod]
        public void ForecastContainsTemperature()
        {
            var expectedTemperature = Unique.Temperature;

            var getJson = ToFunc($"{{\"main\":{{\"temp\":{expectedTemperature}}}}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedTemperature, forecast.Temperature);
        }

        [TestMethod]
        public void ForecastContainsHumidity()
        {
            var expectedHumidity = Unique.Humidity;

            var getJson = ToFunc($"{{\"main\":{{\"humidity\":{expectedHumidity}}}}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedHumidity, forecast.Humidity);
        }

        [TestMethod]
        public void ForecastSourceIdentifierIsOpenWeatherMap()
        {
            var expectedSourceId = "OpenWeatherMap";

            var getJson = ToFunc($"{{\"main\":{{}}}}");

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedSourceId, forecast.SourceId);
        }

        private static ICurrentConditions GetForecast(Func<string> getJson)
        {
            return Factory.GetParser()(getJson);
        }

        private static Func<string> ToFunc(string json)
        {
            var getStringMock = new Mock<Func<string>>();
            getStringMock.Setup(f => f()).Returns(json);
            return getStringMock.Object;
        }
    }
}
