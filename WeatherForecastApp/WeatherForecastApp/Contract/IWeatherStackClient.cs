using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeatherForecastApp.Models;

namespace WeatherForecastApp.Contract
{
    public interface IWeatherStackClient
    {
        Task<WeatherStackResult> GetCurrentWeather(string zipCode);

    }
}
