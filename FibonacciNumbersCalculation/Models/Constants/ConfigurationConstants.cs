using System;
using FibonacciNumbersCalculation.Services.ExceptionFilters;

namespace FibonacciNumbersCalculation.Models.Constants
{
    [FibonacciExceptionFilterAttribute]
	public static class ConfigurationConstants
	{
        private static readonly IConfigurationRoot Configuration;

        static ConfigurationConstants()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public static int CacheExpirationTimeInMinutes => Configuration.GetValue<int>("CacheExpirationTimeInMinutes");
    }
}

