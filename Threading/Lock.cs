using System;
using System.Threading;

namespace Threading
{
    partial class Program
    {
        private static readonly object _LockObject = new object();

        public static void LockA()
        {
            lock (_LockObject)
            {
                
            }
        }

        public static void LockB()
        {
            object lockObject = _LockObject;
            bool lockTaken = false;
            try
            {
                Monitor.Enter(lockObject, ref lockTaken);
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(_LockObject);
                }
            }
        }
    }
}