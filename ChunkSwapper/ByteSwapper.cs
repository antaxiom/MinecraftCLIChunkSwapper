using System;
using System.IO;

namespace Testing
{
    public class ByteSwapper
    {
        private readonly int _blenght = 4;
        public byte[] Cb1 = {0};
        public byte[] Cb2 = {0};

        public void Readbytes(string f1, string f2, int off1, int off2)
        {
            var fs1 = new FileStream(f1, FileMode.Open);
            var fs2 = new FileStream(f2, FileMode.Open);
            var br1 = new BinaryReader(fs1);
            var br2 = new BinaryReader(fs2);
            fs1.Seek(off1, 0);
            var chunkbyte1 = br1.ReadBytes(_blenght);
            fs2.Seek(off2, 0);
            var chunkbyte2 = br2.ReadBytes(_blenght);
            Cb1 = chunkbyte1;
            Cb2 = chunkbyte2;
            br1.Close();
            br2.Close();
            fs1.Close();
            fs2.Close();
        }

        public void Swapbytes(string f1, string f2, int off1, int off2)
        {
            try
            {
                Regiondiscriminator(f1, f2);
            }
            catch
            {
                Console.WriteLine("Backup already exists, overwriting");
            }
            finally
            {
                File.Delete(f1 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                            DateTime.Now.Millisecond + "-" + ".backup");
                File.Delete(f2 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                            DateTime.Now.Millisecond + "-" + ".backup");
            }

            var fs1 = new FileStream(f1, FileMode.Open);
            var fs2 = new FileStream(f2, FileMode.Open);
            var bw1 = new BinaryWriter(fs1);
            var bw2 = new BinaryWriter(fs2);
            fs1.Seek(off1, 0);
            bw1.Write(Cb2);
            fs1.Flush();
            fs2.Seek(off2, 0);
            bw2.Write(Cb1);
            fs2.Flush();
            fs1.Close();
            fs2.Close();
            bw1.Close();
            bw2.Close();
        }

        private void Regiondiscriminator(string f1, string f2)
        {
            if (f1 == f2)
            {
                File.Copy(f1,
                    f1 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                    DateTime.Now.Millisecond + "-" + ".backup");
            }
            else
            {
                File.Copy(f2,
                    f2 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                    DateTime.Now.Millisecond + ".backup");
                File.Copy(f1,
                    f1 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                    DateTime.Now.Millisecond + ".backup");
            }
        }
    }
}