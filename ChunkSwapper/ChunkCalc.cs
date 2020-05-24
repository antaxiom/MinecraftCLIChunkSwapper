using System;

namespace Testing
{
    public class Calc
    {
        /// <summary>
        ///     Gets the byte offsets of the chunks the user specified
        /// </summary>
        /// <returns>Byteoffset of chunk 1 and 2, XY of Region 1 and 2</returns>
        public static ChunkPair ChunkCalcFromChunkCoords(int inX1, int inZ1, int inX2, int inZ2)
        {
            var regionX1 = (int) Math.Floor(inX1 / 32.0);
            var regionZ1 = (int) Math.Floor(inZ1 / 32.0);
            var regionX2 = (int) Math.Floor(inX2 / 32.0);
            var regionZ2 = (int) Math.Floor(inZ2 / 32.0);
            var byteOff1 = Math.Abs(4 * (inX1 % 32 + inZ1 % 32 * 32));
            var byteOff2 = Math.Abs(4 * (inX2 % 32 + inZ2 % 32 * 32));

            Console.WriteLine($"Region inputs are {inX1}, {inZ1} and {inX2}, {inZ2}");

            return new ChunkPair(
                new ChunkCoords(byteOff1, regionX1, regionZ1),
                new ChunkCoords(byteOff2, regionX2,
                    regionZ2) // Named parameters here are unnecessary but a good showcase
            );
        }
    }

    public class ChunkPair
    {
        public readonly ChunkCoords chunk1;
        public readonly ChunkCoords chunk2;

        public ChunkPair(ChunkCoords chunk1, ChunkCoords chunk2)
        {
            this.chunk1 = chunk1;
            this.chunk2 = chunk2;
        }
    }


    public class ChunkCoords
    {
        public readonly int byteOff;
        public readonly int regionX;
        public readonly int regionZ;

        public ChunkCoords(int byteOff, int regionX, int regionZ)
        {
            this.byteOff = byteOff;
            this.regionX = regionX;
            this.regionZ = regionZ;
        }
    }
}