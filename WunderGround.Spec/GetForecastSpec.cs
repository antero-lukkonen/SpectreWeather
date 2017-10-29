namespace SpectreWeather.ForecastSource.WunderGround.Spec
{
    using System;
    using System.Globalization;
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

            var getJson = ToFunc(Wrap($"{{\"pressure_mb\":{expectedPressure}}}"));

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedPressure, forecast.Pressure);
        }       

        [TestMethod]
        public void ForecastContainsTemperature()
        {
            var expectedTemperature = Unique.Temperature;
            var tempInCelsius = expectedTemperature.Value / 274.15;
            var getJson = ToFunc(Wrap($"{{\"temp_c\":{tempInCelsius.ToString(CultureInfo.InvariantCulture)}}}"));

            var forecast = GetForecast(getJson);

            Assert.AreEqual(Math.Round(expectedTemperature.Value, 2), Math.Round(forecast.Temperature.Value, 2));
        }        

        [TestMethod]
        public void ForecastContainsHumidity()
        {
            var expectedHumidity = Unique.Humidity;

            var getJson = ToFunc(Wrap($"{{\"relative_humidity\":\"{expectedHumidity}%\"}}"));

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedHumidity, forecast.Humidity);
        }

        [TestMethod]
        public void ForecastSourceIdentifierIsOpenWeatherMap()
        {
            var expectedSourceId = "WunderGround";

            var getJson = ToFunc(Wrap($"{{}}"));

            var forecast = GetForecast(getJson);

            Assert.AreEqual(expectedSourceId, forecast.SourceId);
        }

        private static ICurrentConditions GetForecast(Func<string> getJson)
        {
            return Factory.Getparser()(getJson);
        }

        private static Func<string> ToFunc(string json)
        {
            var getStringMock = new Mock<Func<string>>();
            getStringMock.Setup(f => f()).Returns(json);
            return getStringMock.Object;
        }

        private static string Wrap(string content)
        {
            return $"{{\"current_observation\":{content}}}";
        }
    }
}
