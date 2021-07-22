using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using System.IO;
using WeatherForecastApp.Contract;
using WeatherForecastApp.Implementation;
using WeatherForecastApp.Models;

namespace WeatherForecastApp
{
    public static class StartUp 
    {
 
        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            //build configuration and custom ones
            var configuration = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: false)
                            .AddEnvironmentVariables()
                            .Build();

            services.Configure<WeatherApiConfig>(configuration.GetSection("WeatherApiConfig"));

            //add http client 
            services.AddHttpClient<IWeatherStackClient, WeatherStackClient>();

            //add logging
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)
                            .CreateLogger();

            services.AddLogging(builder =>
            {
                builder.AddConfiguration(configuration.GetSection("Logging"));
                builder.AddConsole();
                builder.AddSerilog();
            });


            // add application entry
            services.AddTransient<AppEntry>();

           
            

            return services;
        }
    }
}
