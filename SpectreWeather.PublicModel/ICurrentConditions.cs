namespace SpectreWeather.PublicModel
{
    public interface ICurrentConditions
    {
        long Pressure { get; }
        Fahrenheit Temperature { get; }
        long Humidity { get; }
        string SourceId { get; }
    }
}