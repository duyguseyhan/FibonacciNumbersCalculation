using System;
using System.ComponentModel.DataAnnotations;

namespace FibonacciNumbersCalculation.Models
{
	public class FibonacciSubsequenceRequestModel
	{
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "StartIndex must be non-negative.")]
        public int StartIndex { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "EndIndex must be non-negative.")]
        public int EndIndex { get; set; }

        [Required(ErrorMessage = "UseCache is required.")]
        public bool UseCache { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "TimeOut must be non-negative.")]
        public int TimeOut { get; set; }

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "MaxMemoryUsage must be non-negative.")]
        public long MaxMemoryUsage { get; set; }	
    }
}

