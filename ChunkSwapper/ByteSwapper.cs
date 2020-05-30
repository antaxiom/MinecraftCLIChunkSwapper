using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Testing
{
    public static class ByteSwapper
    {
        private const int BLength = 4;

        /// <summary>
        ///     Using file 1 and 2 and their byte offsets reads 4 bytes from there and writes it to Cb1 and 2 accordingly
        /// </summary>
        /// <param name="file1">File Directory 1</param>
        /// <param name="file2">File Directory 2</param>
        /// <param name="off1">Byte offset for file1</param>
        /// <param name="off2">Byte offset for file2</param>
        public static async Task<(byte[] _chunkBytes1, byte[] _chunkBytes2)> ReadBytes(string file1, string file2, int off1, int off2)
        {
            Thread.Sleep(10); // Wait to finish closing file

            await using var fileStream1 = File.OpenRead(file1);
            await using var fileStream2 = File.OpenRead(file2);

            using var binaryReader1 = new BinaryReader(fileStream1);
            using var binaryReader2 = new BinaryReader(fileStream2);
            fileStream1.Seek(off1, 0);
            fileStream2.Seek(off2, 0);

            var chunkByte1 = binaryReader1.ReadBytes(BLength);
            var chunkByte2 = binaryReader2.ReadBytes(BLength);


            binaryReader1.Close();
            binaryReader2.Close();

            fileStream1.Close();
            fileStream2.Close();

            return (chunkByte1, chunkByte2);
        }

        /// <summary>
        ///     Takes in file 1 and 2 and uses their byte offsets to write their (4) bytes to each other (Cb1 and Cb2)
        /// </summary>
        /// <param name="file1">File Directory 1</param>
        /// <param name="file2">File Directory 2</param>
        /// <param name="off1">Byte offset for file1</param>
        /// <param name="off2">Byte offset for file2</param>
        /// <param name="chunkBytes1"></param>
        /// <param name="chunkBytes2"></param>
        public static async Task SwapBytes(string file1, string file2, int off1, int off2, byte[] chunkBytes1,
            byte[] chunkBytes2)
        {
            RegionDiscriminator(file1, file2);

            AsyncLogger.WriteLine($"Swapping chunks bytes at {file1}:{off1} with {file2}:{off2}");


            if (file1 != file2)
            {

                await using var fileStream1 = File.OpenWrite(file1);
                await using var fileStream2 = File.OpenWrite(file2);
                await using var binaryWriter1 = new BinaryWriter(fileStream1);
                await using var binaryWriter2 = new BinaryWriter(fileStream2);

                fileStream1.Seek(off1, 0);
                fileStream2.Seek(off2, 0);

                binaryWriter1.Write(chunkBytes2);
                binaryWriter2.Write(chunkBytes1);
                await fileStream1.FlushAsync();
                await fileStream2.FlushAsync();
            }
            else
            {
                await using var fileStream1 = File.OpenWrite(file1);
                await using var binaryWriter1 = new BinaryWriter(fileStream1);

                fileStream1.Seek(off1, 0);

                binaryWriter1.Write(chunkBytes2);
                await fileStream1.FlushAsync();
            }
        }

        /// <summary>
        ///     Discriminates to see if both region files are the same and writes one or both of them depending on the case
        /// </summary>
        /// <param name="file1">File Location 1</param>
        /// <param name="file2">File Location 2</param>
        private static void RegionDiscriminator(string file1, string file2)
        {
            var timeBackupFormat = $"{DateTime.Now.Day}-${DateTime.Now.Hour}-{DateTime.Now.Minute}" +
                                   $"-{DateTime.Now.Second}.backup";

            AsyncLogger.WriteLine($"Creating backup for {file1} {file2}");
            if (file1 == file2)
            {
                File.Copy(file1,
                    $"{file1}-{timeBackupFormat}", true);
            }
            else
            {
                File.Copy(file2,
                    $"{file2}-{timeBackupFormat}", true);
                File.Copy(file1,
                    $"{file1}-{timeBackupFormat}", true);
            }
            AsyncLogger.WriteLine($"Finished backup for {file1} {file2}");
        }

        public static async Task ReadAndSwapBytes(string rf1, string rf2, int calcData1ByteOff, int calcData2ByteOff)
        {
            var (byteSwaps1, byteSwaps2) = await ReadBytes(rf1, rf2, calcData1ByteOff, calcData2ByteOff);
            await SwapBytes(rf1, rf2, calcData1ByteOff, calcData2ByteOff, byteSwaps1, byteSwaps2);
        }
    }
}