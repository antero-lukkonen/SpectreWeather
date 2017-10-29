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
            var expectedForecast = Unique.CurrentConditions;

            var forecast = GetForecast(new [] { ToForecastSource(null), ToForecastSource(expectedForecast) });   
            
            Assert.AreEqual(1, forecast.Length);
            Asserts.AllForecastsReturned(new []{expectedForecast}, forecast);            
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

            Asserts.AllForecastsReturned(expectedForecasts, actualForecasts);
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

            Asserts.AllForecastsReturned(expectedForecasts, actualForecasts);
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

            Asserts.AllForecastsReturned(expectedForecasts, actualForecasts);
        }

        [TestMethod]
        public void DoesNotAddAggregatedIfOneForecastReturned()
        {
            var actualForecasts = GetForecast(new[]
            {
                ToForecastSource(Unique.CurrentConditions)
            });

            Assert.AreEqual(1, actualForecasts.Length);
        }

        [TestMethod]
        public void DoesNotAddAggregatedForecastIfZeroForecastsReturned()
        {
            var actualForecasts = GetForecast(new[]
            {
                ToForecastSource(Unique.CurrentConditions)
            });

            Assert.AreEqual(1, actualForecasts.Length);
        }

        [TestMethod]
        public void CallsTheAggregationFunctionToAddAggregatedForecast()
        {
            var actualForecasts = GetForecast(new[]
            {
                ToForecastSource(Unique.CurrentConditions)
            }, null, new Mock<Func<IEnumerable<ICurrentConditions>, ICurrentConditions>>().Object);

            Assert.AreEqual(1, actualForecasts.Length);
        }

        [TestMethod]
        public void AddsAggregatedForecastIfMoreThanOneForecastsReturned()
        {
            var actualForecasts = GetForecast(new []
            {
                ToForecastSource(Unique.CurrentConditions),
                ToForecastSource(Unique.CurrentConditions)
            });

            Assert.AreEqual(3, actualForecasts.Length);
        }

        [TestMethod]
        public void CallsGivenAggregateFuncToCreateTheAggregation()
        {
            var expectedAggregate = Unique.CurrentConditions;
            var currentConditions1 = Unique.CurrentConditions;
            var currentConditions2 = Unique.CurrentConditions;

            var aggregateMock = new Mock<Func<IEnumerable<ICurrentConditions>, ICurrentConditions>>();
            aggregateMock.Setup(f => f(It.IsAny<IEnumerable<ICurrentConditions>>())).Returns(expectedAggregate);
            var aggregate = aggregateMock.Object;
            
            GetForecast(new[]
            {
                ToForecastSource(currentConditions1),
                ToForecastSource(currentConditions2)
            }, DoNothing, aggregate);

            Func<IEnumerable<ICurrentConditions>, bool> Xxx = conditions =>
            {
                var cond = conditions.ToList();
                Assert.AreEqual(3, cond.Count);
                Assert.IsTrue(cond.Contains(currentConditions1));
                Assert.IsTrue(cond.Contains(currentConditions2));
                Assert.IsTrue(cond.Contains(expectedAggregate));
                return true;
            };

            aggregateMock.Verify(f => f(It.Is<IEnumerable<ICurrentConditions>>(conditions => Xxx(conditions))));
        }

        [TestMethod]
        public void NotifiesAboutAggregateErrors()
        {
            var expectedException = new Exception();

            Exception exception = null;
            GetForecast(new[]
            {
                ToForecastSource(Unique.CurrentConditions),
                ToForecastSource(Unique.CurrentConditions)
            }, 
            e => exception = e,
            x => throw expectedException);

            Assert.AreEqual(expectedException, exception);
        }

        [TestMethod]
        public void ReturnsOtherResultsIfAggregationFails()
        {
            var currentConditions = new[]
            {
                Unique.CurrentConditions,
                Unique.CurrentConditions
            };
          
            var actualConditions = GetForecast(currentConditions.Select(ToForecastSource),
                DoNothing,
                x => throw new Exception());

            Assert.AreEqual(2, actualConditions.Length);
            Asserts.AllForecastsReturned(currentConditions, actualConditions);
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

        private static ICurrentConditions[] GetForecast(
            IEnumerable<Func<Coordinates, ICurrentConditions>> forecastSources, 
            Action<Exception> onError = null, 
            Func<IEnumerable<ICurrentConditions>, ICurrentConditions> aggregator = null)
        {
            return new ForecastApi(
                onError ?? DoNothing, 
                aggregator ?? (enumerable => null),
                forecastSources.ToArray()).GetCurrentConditions(coordinates).ToArray();
        }

        private static void DoNothing<T>(T obj)
        {            
        }
    }
}
