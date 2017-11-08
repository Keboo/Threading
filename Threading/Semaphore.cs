using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    partial class Program
    {
        private static readonly SemaphoreSlim _CheezburgerLock = new SemaphoreSlim(1, 1);
        private static async Task CanHazCheezburgerAsync()
        {
            await _CheezburgerLock.WaitAsync();
            try
            {
                await GetCheezBurgerAsync();
            }
            finally
            {
                _CheezburgerLock.Release();
            }

        }

        private static void AsyncLockExample()
        {
            CanHazCheezburgerAsync().Wait();
        }

        private static Task GetCheezBurgerAsync()
        {
            return Task.Delay(100);
        }

        #region Async Lock
        private static readonly AsyncLock _AsyncLock = new AsyncLock();

        private static async Task UseAsyncLock()
        {
            using (await _AsyncLock.LockAsync())
            {
                await LongRunningMethod();
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