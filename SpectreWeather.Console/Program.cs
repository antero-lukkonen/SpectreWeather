namespace SpectreWeather.ConsoleApp
{
    using System;
    using Newtonsoft.Json;
    using PublicModel;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var coordinatesForTallinn = new Coordinates(59.436962, 24.753574);

            var func = Factory.Default.GetCurrentConditions(Console.WriteLine);

            Console.WriteLine(JsonConvert.SerializeObject(func(coordinatesForTallinn)));
        }
    }
}
