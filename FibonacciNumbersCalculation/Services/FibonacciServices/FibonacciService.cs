using System;
using System.Collections.Concurrent;
using FibonacciNumbersCalculation.Services.ExceptionFilters;

namespace FibonacciNumbersCalculation.Services.FibonacciServices
{
    public class FibonacciService : IFibonacciService
    {
        private ConcurrentDictionary<int, long> _dictionaryFibonacci;

        public FibonacciService()
        {
            _dictionaryFibonacci = new ConcurrentDictionary<int, long>();
        }

        public async Task<long> GetFibonacciNumberAsync(int index)
        {
            if (index < 0)
            {
                throw new ArgumentException("Index must be positive.", nameof(index));
            }

            if (_dictionaryFibonacci.TryGetValue(index, out long value))
            {
                return value;
            }

            long result;

            if (index <= 1)
            {
                result = index;
            }
            else
            {
                var computationTasks = new[]
                {
                GetFibonacciNumberAsync(index - 1),
                GetFibonacciNumberAsync(index - 2)
            };

                await Task.WhenAll(computationTasks).ConfigureAwait(false);

                result = computationTasks[0].Result + computationTasks[1].Result;
            }

            _dictionaryFibonacci[index] = result;

            return result;
        }
    }
}

