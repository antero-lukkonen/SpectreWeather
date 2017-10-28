namespace SpectreWeather.ConsoleApp
{
    using System;
    using Newtonsoft.Json;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var func = Factory.Default.GetCurrentConditions(Console.WriteLine);
            Console.WriteLine(JsonConvert.SerializeObject(func()));
        }
    }
}
