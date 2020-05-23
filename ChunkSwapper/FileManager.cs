using System;

namespace Testing
{
    public class FileManager
    {
        /// <summary>
        ///     Calculates the file names and locations of the region files associated with the selected chunks and returns them
        /// </summary>
        /// <param name="rx1">X of Chunk 1</param>
        /// <param name="rz1">Z of Chunk 1</param>
        /// <param name="rx2">X of Chunk 2</param>
        /// <param name="rz2">Z pf Chunk 2</param>
        /// <returns>Directory and Filename of region 1 and 2</returns>
        public static (string filename1, string filename2) Dirfind(int rx1, int rz1, int rx2, int rz2)
        {
            var filename1 = AppDomain.CurrentDomain.BaseDirectory + "/r." + rx1 + "." + rz1 + ".mca";
            var filename2 = AppDomain.CurrentDomain.BaseDirectory + "/r." + rx2 + "." + rz2 + ".mca";
            return (filename1, filename2);
        }
    }
}