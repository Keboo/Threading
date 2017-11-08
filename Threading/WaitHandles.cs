using System;
using System.Threading;

namespace Threading
{
    partial class Program
    {
        private static readonly AutoResetEvent _AutoResetEvent = new AutoResetEvent(false);
        private static readonly ManualResetEvent _ManaualResetEvent = new ManualResetEvent(false);

        public static void WaitForMe()
        {
            int count =  0;
            for (int i = 0; i < 10; i++)
            {
                new Thread(ThreadMethod).Start();
            }

            _AutoResetEvent.Set();
            _AutoResetEvent.Set();

            for (int i = 0; i < 5; i++)
            {
                _AutoResetEvent.Set();
            }

            Thread.Sleep(1000);

            Console.WriteLine($"{count} threads made in through");

            void ThreadMethod()
            {
                _AutoResetEvent.WaitOne();
                Interlocked.Increment(ref count);
            }
        }

        private static WaitHandle GetHandle() => new ManualResetEvent(false);
    }
}