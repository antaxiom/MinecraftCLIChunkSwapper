using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Testing
{
    internal static class Program
    {
        //TODO Add support for giving a text file as input and reading all chunks from there
        //TODO Ability to go back and do another chunk without having the program close on you
        //TODO Make this code clean
        //TODO Gui? (Probably for a fork)
        private static async Task Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            AsyncLogger.WriteLine("Systems are go");

            var chunkPairs = new List<ChunkPair>();

            if (args.Length > 0)
            {
                // Read a text file
                // For now only support for the first line

                var file = args[0];

                if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"Unable to find the file {file}");
                }

                chunkPairs = await FileManager.getChunkSwapList(args[0]);

            }
            else
            {
                var chunkPair = initializeInputVariables();
                chunkPairs.Add(chunkPair);
            }

            foreach (var chunkPair in chunkPairs)
            {
                var calcData1 = chunkPair.chunk1;
                var calcData2 = chunkPair.chunk2;

                AsyncLogger.WriteLine($"{calcData1.byteOff} {calcData2.byteOff}");
                string rf1;
                string rf2;
                (rf1, rf2) = FileManager.DirFind(calcData1.regionX, calcData1.regionZ, calcData2.regionX, calcData2.regionZ);
                var bs = new ByteSwapper();
                bs.Readbytes(rf1, rf2, calcData1.byteOff, calcData2.byteOff);
                bs.SwapBytes(rf1, rf2, calcData1.byteOff, calcData2.byteOff);

            }

            AsyncLogger.WriteLine($"Operation Completed. Took {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Stop();
            Console.ReadLine();
            AsyncLogger.StopThread();
        }
        internal static ChunkPair initializeInputVariables() {
            AsyncLogger.WriteLine("All Good");
            AsyncLogger.WriteLine("What is chunk X1?");
            var inX1 = Convert.ToInt32(Console.ReadLine());
            AsyncLogger.WriteLine("What is chunk Z1?");
            var inZ1 = Convert.ToInt32(Console.ReadLine());
            AsyncLogger.WriteLine("What is chunk X2?");
            var inX2 = Convert.ToInt32(Console.ReadLine());
            AsyncLogger.WriteLine("What is chunk Z2?");
            var inZ2 = Convert.ToInt32(Console.ReadLine());


            return Calc.ChunkCalcFromBlockCoords(inX1, inZ1, inX2, inZ2);
        }

    }


}