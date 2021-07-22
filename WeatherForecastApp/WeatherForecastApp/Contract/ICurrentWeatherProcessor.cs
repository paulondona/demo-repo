using System;
using System.Collections.Generic;
using System.Text;

namespace WeatherForecastApp.Contract
{
    public interface ICurrentWeatherProcessor
    {
        bool IsWeatherSuitableOutside();
        bool IsUvIndexInNormalRange();
        bool IsWindSpeedInNormalRange();


    }
}
