using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FibonacciNumbersCalculation.Models;

namespace FibonacciNumbersCalculation.Services.ExceptionFilters
{
    public class FibonacciExceptionFilterAttribute : ExceptionFilterAttribute
    {

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var exception = context.Exception;
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();

            var errorResponse = new ErrorModel
            {
                Controller = controllerName,
                Action = actionName,
                ErrorMessage = exception.Message
            };

            if (exception is ArgumentException)
            {
                context.Result = new BadRequestObjectResult(errorResponse);
            }

            else
            {
                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }

            await base.OnExceptionAsync(context);
        }
    }
}

