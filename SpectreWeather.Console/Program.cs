namespace SpectreWeather.ConsoleApp
{
    using System;
    using Newtonsoft.Json;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var api = new Api.ForecastApi(new []
            {
                ForecastSource.OpenWeatherMap.ForecastSource.Get(() => "{\"coord\":{\"lon\":139,\"lat\":35},\r\n\"sys\":{\"country\":\"JP\",\"sunrise\":1369769524,\"sunset\":1369821049},\r\n\"weather\":[{\"id\":804,\"main\":\"clouds\",\"description\":\"overcast clouds\",\"icon\":\"04n\"}],\r\n\"main\":{\"temp\":289.5,\"humidity\":89,\"pressure\":1013,\"temp_min\":287.04,\"temp_max\":292.04},\r\n\"wind\":{\"speed\":7.31,\"deg\":187.002},\r\n\"rain\":{\"3h\":0},\r\n\"clouds\":{\"all\":92},\r\n\"dt\":1369824698,\r\n\"id\":1851632,\r\n\"name\":\"Shuzenji\",\r\n\"cod\":200}")
            }, Console.WriteLine);

            Console.WriteLine(JsonConvert.SerializeObject(api.Get()));
        }
    }
}
