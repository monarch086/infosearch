using System.Collections;

namespace InfoSearch.Core.Spimi
{
    internal static class BitExtensions
    {
        public static byte[] ToBytes(this BitArray bits)
        {
            var bytesCount = bits.Length / 8;
            if (bits.Length % 8 > 0) bytesCount++;

            var bytes = new byte[bytesCount];
            bits.CopyTo(bytes, 0);

            return bytes;
        }

        public static BitArray InsertBit(this BitArray bits, int position)
        {
            var prev = false;

            for (int i = position; i < bits.Length; i++)
            {
                if (i == position)
                {
                    prev = bits[i];

                    bits[i] = true;
                }
                else
                {
                    var temp = bits[i];
                    bits[i] = prev;
                    prev = temp;
                }
            }

            return bits;
        }

        public static BitArray RemoveVariableBits(this BitArray bits)
        {
            var shiftCount = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if (i.IsLastBitInByte())
                {
                    shiftCount++;
                }

                bits[i] = i + shiftCount < bits.Length ? bits[i + shiftCount] : false;
            }

            return bits;
        }

        public static int ToInt(this BitArray bits)
        {
            if (bits.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bits.CopyTo(array, 0);

            return array[0];
        }

        public static BitArray Reverse(this BitArray bits)
            => new BitArray(bits.Cast<bool>().Reverse().ToArray());

        public static int GetLastSetBit(this BitArray bits)
        {
            for (int i = bits.Length - 1; i >= 0; i--)
            {
                if (bits[i]) return i;
            }

            return -1;
        }

        public static bool IsLastBitInByte(this int bitIndex)
            => (bitIndex + 1) % 8 == 0;

        public static int CountUsedBytes(this BitArray bits)
        {
            var lastSetBit = bits.GetLastSetBit();
            var bytes = (lastSetBit + 1) / 8 + ((lastSetBit + 1) % 8 > 0 ? 1 : 0);
            return bytes;
        }

        public static BitArray Trim(this BitArray bits)
        {
            var byteSize = bits.CountUsedBytes();
            var trimmedArray = new BitArray(byteSize * 8);
            var lastSetBit = bits.GetLastSetBit();

            for (int i = 0; i <= lastSetBit; i++)
            {
                trimmedArray[i] = bits[i];
            }

            return trimmedArray;
        }
    }
}
