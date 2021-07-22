using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WeatherForecastApp.Common;

namespace WeatherForecastApp.Models
{
    public class Current
    {
        [JsonPropertyName("temperature")]
        public int Temperature { get; set; }
        [JsonPropertyName("weather_code")]
        public WeatherCodes WeatherCode { get; set; }
        [JsonPropertyName("wind_speed")]
        public int WindSpeed { get; set; }
        [JsonPropertyName("uv_index")]
        public int UvIndex { get; set; }

        [JsonPropertyName("weather_descriptions")]
        public List<string> WeatherDescription { get; set; }
    }
}
