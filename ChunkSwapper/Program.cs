using System;

namespace Testing
{
    internal class Program
    {
        //TODO Add support for giving a text file as input and reading all chunks from there
        //TODO Ability to go back and do another chunk without having the program close on you
        //TODO Make this code clean
        //TODO Gui? (Probably for a fork)
        private static void Main(string[] args)
        {
            Console.WriteLine("Systems are go");
            var (off1, off2, rx1, rz1, rx2, rz2) = Calc.ChunkCalc();
            Console.WriteLine(off1 + " " + off2);
            string rf1;
            string rf2;
            (rf1, rf2) = FileManager.Dirfind(rx1, rz1, rx2, rz2);
            var bs = new ByteSwapper();
            bs.Readbytes(rf1, rf2, off1, off2);
            bs.Swapbytes(rf1, rf2, off1, off2);
            Console.WriteLine("Operation Completed");
            Console.ReadLine();
        }
    }
}