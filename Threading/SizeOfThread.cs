using System;
using System.Diagnostics;
using System.Threading;

namespace Threading
{
    partial class Program
    {
        public static void SizeOfThread()
        {
            const int MB = 1024 * 1024;
            do
            {
                Thread t = new Thread(RunForever);
                t.Start();
                Console.WriteLine($"Current memory size {Process.GetCurrentProcess().VirtualMemorySize64 / MB} MB");
            } while (Console.ReadLine() == "");
        }

        private static void RunForever()
        {
            while(true) { }
        }
    }
}