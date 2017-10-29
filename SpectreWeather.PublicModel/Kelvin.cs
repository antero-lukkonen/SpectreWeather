namespace SpectreWeather.PublicModel
{
    using System.Globalization;

    public struct Kelvin
    {
        private readonly double value;

        public Kelvin(double value)
        {
            this.value = value;
        }

        public double Value => this.value;

        public override string ToString()
        {
            return this.value.ToString(CultureInfo.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public bool Equals(Kelvin other)
        {
            return this.value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }
    }
}