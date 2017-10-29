namespace SpectreWeather.PublicModel
{
    public interface ICurrentConditions
    {
        long Pressure { get; }
        Kelvin Temperature { get; }
        long Humidity { get; }
        string SourceId { get; }
    }
}