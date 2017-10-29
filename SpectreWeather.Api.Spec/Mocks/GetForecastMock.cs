namespace SpectreWeather.Api.Spec.Mocks
{
    using System;
    using Moq;
    using PublicModel;

    public class GetForecastMock : Mock<Func<Coordinates, ICurrentConditions>>
    {
        public GetForecastMock SetupForecast(ICurrentConditions expectedCurrentConditions)
        {
            this.Setup(f => f(It.IsAny<Coordinates>())).Returns(expectedCurrentConditions);
            return this;
        }

        public GetForecastMock SetupThrows(Exception exception)
        {
            this.Setup(f => f(It.IsAny<Coordinates>())).Throws(exception);
            return this;
        }
    }    
}