namespace SpectreWeather.Api.Spec.Fixtures
{
    using PublicModel;
    using PublicModel.Spec.Mocks;

    internal static class Unique
    {
        public static IForecast Forecast => new ForecastMock().Object;
    }
}