using System.Collections.Generic;
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
                //Block to synchonize
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

        #region Alternate Locks

        private static readonly object _Kvp = new KeyValuePair<int, object>();
        private static readonly object _Number = 42;
        private static readonly object _Text = "Inigo Montoya";
        private static readonly object _Type = typeof(Program);

        public static void LockC()
        {
            lock (_Kvp) { }
            lock (_Number) { }
            lock (_Text) { }
            lock (_Type) { }
            //lock(this) { }
        }

        #endregion
    }
}