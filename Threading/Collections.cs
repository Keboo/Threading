using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Threading
{
    partial class Program
    {
        public static void Collections()
        {
            var bag = new ConcurrentBag<int>();
            var dictionary = new ConcurrentDictionary<int, string>();
            var queue = new ConcurrentQueue<int>();
            var stack = new ConcurrentStack<int>();
            
        }
    }
}