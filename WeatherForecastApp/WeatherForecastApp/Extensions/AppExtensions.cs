using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeatherForecastApp.Common;

namespace WeatherForecastApp.Extensions
{
    public static class AppExtensions
    {
        public static bool IsIn<WeatherCodes>(this WeatherCodes @this, params WeatherCodes[] possibles)
        {
            return possibles.Contains(@this);
        }

        public static string ToYesNoString(this bool value)
        {
            return value ? AppConstant.RespondYes : AppConstant.RespondNo;
        }
    }
}
