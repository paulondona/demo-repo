using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecastApp.Common;
using WeatherForecastApp.Implementation;
using WeatherForecastApp.Models;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace WeatherForecastAppTest.Implementation
{
    public class WeatherStackClientTest
    {
        private readonly IOptions<WeatherApiConfig> _weatherApiConfigOptions;
        private readonly ILogger<WeatherStackClient> _logger;

        public WeatherStackClientTest()
        {
            var mockApiConfig = new Mock<IOptions<WeatherApiConfig>>();
            mockApiConfig.Setup(ap => ap.Value).Returns(new WeatherApiConfig { Key = "someKey", Url = "http://some.weatherstack-test.com" });

            var mockLogger = new Mock<ILogger<WeatherStackClient>>();

            _weatherApiConfigOptions = mockApiConfig.Object;
            _logger = mockLogger.Object;
        }

        
        [Fact]
        public async void GetCurrentWeather_Should_Return_WeatherStackResultResponse()
        {
            //Arrange     
            var outputResponse = new WeatherStackResponse
            {
                CurrentWeather = new Current
                {
                    Temperature = 2,
                    UvIndex = 3,
                    WindSpeed = 1,
                    WeatherCode = WeatherCodes.Sunny
                }
            };

            var expected = new WeatherStackResponse
            {
                CurrentWeather = new Current
                {
                    Temperature = 2,
                    UvIndex = 3,
                    WindSpeed = 1,
                    WeatherCode = WeatherCodes.Sunny
                }
            };

            var httpMessageHandler = ConfigureHttpMessageHandler(HttpStatusCode.OK, outputResponse);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var target = GetWeatherStackClient(httpClient);

            //Act
            var actual = await target.GetCurrentWeather("10001");

            //Assert
            actual.Response.Should().BeOfType<WeatherStackResponse>().Which.CurrentWeather.Equals(expected.CurrentWeather);
            
        }

        [Fact]
        public async void GetCurrentWeather_Should_Return_ErrorUsageLimitMessageReached_WhenUsageLimitReached()
        {
            //Arrange     
            var currentWeather = new WeatherStackResponse
            {
                Success = false,
                ErrorInfo = new Error
                {
                    Code = 104,
                    Type = "usage_limit_reached",
                    Info = "Your monthly API request volume has been reached. Please upgrade your plan."
                },
                CurrentWeather = null
            };

            var expected = new Error
            {
                Code = 104,
                Type = "usage_limit_reached",
                Info = "Your monthly API request volume has been reached. Please upgrade your plan."
            };

            var httpMessageHandler = ConfigureHttpMessageHandler(HttpStatusCode.OK, currentWeather);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var target = GetWeatherStackClient(httpClient);

            //Act
            var actual = await target.GetCurrentWeather("10001");

            //Assert
            actual.ErrorMessage.Should().BeEquivalentTo(expected.Info);          
        }

        [Fact]
        public async void GetCurrentWeather_Should_Return_IsValidFalse_WhenUsageLimitReached()
        {
            //Arrange     
            var currentWeather = new WeatherStackResponse
            {
                Success = false,
                ErrorInfo = new Error
                {
                    Code = 104,
                    Type = "usage_limit_reached",
                    Info = "Your monthly API request volume has been reached. Please upgrade your plan."
                },
                CurrentWeather = null
            };

       

            var httpMessageHandler = ConfigureHttpMessageHandler(HttpStatusCode.OK, currentWeather);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var target = GetWeatherStackClient(httpClient);

            //Act
            var actual = await target.GetCurrentWeather("10001");

            //Assert
            actual.IsValid.Should().BeFalse();
        }

        [Fact]
        public async void GetCurrentWeather_Should_Return_IsValidTrue_WhenApiReturnOkReuestAndNoError()
        {
            //Arrange     

            var outputResponse = new WeatherStackResponse
            {
                CurrentWeather = new Current
                {
                    Temperature = 2,
                    UvIndex = 3,
                    WindSpeed = 1,
                    WeatherCode = WeatherCodes.Sunny
                }
            };

            var httpMessageHandler = ConfigureHttpMessageHandler(HttpStatusCode.OK, outputResponse);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var target = GetWeatherStackClient(httpClient);

            //Act
            var actual = await target.GetCurrentWeather("10001");

            //Assert
            actual.IsValid.Should().BeTrue();


        }

        [Fact]
        public async void GetCurrentWeather_Should_Return_IsValidFalse_WhenApiCallReturnBadRequest()
        {
            //Arrange     
            var httpMessageHandler = ConfigureHttpMessageHandler(HttpStatusCode.BadRequest, null);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var target = GetWeatherStackClient(httpClient);

            //Act
            var actual = await target.GetCurrentWeather("10001");

            //Assert
            actual.IsValid.Should().BeFalse();
           
           
        }

        [Fact]
        public async void GetCurrentWeather_Should_Return_WebApiExceptionMessage_WhenApiCallReturnBadRequest()
        {
            //Arrange     
            var expectedMessage = AppConstant.WeatherApiException;

            var httpMessageHandler = ConfigureHttpMessageHandler(HttpStatusCode.BadRequest, null);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            var target = GetWeatherStackClient(httpClient);

            //Act
            var actual = await target.GetCurrentWeather("10001");

            //Assert
            actual.ErrorMessage.Should().BeEquivalentTo(expectedMessage);


        }

        

        private Mock<HttpMessageHandler> ConfigureHttpMessageHandler(HttpStatusCode code, object responseData )
        {
            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.IsAny<HttpRequestMessage>(),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync((HttpRequestMessage request, CancellationToken token) =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = code;//Setting statuscode                                                                           
                        response.Content = new StringContent(JsonSerializer.Serialize(responseData)); // configure your response here    
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); //Setting media type for the response    
                        return response;
                    });

            return httpMessageHandler;
        }

        private WeatherStackClient GetWeatherStackClient(HttpClient httpClient)
        {
            return new WeatherStackClient(httpClient, _weatherApiConfigOptions, _logger);
        }

    }
}
