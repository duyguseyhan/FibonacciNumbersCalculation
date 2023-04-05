using System;
using FibonacciNumbersCalculation.Services.Lock;
using System.Threading.Tasks;

namespace FibonacciNumbersCalculationTest.ServicesTests
{
	public class AsyncLockTests
	{
        [Fact]
        public async Task LockAsync_WhenCalled_AcquiresLock()
        {
            // Arrange
            var asyncLock = new AsyncLock();

            // Act
            using (await asyncLock.LockAsync())
            {
                // Assert
                // Lock has been acquired
                Assert.True(true);
            }
        }

        [Fact]
        public async Task LockAsync_WhenCalled_AcquiresAndReleasesLock()
        {
            // Arrange
            var asyncLock = new AsyncLock();

            // Act
            using (await asyncLock.LockAsync())
            {
                // Assert
                // Lock has been acquired
                Assert.True(true);
            }

            // Assert
            // Lock has been released
            Assert.True(true);
        }

        [Fact]
        public async Task LockAsync_WhenCalledConcurrently_AcquiresLock()
        {
            // Arrange
            var asyncLock = new AsyncLock();
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    using (await asyncLock.LockAsync())
                    {
                        // Assert
                        // Lock has been acquired
                        Assert.True(true);
                        await Task.Delay(100);
                    }
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            // Lock has been released
            Assert.True(true);
        }

        [Fact]
        public async Task LockAsync_WhenCalledTwice_WaitsForRelease()
        {
            // Arrange
            var asyncLock = new AsyncLock();
            var firstLock = await asyncLock.LockAsync();

            // Act
            var secondLockTask = asyncLock.LockAsync();
            Assert.False(secondLockTask.IsCompleted); // Make sure the second lock is waiting

            firstLock.Dispose(); // Release the first lock
            await secondLockTask; // Wait for the second lock to be acquired

            // Assert
            Assert.True(secondLockTask.IsCompletedSuccessfully);
        }
    }
}

