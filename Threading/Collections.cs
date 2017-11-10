using System;
using System.Collections.Concurrent;
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

        public static void BlockingCollection()
        {
            const int NumValues = 10;
            BlockingCollection<int> numbersToProcess = new BlockingCollection<int>();
            Random random = new Random();
            int numAdded = 0;

            var produceTasks = Enumerable.Range(1, NumValues)
                        .Select(ProduceValues);

            var consumingTask = Task.Run(async () =>
            {
                foreach (int number in numbersToProcess.GetConsumingEnumerable())
                {
                    await Task.Delay(random.Next(50, 250));
                    Console.WriteLine($"Processed {number}");
                }
            });

            Task.WhenAll(produceTasks).Wait();
            consumingTask.Wait();

            Console.WriteLine("Done");

            async Task ProduceValues(int value)
            {
                await Task.Delay(random.Next(100, 1000));
                Console.WriteLine($"Added {value}");
                numbersToProcess.Add(value);
                if (++numAdded == NumValues) numbersToProcess.CompleteAdding();
            }
        }

        #endregion
    }
}