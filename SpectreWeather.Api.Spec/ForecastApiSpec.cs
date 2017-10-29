namespace SpectreWeather.Api.Spec
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Api;
    using Fixtures;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Mocks;
    using Moq;
    using PublicModel;

    [TestClass]
    public class ForecastApiSpec
    {
        private static readonly Coordinates coordinates = Unique.Coordinates;

        [TestMethod]
        public void FiltersOutNullForecasts()
        {   
            var forecast = GetForecast(new [] { ToForecastSource(null) });   
            
            Assert.AreEqual(0, forecast.Length);            
        }

        [TestMethod]
        public void ReturnsResultsFromSuccessfulSourcesIfOneFails()
        {
            var expectedForecasts = new[]
            {
                Unique.CurrentConditions
            };

            var forecastSources = expectedForecasts
                .Select(ToForecastSource)
                .Concat(new[] { GetFailingSource(new Exception()) });
        
            var actualForecasts = GetForecast(forecastSources);

            Asserts.MultipleForecastsReturned(expectedForecasts, actualForecasts);
        }
       
        [TestMethod]
        public void NotifiesCallingCodeAboutFailedSource()
        {
            var expectedException = new Exception();

            var failingSource = GetFailingSource(expectedException);

            var onErrorMock = new Mock<Action<Exception>>();

            GetForecast(new [] { failingSource }, onErrorMock.Object);

            onErrorMock.Verify(f => f(expectedException));            
        }

        [TestMethod]
        public void ForecastsAreReturnedFromOneApi()
        {
            var expectedForecasts = new[]
            {
                Unique.CurrentConditions
            };

            var forecastSources = expectedForecasts.Select(ToForecastSource);

            var actualForecasts = GetForecast(forecastSources);

            Asserts.MultipleForecastsReturned(expectedForecasts, actualForecasts);
        }

        [TestMethod]
        public void ForecastsAreReturnedFromMultipleApis()
        {
            var expectedForecasts = new[]
            {
                Unique.CurrentConditions,
                Unique.CurrentConditions
            };

            var forecastSources = expectedForecasts.Select(ToForecastSource);

            var actualForecasts = GetForecast(forecastSources);

            Asserts.MultipleForecastsReturned(expectedForecasts, actualForecasts);
        }

        [TestMethod]
        public void EmptyForecastSourcesAreNotAllowed()
        {
            var forecastSources = Unique.EmptyForecastSources();
            const string expectedMessage = "At least one forecast source is required.";
            var expectedValue = forecastSources;
                        
            Asserts.Exception<ArgumentOutOfRangeException>(
                () => GetForecast(forecastSources), 
                e => {
                    Assert.IsTrue(e.Message.StartsWith(expectedMessage), $"{e.Message} does not start with {expectedMessage}");
                    CollectionAssert.AreEqual(expectedValue, (ICollection)e.ActualValue);
                });   
        }

        private static Func<Coordinates, ICurrentConditions> ToForecastSource(ICurrentConditions currentConditions)
        {
            return new GetForecastMock().SetupForecast(currentConditions).Object;
        }

        private static Func<Coordinates, ICurrentConditions> GetFailingSource(Exception exception)
        {
            return new GetForecastMock().SetupThrows(exception).Object;
        }

        private static ICurrentConditions[] GetForecast(IEnumerable<Func<Coordinates, ICurrentConditions>> forecastSources, Action<Exception> onError = null)
        {
            return new ForecastApi(onError ?? DoNothing, forecastSources.ToArray()).GetCurrentConditions(coordinates).ToArray();
        }

        private static void DoNothing<T>(T obj)
        {            
        }
    }
}
