namespace SpectreWeather.Api.Spec.Fixtures
{
    using PublicModel;
    using PublicModel.Spec.Mocks;

    internal static class Unique
    {
        public static ICurrentConditions CurrentConditions => new ForecastMock().Object;
        public static Coordinates Coordinates => PublicModel.Spec.Fixtures.Unique.Coordinates;
    }
}