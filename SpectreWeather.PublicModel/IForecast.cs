namespace SpectreWeather.PublicModel
{
    public interface IForecast
    {
        long Pressure { get; }
        double Temperature { get; }
        long Humidity { get; }
    }
}