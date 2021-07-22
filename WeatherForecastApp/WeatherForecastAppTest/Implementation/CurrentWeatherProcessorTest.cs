using System.Collections.Generic;
using WeatherForecastApp.Common;
using WeatherForecastApp.Implementation;
using WeatherForecastApp.Models;
using Xunit;

namespace WeatherForecastAppTest.Implementation
{
    public class CurrentWeatherProcessorTest
    {
      
        [Fact]
        public void IsUvIndexInNormalRange_Should_Return_True_WhenUvIndexAboveThree()
        {
            //Arrange
            var currentWeather = new Current
            {
                WindSpeed = 15,
                UvIndex = 4,
                WeatherCode = WeatherCodes.HeavyRain,
                WeatherDescription = new List<string>() { "Heavy rain" },
            };

            var expected = true;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsUvIndexInNormalRange();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsUvIndexInNormalRange_Should_Return_False_WhenUvIndexBelowThree()
        {
            //Arrange
            var currentWeather = new Current
            {
                WindSpeed = 15,
                UvIndex = 1,
                WeatherCode = WeatherCodes.HeavyRain,
                WeatherDescription = new List<string>() { "Heavy rain" },
            };

            var expected = false;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsUvIndexInNormalRange();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWeatherSuitableOutside_Should_Return_False_WhenAnyTypeOfRain()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.HeavyRainAtTimes,
                WeatherDescription = new List<string>() { "Heavy rain at times" },
            };

            var expected = false;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWeatherSuitableOutside();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWeatherSuitableOutside_Should_Return_True_WhenNotAnyTypeOfRain()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.Cloudy,
                WeatherDescription = new List<string>() { "Cloudy" },
            };

            var expected = true;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWeatherSuitableOutside();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWeatherSuitableOutside_Should_Return_True_WhenNotAnyTypeOfRain_And_NotContainMultipleRainyWeatherDescription()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.PartlyCloudy,
                WeatherDescription = new List<string>() { "Cloudy", "PartlyCloudy","Overcast" },
            };

            var expected = true;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWeatherSuitableOutside();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWeatherSuitableOutside_Should_Return_False_WhenAnyTypeOfRain_And_ContainMultipleRainyWeatherDescription()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.HeavyRain,
                WeatherDescription = new List<string>() { "Heavy freezing drizzle", "Light rain", "Moderate rain at times" },
            };

            var expected = false;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWeatherSuitableOutside();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWindSpeedInNormalRange_Should_Return_True_WhenWindSpeedOver15_And_NotAnyTypeOfRain()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.PartlyCloudy,
                WeatherDescription = new List<string>() { "Cloudy", "PartlyCloudy", "Overcast" },
                WindSpeed = 20
            };

            var expected = true;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWeatherSuitableOutside();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWindSpeedInNormalRange_Should_Return_False_WhenWindSpeedOver15_And_AnyTypeOfRain()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.HeavyRain,
                WeatherDescription = new List<string>() { "Heavy freezing drizzle", "Light rain", "Moderate rain at times" },
                WindSpeed = 20
            };

            var expected = false;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWindSpeedInNormalRange();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWindSpeedInNormalRange_Should_Return_False_WhenWindSpeedBelow15_And_NotAnyTypeOfRain()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.PartlyCloudy,
                WeatherDescription = new List<string>() { "Cloudy", "PartlyCloudy", "Overcast" },
                WindSpeed = 10
            };

            var expected = false;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWindSpeedInNormalRange();

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsWindSpeedInNormalRange_Should_Return_False_WhenWindSpeedEquals15_And_NotAnyTypeOfRain()
        {
            //Arrange
            var currentWeather = new Current
            {
                WeatherCode = WeatherCodes.PartlyCloudy,
                WeatherDescription = new List<string>() { "Cloudy", "PartlyCloudy", "Overcast" },
                WindSpeed = 15
            };

            var expected = false;
            var target = new CurrentWeatherProcessor(currentWeather);

            //Act
            var actual = target.IsWindSpeedInNormalRange();

            //Assert
            Assert.Equal(expected, actual);
        }
    }
}

