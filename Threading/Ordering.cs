using System;
using System.Threading;

namespace Threading
{
    partial class Program
    {
        public static int _FinishedResult;
        public static bool _IsFinished;

        public static void SetInOrder()
        {
            _IsFinished = false;
            new Thread(FinishOnThread).Start();
            while (true)
            {
                if (_IsFinished)
                {
                    Console.WriteLine("_FinishedResult = {0}", _FinishedResult);
                    return;
                }
            }
        }

        private static void FinishOnThread()
        {
            _FinishedResult = 42;
            _IsFinished = true;
        }
    }
}