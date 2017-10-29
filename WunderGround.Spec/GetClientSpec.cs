using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpectreWeather.ForecastSource.WunderGround.Spec
{
    using System;
    using System.Globalization;
    using Moq;
    using PublicModel;
    using PublicModel.Spec.Fixtures;

    [TestClass]
    public class GetClientSpec
    {
        [TestMethod]
        public void UsesProvidedHttpGetFuncToIssueRequest()
        {
            var httpGetMock = new Mock<Func<Uri, string>>();

            var func = Factory.GetClient(httpGetMock.Object, Unique.String);

            func(Unique.Coordinates);

            httpGetMock.Verify(f => f(It.IsAny<Uri>()));
        }

        [TestMethod]
        public void SendsRequestToWunderGroundApi()
        {
            VerifyUri(uri =>
            {
                Assert.AreEqual("api.wunderground.com", uri.Host);
            }, Unique.Coordinates, Unique.String);
        }

        [TestMethod]
        public void PlacesCoordinatesInTheUriInCultureAgnosticFormat()
        {
            var expectedCoordinates = Unique.Coordinates;
            VerifyUri(uri =>
            {
                var splitPath = uri.PathAndQuery.Split('/');
                var coordinatesString = $"{expectedCoordinates.Lat.ToString(CultureInfo.InvariantCulture)},{expectedCoordinates.Lon.ToString(CultureInfo.InvariantCulture)}.json";
                Assert.AreEqual(coordinatesString, splitPath[5]);
            }, expectedCoordinates, Unique.String);
        }

        [TestMethod]
        public void PlacesApiKeyToPath()
        {
            var apiKey = Unique.String;
            VerifyUri(uri =>
            {
                var expectedPathStart = $"/api/{apiKey}/conditions/q/";
                Assert.IsTrue(uri.PathAndQuery.StartsWith(expectedPathStart));
            }, Unique.Coordinates, apiKey);
        }

        [TestMethod]
        public void UsesHttpProtocol()
        {
            VerifyUri(uri =>
            {
                Assert.AreEqual("http", uri.Scheme);               
            }, Unique.Coordinates, Unique.String);
        }        

        [TestMethod]
        public void ReturnsTheResponseBody()
        {
            var expectedResult = Unique.String;

            var httpGetMock = new Mock<Func<Uri, string>>();
            
            httpGetMock.Setup(f => f(It.IsAny<Uri>())).Returns(expectedResult);

            var func = Factory.GetClient(httpGetMock.Object, Unique.String);

            var result = func(Unique.Coordinates);

            Assert.AreEqual(expectedResult, result);
        }

        private static void VerifyUri(Action<Uri> assert, Coordinates coordinates, string apiKey)
        {
            var httpGetMock = new Mock<Func<Uri, string>>();

            var func = Factory.GetClient(httpGetMock.Object, apiKey);

            func(coordinates);

            Func<Uri, bool> AssertReturn = uri =>
            {
                assert(uri);
                return true;
            };

            httpGetMock.Verify(f => f(It.Is<Uri>(uri => AssertReturn(uri))));
        }
    }
}
