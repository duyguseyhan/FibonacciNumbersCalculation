using System;
namespace FibonacciNumbersCalculation.Services.ExecutionTimeLimiter
{
	public interface IExecutionTimeLimiter
	{
        public bool IsTimeLimitExceeded();
        public void StartTimer(int executionTime);
    }
}

