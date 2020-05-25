using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
    public static class AsyncLogger
    {
        private static readonly Queue<Tuple<string, object[]>> PrintQueue = new Queue<Tuple<string, object[]>>();

        private static bool running;

        public static void StartThread()
        {
            running = true;
            new Thread(LogAsync).Start();
        }

        public static void StopThread()
        {
            running = false;
        }

        // Debug variable. Switch this to see performance difference
        private const bool WriteLineAsync = true;

        public static void WriteLine(string s, params object[] args)
        {
            if (WriteLineAsync)
            {
                // TODO: Benchmark and ensure this is running asynchronous
                Task.Run(() => Console.WriteLine(s, args));
                // PrintQueue.Enqueue(new Tuple<string, object[]>(s, args));
            }
            else
            {
                Console.WriteLine(s, args);
            }
        }

        private static void LogAsync()
        {
            while (running)
            {
                var wait = true; // To avoid waiting next cycle

                while (PrintQueue.Count > 0)
                {
                    wait = false;

                    var (item1, item2) = PrintQueue.Dequeue();

                    Console.WriteLine(item1, item2);
                }

                if (wait) Thread.Sleep(10); // Wait for next cycle of logs
            }
        }
    }
}