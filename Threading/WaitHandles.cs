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
            for (int i = 0; i < 10; i++)
            {
                new Thread(ThreadMethod).Start();
            }

            Console.WriteLine("Waiting...");

            for (int i = 0; i < 10; i++)
            {
                _AutoResetEvent.Set();
            }

            Thread.Sleep(1000);

            Console.WriteLine("Done");

            void ThreadMethod()
            {
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} waiting");
                _AutoResetEvent.WaitOne();
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} done");
            }
        }
    }
}