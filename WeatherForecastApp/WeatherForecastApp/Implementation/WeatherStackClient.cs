using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherForecastApp.Common;
using WeatherForecastApp.Contract;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Implementation
{
    public class WeatherStackClient : IWeatherStackClient
    {
        private readonly WeatherApiConfig _apiConfig;
        private readonly HttpClient _client;
        private readonly ILogger<IWeatherStackClient> _logger;

        public WeatherStackClient(HttpClient client, IOptions<WeatherApiConfig> options, ILogger<WeatherStackClient> logger)
        {
            _client = client;
            _apiConfig = options.Value;
            _logger = logger;
            _client.BaseAddress = new Uri(_apiConfig.Url);
        }

        public async Task<WeatherStackResult> GetCurrentWeather(string zipCode)
        {
            WeatherStackResult result;
            try
            {
                var response = await _client.GetStreamAsync($"current?access_key={_apiConfig.Key}&query={zipCode}");
                
                var currentWeather = await JsonSerializer.DeserializeAsync<WeatherStackResponse>(response);

                result = new WeatherStackResult(currentWeather);

                

            }
            catch (Exception ex)
            {
                _logger.LogError($"{AppConstant.WeatherApiException}:{ex.Message}");

                result = new WeatherStackResult(false, $"{AppConstant.WeatherApiException}");
               
            }

            return result;
        }
    }
}
