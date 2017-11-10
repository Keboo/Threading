using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        #region BlockingCollection

        private static readonly BlockingCollection<int> _NumbersToProcess = new BlockingCollection<int>();
        private static readonly Random _Random = new Random();
        private static int _NumAdded;
        
        public static void BlockingCollection()
        {
            var produceTasks = Enumerable.Range(1, 10)
                .Select(ProduceValues);

            var consumingTask = Task.Run(async () =>
            {
                foreach (int number in _NumbersToProcess.GetConsumingEnumerable())
                {
                    await Task.Delay(_Random.Next(50, 250));
                    Console.WriteLine($"Processed {number}");
                }
            });

            Task.WhenAll(produceTasks).Wait();
            consumingTask.Wait();

            Console.WriteLine("Done");
        }

        private static async Task ProduceValues(int value)
        {
            await Task.Delay(_Random.Next(100, 1000));
            Console.WriteLine($"Added {value}");
            _NumbersToProcess.Add(value);
            if (++_NumAdded == 10) _NumbersToProcess.CompleteAdding();
        }

        #endregion
    }
}