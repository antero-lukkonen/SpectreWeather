namespace SpectreWeather.PublicModel
{
    using System.Globalization;

    public struct Fahrenheit
    {
        private readonly double value;

        public Fahrenheit(double value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return this.value.ToString(CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Fahrenheit other)
        {
            return this.value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}