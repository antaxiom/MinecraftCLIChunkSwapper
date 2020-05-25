using System;
using System.IO;

namespace Testing
{
    public class ByteSwapper
    {
        private const int BLength = 4;
        private byte[] _chunkBytes1 = {0}; //Bytes of Chunk 1
        private byte[] _chunkBytes2 = {0}; //Bytes of Chunk2

        /// <summary>
        ///     Using file 1 and 2 and their byte offsets reads 4 bytes from there and writes it to Cb1 and 2 accordingly
        /// </summary>
        /// <param name="file1">File Directory 1</param>
        /// <param name="file2">File Directory 2</param>
        /// <param name="off1">Byte offset for file1</param>
        /// <param name="off2">Byte offset for file2</param>
        public void Readbytes(string file1, string file2, int off1, int off2)
        {
            using var fileStream1 = new FileStream(file1, FileMode.Open);
            using var fileStream2 = new FileStream(file2, FileMode.Open);
            using var binaryReader1 = new BinaryReader(fileStream1);
            using var binaryReader2 = new BinaryReader(fileStream2);
            fileStream1.Seek(off1, 0);
            var chunkByte1 = binaryReader1.ReadBytes(BLength);
            fileStream2.Seek(off2, 0);
            var chunkByte2 = binaryReader2.ReadBytes(BLength);
            _chunkBytes1 = chunkByte1;
            _chunkBytes2 = chunkByte2;
            binaryReader1.Close();
            binaryReader2.Close();
            fileStream1.Close();
            fileStream2.Close();
        }

        /// <summary>
        ///     Takes in file 1 and 2 and uses their byte offsets to write their (4) bytes to each other (Cb1 and Cb2)
        /// </summary>
        /// <param name="file1">File Directory 1</param>
        /// <param name="file2">File Directory 2</param>
        /// <param name="off1">Byte offset for file1</param>
        /// <param name="off2">Byte offset for file2</param>
        public void SwapBytes(string file1, string file2, int off1, int off2)
        {
            try
            {
                RegionDiscriminator(file1, file2);
            }
            catch
            {
                Console.WriteLine("Backup already exists, overwriting");
            }
            finally
            {
                File.Delete(file1 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                            DateTime.Now.Millisecond + "-" + ".backup");
                File.Delete(file2 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                            DateTime.Now.Millisecond + "-" + ".backup");
            }

            using var fileStream1 = new FileStream(file1, FileMode.Open);
            using var fileStream2 = new FileStream(file2, FileMode.Open);
            using var binaryWriter1 = new BinaryWriter(fileStream1);
            using var binaryWriter2 = new BinaryWriter(fileStream2);
            fileStream1.Seek(off1, 0);
            binaryWriter1.Write(_chunkBytes2);
            fileStream1.Flush();
            fileStream2.Seek(off2, 0);
            binaryWriter2.Write(_chunkBytes1);
            fileStream2.Flush();
            fileStream1.Close();
            fileStream2.Close();
            binaryWriter1.Close();
            binaryWriter2.Close();
        }

        /// <summary>
        ///     Discriminates to see if both region files are the same and writes one or both of them depending on the case
        /// </summary>
        /// <param name="file1">File Location 1</param>
        /// <param name="file2">File Location 2</param>
        private static void RegionDiscriminator(string file1, string file2)
        {
            if (file1 == file2)
            {
                File.Copy(file1,
                    file1 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                    DateTime.Now.Millisecond + "-" + ".backup");
            }
            else
            {
                File.Copy(file2,
                    file2 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                    DateTime.Now.Millisecond + ".backup");
                File.Copy(file1,
                    file1 + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" +
                    DateTime.Now.Millisecond + ".backup");
            }
        }
    }
}