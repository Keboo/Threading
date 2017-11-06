using System;
using System.Threading;

namespace Threading
{
    partial class Program
    {
        private static int _ShouldStop;

        public static void ReleaseBuild()
        {
            var thread = new Thread(Count);
            thread.Start();
            Thread.Sleep(1000);
            _ShouldStop = 1;
            Console.WriteLine("Waiting for thread to stop");
            thread.Join();
            Console.WriteLine("Done");
        }

        private static void Count()
        {
            int x = 0;
            while (_ShouldStop == 0) x++;
            Console.WriteLine($"Stopping: x is {x}");
        }
    }
}