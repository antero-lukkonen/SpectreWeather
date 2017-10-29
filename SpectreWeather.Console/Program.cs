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

            var func = Factory.Default.GetCurrentConditions(Console.WriteLine, 
                "1cea90c6e3f4c93a70d5af7ca4da8419", 
                "1dd56b419fad8b01");

            Console.WriteLine(JsonConvert.SerializeObject(func(coordinatesForTallinn)));
        }
    }
}
