using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WeatherForecastApp.Models
{
    public class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("info")]
        public string Info { get; set; }
    }
}
