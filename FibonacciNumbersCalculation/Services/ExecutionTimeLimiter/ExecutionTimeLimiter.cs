using System;

namespace FibonacciNumbersCalculation.Services.ExecutionTimeLimiter
{
    public class ExecutionTimeLimiter : IExecutionTimeLimiter
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        
        public ExecutionTimeLimiter()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public bool IsTimeLimitExceeded()
        {            
            return _cancellationTokenSource.IsCancellationRequested;
        }

        public void StartTimer(int executionTime)
        {
            _cancellationTokenSource.CancelAfter(executionTime);
        }
    }
}

