using System;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    partial class Program
    {
        private static readonly SemaphoreSlim _CheezburgerLock = new SemaphoreSlim(1, 1);
        private static async Task CanHazCheezeBurgerAsync()
        {
            await _CheezburgerLock.WaitAsync();
            try
            {
                await GetCheezeBurgerAsync();
            }
            finally
            {
                _CheezburgerLock.Release();
            }
        }

        private static void AsyncLockExample()
        {
            CanHazCheezeBurgerAsync().Wait();
            //UseAsyncLock().Wait();
        }

        private static Task GetCheezeBurgerAsync()
        {
            return Task.Delay(1000);
        }

        #region Async Lock
        private static readonly AsyncLock _AsyncLock = new AsyncLock();

        private static async Task UseAsyncLock()
        {
            using (_AsyncLock.LockAsync())
            {
                await GetCheezeBurgerAsync();
            }
        }

        private sealed class AsyncLock : IDisposable
        {
            private readonly SemaphoreSlim _Semaphore = new SemaphoreSlim(1, 1);

            public async Task<IDisposable> LockAsync()
            {
                await _Semaphore.WaitAsync();
                return this;
            }

            public void Dispose()
            {
                _Semaphore.Release();
            }
        }
        #endregion
    }
}