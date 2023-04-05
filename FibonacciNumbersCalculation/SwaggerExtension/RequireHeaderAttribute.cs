using System;
namespace FibonacciNumbersCalculation.SwaggerExtension
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RequireHeaderAttribute : Attribute
	{
        public string HeaderName { get; }

        public RequireHeaderAttribute(string headerName)
        {
            HeaderName = headerName;
        }
    }
}

