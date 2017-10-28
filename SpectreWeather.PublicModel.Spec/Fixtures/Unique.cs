namespace SpectreWeather.PublicModel.Spec.Fixtures
{
    using System;
    using System.Threading;

    public static class Unique
    {
        private static long sequence = new Random(DateTimeOffset.UtcNow.Second).Next(1000, 10000);
        private static long Long => Next();
        private static double Double => Math.Round((double)Next(), 2);

        public static long Pressure => Long;
        public static double Temperature => Double;
        public static long Humidity => Long;

        private static long Next()
        {
            Interlocked.Increment(ref sequence);
            return sequence;
        }
    }
}