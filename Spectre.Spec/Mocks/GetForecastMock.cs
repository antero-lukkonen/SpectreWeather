namespace SpectreWeather.Api.Spec.Mocks
{
    using System;
    using Moq;
    using PublicModel;

    public class GetForecastMock : Mock<Func<IForecast>>
    {
        public GetForecastMock SetupForecast(IForecast expectedForecast)
        {
            this.Setup(f => f()).Returns(expectedForecast);
            return this;
        }

        public GetForecastMock SetupThrows(Exception exception)
        {
            this.Setup(f => f()).Throws(exception);
            return this;
        }
    }    
}