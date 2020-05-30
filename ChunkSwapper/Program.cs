using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
    internal static class Program
    {

        // Keeps a list of file locks.
        private static readonly Dictionary<string, object> FileLock = new Dictionary<string, object>();

        //TODO Add support for giving a text file as input and reading all chunks from there
        private static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            AsyncLogger.WriteLine("Systems are go");

            var chunkPairs = new List<ChunkPair>();
            string fileWorld;

            if (args.Length > 0)
            {
                // Read a text file
                // For now only support for the first line
                stopwatch.Start();
                var file = args[0];

                if (!File.Exists(file)) throw new FileNotFoundException($"Unable to find the file {file}");

                (fileWorld, chunkPairs) = await FileManager.GetChunkSwapList(args[0]);

            }
            else
            {
                fileWorld = ".";
                chunkPairs.Add(InitializeInputVariables());
                stopwatch.Start();
            }

            var tasks = new List<Task>();

            foreach (var chunkPair in chunkPairs)
            {
                var calcData1 = chunkPair.chunk1;
                var calcData2 = chunkPair.chunk2;

                AsyncLogger.WriteLine($"{calcData1.byteOff} {calcData2.byteOff}");
                string rf1;
                string rf2;
                (rf1, rf2) = FileManager.DirFind(fileWorld, calcData1.regionX, calcData1.regionZ, calcData2.regionX,
                    calcData2.regionZ);

                tasks.Add(Task.Run(async () =>
                {
                    var lock1 = GetLock(rf1); // Will be by default the already created lock or a new one.
                    var lock2 = GetLock(rf2); // Will be by default the already created lock or a new one.

                    if (lock1 == null)
                    {
                        if (lock2 != null) lock1 = lock2;
                        else lock1 = new object();


                        lock1 = AddLock(rf1, lock1);

                    }

                    if (lock2 == null)
                    {
                        lock2 = lock1;

                        lock2 = AddLock(rf2, lock2);
                    }

                    // Lock the file to avoid writing to the same file at the same time
                    Task t;
                    lock (lock1)
                    {
                        lock (lock2)
                        {
                            t = ByteSwapper.ReadAndSwapBytes(rf1, rf2, calcData1.byteOff, calcData2.byteOff);
                        }
                    }

                    // Wait for task to finish.
                    await t;
                }));
            }

            // Wait for tasks to finish
            while (tasks.Count > 0)
            {
                var taskListCopy = new List<Task>(tasks);

                foreach (var task in taskListCopy)
                {
                    await task;
                    tasks.Remove(task);
                }
            }



            AsyncLogger.WriteLine($"Operation Completed. Took {stopwatch.ElapsedMilliseconds}ms");
            //TODO Ability to go back and do another chunk without having the program close on you
            stopwatch.Stop();
            Console.ReadLine();
            AsyncLogger.StopThread();

            Environment.Exit(0);
        }


        private static object AddLock(string rf2, object lock2)
        {
            lock (FileLock)
            {
                if (FileLock.ContainsKey(rf2)) return FileLock[rf2];

                FileLock[rf2] = lock2;

                return lock2;
            }
        }

        private static ChunkPair InitializeInputVariables()
        {
            AsyncLogger.WriteLine("All Good");
            AsyncLogger.WriteLine("What is chunk X1?");
            var inX1 = Convert.ToInt32(Console.ReadLine());
            AsyncLogger.WriteLine("What is chunk Z1?");
            var inZ1 = Convert.ToInt32(Console.ReadLine());
            AsyncLogger.WriteLine("What is chunk X2?");
            var inX2 = Convert.ToInt32(Console.ReadLine());
            AsyncLogger.WriteLine("What is chunk Z2?");
            var inZ2 = Convert.ToInt32(Console.ReadLine());


            return Calc.ChunkCalcFromChunkCoords(inX1, inZ1, inX2, inZ2);
        }

        // Gets the locks for the string or null if none found.
        private static object GetLock(string str)
        {
            try
            {
                return FileLock[str];
            }
            catch
            {
                return null;
            }
        }
    }
}