using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FibonacciNumbersCalculation.SwaggerExtension
{
	public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                var requiredHeaders = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                    .OfType<RequireHeaderAttribute>()
                    .Select(attr => attr.HeaderName);

                foreach (var header in requiredHeaders)
                {
                    if (!operation.Parameters.Any(p => p.Name == header))
                    {
                        operation.Parameters.Add(new OpenApiParameter
                        {
                            Name = header,
                            In = ParameterLocation.Header,
                            Required = true
                        });
                    }
                }
            }
        }
    }
}

