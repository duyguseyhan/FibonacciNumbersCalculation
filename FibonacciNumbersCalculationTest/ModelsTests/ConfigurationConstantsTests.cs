using System;
using Microsoft.Extensions.Configuration;

namespace FibonacciNumbersCalculationTest.ModelsTests
{
	public class ConfigurationConstantsTests
	{
        [Fact]
        public void CacheExpirationTimeInMinutes_ShouldReturnConfiguredValue()
        {
            // Arrange
            var expectedValue = 10; // Assuming the appsettings.json file has a "CacheExpirationTimeInMinutes" key with value 10
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["CacheExpirationTimeInMinutes"] = expectedValue.ToString()
                })
                .Build();

            // Act
            var actualValue = configuration.GetValue<int>("CacheExpirationTimeInMinutes");

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }


        [Fact]
        public void CacheExpirationTimeInMinutes_ShouldThrowException_WhenConfigValueIsInvalid()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["CacheExpirationTimeInMinutes"] = "not a number"
                })
                .Build();

            // Act and assert
            Assert.Throws<System.InvalidOperationException>(() => configuration.GetValue<int>("CacheExpirationTimeInMinutes"));
        }
    }
}

