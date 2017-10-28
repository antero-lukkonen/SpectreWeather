namespace SpectreWeather.PublicModel
{
    public struct Coordinates
    {
        public Coordinates(double lat, double lon)
        {
            this.Lat = lat;
            this.Lon = lon;
        }

        public double Lat { get; }
        public double Lon { get; }
    }    
}