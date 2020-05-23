using System;

namespace Testing
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var (off1, off2, rx1, rz1, rx2, rz2) = Calc.ChunkCalc();
            Console.WriteLine(off1 + " " + off2);
            string rf1;
            string rf2;
            (rf1, rf2) = FileManager.Dirfind(rx1, rz1, rx2, rz2);
            var bs = new ByteSwapper();
            bs.Readbytes(rf1, rf2, off1, off2);
            bs.Swapbytes(rf1, rf2, off1, off2);
        }
    }
}