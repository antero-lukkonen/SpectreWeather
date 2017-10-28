namespace SpectreWeather.PublicModel.Spec.Fixtures
{
    using System;
    using System.Threading;

    public static class Unique
    {
        private static long sequence = new Random(DateTimeOffset.UtcNow.Second).Next(1000, 10000);

        private static long Long => Next();

        private static double Double => Math.Round((double)Next() / 10, 2);

        public static string String => Next().ToString();

        public static long Pressure => Long;

        public static Fahrenheit Temperature => new Fahrenheit(Double);

        public static long Humidity => Long;

        public static string SourceId => String;
        public static Coordinates Coordinates => new Coordinates(Double, Double);

        private static long Next()
        {
            Interlocked.Increment(ref sequence);
            return sequence;
        }
    }
}