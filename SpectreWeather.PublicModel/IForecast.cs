namespace SpectreWeather.PublicModel
{
    public interface IForecast
    {
        long Pressure { get; }
        Fahrenheit Temperature { get; }
        long Humidity { get; }
        string SourceId { get; }
    }
}