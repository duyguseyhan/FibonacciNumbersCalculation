using System;
using Microsoft.AspNetCore.Mvc;

namespace FibonacciNumbersCalculation.Models
{
	public class ErrorModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string ErrorMessage { get; set; } 
	}
}

