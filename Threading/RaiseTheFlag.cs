using System;
using System.Threading;

namespace Threading
{
    partial class Program
    {
        private static int _ShouldStop;

        public static void RaiseTheFlag()
        {
            var thread = new Thread(Count);
            thread.Start();

            //Let the thread count for a bit
            Thread.Sleep(500);
            _ShouldStop = 1;

            Console.WriteLine("Waiting for thread to stop");
            thread.Join();
            Console.WriteLine("Done");

            void Count()
            {
                int x = 0;
                while (_ShouldStop == 0) x++;
                Console.WriteLine($"Stopping: x is {x}");
            }
        }
    }
}