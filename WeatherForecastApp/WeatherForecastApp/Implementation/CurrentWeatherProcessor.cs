using System.Linq;
using WeatherForecastApp.Common;
using WeatherForecastApp.Contract;
using WeatherForecastApp.Extensions;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Implementation
{
    public class CurrentWeatherProcessor : ICurrentWeatherProcessor
    {
        private readonly Current _currentWeather;
        private readonly IWeatherStackClient _client;

        
        /// <summary>
        /// Constructor new something
        /// </summary>
        /// <param name="current"></param>
        public CurrentWeatherProcessor(Current current)
        {     
             _currentWeather = current;
        }


        public bool IsUvIndexInNormalRange()
        {
            var response = false;

            if (_currentWeather.UvIndex > 3)
                response = true;

            return response;
        }

        public bool IsWeatherSuitableOutside()
        {
            var response = true;

            if (IsWeatherCodeRaining())
                response = false;
               

            return response;

        }

        private bool IsWeatherCodeRaining()
        {
            bool result = false;

            if (_currentWeather.WeatherCode.IsIn(WeatherCodes.HeavyRain,
                                             WeatherCodes.HeavyRainAtTimes,
                                             WeatherCodes.LightFreezingRain,
                                             WeatherCodes.LightRain,
                                             WeatherCodes.ModerateRain,
                                             WeatherCodes.ModerateRainAtTimes,
                                             WeatherCodes.PatchyLightRain,
                                             WeatherCodes.PatchyRainPossible) && _currentWeather.WeatherDescription.Any(value => value.Contains("rain")))
            {


                result = true;
            }

            return result;
        }

        public bool IsWindSpeedInNormalRange()
        {
            var response = false;

            if (_currentWeather.WindSpeed > 15 && !IsWeatherCodeRaining())
                response = true;

            return response;
        }

        
    }
}
