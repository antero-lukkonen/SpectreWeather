namespace SpectreWeather.PublicModel
{
    public class CurrentConditions : ICurrentConditions
    {
        public CurrentConditions(long pressure, Kelvin temperature, long humidity, string sourceId)
        {
            this.Pressure = pressure;
            this.Temperature = temperature;
            this.Humidity = humidity;
            this.SourceId = sourceId;
        }

        public long Pressure { get; }
        public Kelvin Temperature { get; }
        public long Humidity { get; }
        public string SourceId { get; }
    }    
}