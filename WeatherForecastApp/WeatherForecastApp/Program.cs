using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace WeatherForecastApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create service collection
            var services = StartUp.ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            
            //entry to run app
            
            Task mainTask = serviceProvider.GetService<AppEntry>().Run(args);

            mainTask.Wait();
        
        
        
        }
    }
}
