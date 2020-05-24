using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Testing
{
    public static class FileManager
    {
        /// <summary>
        ///     Calculates the file names and locations of the region files associated with the selected chunks and returns them
        /// </summary>
        /// <param name="rx1">X of Chunk 1</param>
        /// <param name="rz1">Z of Chunk 1</param>
        /// <param name="rx2">X of Chunk 2</param>
        /// <param name="rz2">Z pf Chunk 2</param>
        /// <returns>Directory and Filename of region 1 and 2</returns>
        public static (string filename1, string filename2) DirFind(int rx1, int rz1, int rx2, int rz2)
        {
            var filename1 = $"{AppDomain.CurrentDomain.BaseDirectory}r.{rx1}.{rz1}.mca";
            var filename2 = $"{AppDomain.CurrentDomain.BaseDirectory}r.{rx2}.{rz2}.mca";
            return (filename1, filename2);
        }


        public static async Task<List<ChunkPair>> getChunkSwapList(string file)
        {
            using var fileStream = File.OpenText(file);

            var worldLine = await fileStream.ReadLineAsync();

            if (worldLine == null)
                throw new FormatException(
                    $"Missing world('the_word') statement at beginning of file file {file}. The world parameter must be a folder directory that is accesible");

            if (!File.Exists(worldLine))
                throw new FileNotFoundException($"Could not find file {Path.GetFullPath(worldLine)}");

            var chunkPairs = new List<ChunkPair>();

            while (!fileStream.EndOfStream)
            {
                var pair = ParseLine(await fileStream.ReadLineAsync());
                chunkPairs.Add(pair);
            }

            return chunkPairs;
        }

        /// <summary>
        ///     This is a method used for parsing the string x1:z1@x2:z2
        /// </summary>
        /// <returns></returns>
        public static ChunkPair ParseLine(string line)
        {
            var lineSeparated = line.Split("@", 2);

            var lineXZ1 = lineSeparated[0].Split(":");
            var lineXZ2 = lineSeparated[1].Split(":");

            var rx1 = int.Parse(lineXZ1[0]);
            var rz1 = int.Parse(lineXZ1[1]);

            var rx2 = int.Parse(lineXZ2[0]);
            var rz2 = int.Parse(lineXZ2[1]);


            return Calc.ChunkCalcFromChunkCoords(rx1, rz1, rx2, rz2);
        }
    }
}