using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherForecastApp.Contract;
using WeatherForecastApp.Extensions;
using WeatherForecastApp.Implementation;
using WeatherForecastApp.Models;

namespace WeatherForecastApp
{
    public class AppEntry
    {
        private readonly WeatherApiConfig _configuration;
        private readonly ILogger _logger;
        private readonly IWeatherStackClient _client;
        private ICurrentWeatherProcessor _weatherProcessor;

        public AppEntry(IOptions<WeatherApiConfig> options, ILogger<AppEntry> logger, IWeatherStackClient client)
        {
            _configuration = options.Value;
            _logger = logger;
            _client = client;
            
        }

        public string IsWeatherSuitableOutSide { get; set; }


        public async Task Run(String[] args)
        {
            _logger.LogInformation("Console is running....");

            Console.WriteLine("Please enter a valid zipcode:");

            string zipCode = Console.ReadLine();

            var weatherResult = await _client.GetCurrentWeather(zipCode);
                 

            if (!weatherResult.IsValid)
            {
                _logger.LogError(weatherResult.ErrorMessage);
                Console.WriteLine(weatherResult.ErrorMessage);
                return;
            }

            var currentWeather = weatherResult.Response.CurrentWeather;

            _weatherProcessor = new CurrentWeatherProcessor(currentWeather);

            Console.WriteLine($"Should I go outside? { _weatherProcessor.IsWeatherSuitableOutside().ToYesNoString() }");
            Console.WriteLine($"Should I wear sunscreen? { _weatherProcessor.IsUvIndexInNormalRange().ToYesNoString() }");
            Console.WriteLine($"Can I fly my kite? { _weatherProcessor.IsWindSpeedInNormalRange().ToYesNoString() }");


            Console.ReadLine();
            Console.WriteLine("Finish.....");

        }
    }
}
