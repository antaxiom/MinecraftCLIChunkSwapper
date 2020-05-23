using System;

namespace Testing
{
    public class Calc
    {
        /// <summary>
        ///     Gets the byte offsets of the chunks the user specified
        /// </summary>
        /// <returns>byteoff1, byteoff2</returns>
        public static (int byteoff1, int byteoff2, int regionx1, int regionz1, int regionx2, int regionz2) ChunkCalc()
        {
            Console.WriteLine("All Good");
            Console.WriteLine("What is chunk X1?");
            var inx1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What is chunk Z1?");
            var inz1 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What is chunk X2?");
            var inx2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("What is chunk Z2?");
            var inz2 = Convert.ToInt32(Console.ReadLine());
            var regionx1 = (int) Math.Floor(inx1 / 32.0);
            var regionz1 = (int) Math.Floor(inz1 / 32.0);
            var regionx2 = (int) Math.Floor(inx2 / 32.0);
            var regionz2 = (int) Math.Floor(inz2 / 32.0);
            var byteoff1 = 4 * (inx1 % 32 + inz1 % 32 * 32);
            var byteoff2 = 4 * (inx2 % 32 + inz2 % 32 * 32);
            Console.WriteLine("Inputs are " + inx1 + "," + inz1 + " and " + inx2 + "," + inz2);
            return (byteoff1, byteoff2, regionx1, regionz1, regionx2, regionz2);
        }
    }
}