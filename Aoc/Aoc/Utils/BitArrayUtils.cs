using System.Collections;
using System.Text;

namespace Aoc.Utils;

public static class BitArrayUtils
{
    /// <summary>
    /// Parses a string of bits (eg. "1010") into a BitArray.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static BitArray Parse(string input)
    {
        var output = new BitArray(input.Length);
        for (int i = input.Length - 1; i >= 0; i--)
        {
            output[i] = input[i] switch
            {
                '1' => true,
                '0' => false,
                _ => throw new ArgumentException("Input string must contain only ones and zeroes.")
            };
        }
        return output;
    }

    /// <summary>
    /// Converts the input BitArray to integer. The input BitArray must be at most 32 bits.
    /// </summary>
    /// <param name="bitArray"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static int ToInteger(this BitArray bitArray)
    {
        if (bitArray.Count > 32)
        {
            throw new ArgumentException("Input BitArray must be 32 bits or under.");
        }

        // Reverse the BitArray - BitArray.Copy works this way around.
        var reversed = new BitArray(bitArray.Count);
        for (int i = 0; i < bitArray.Count; i++)
        {
            reversed[reversed.Length - 1 - i] = bitArray[i];
        }

        // https://stackoverflow.com/a/5283199
        var array = new int[1];
        reversed.CopyTo(array, 0);
        return array[0];
    }

    /// <summary>
    /// Converts the input BitArray to a long integer. The input BitArray must be at most 64 bits.
    /// </summary>
    /// <param name="bitArray"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static long ToLong(this BitArray bitArray)
    {
        if (bitArray.Count > 64)
        {
            throw new ArgumentException("Input BitArray must be 64 bits or under.");
        }

        // Reverse the BitArray - BitArray.Copy works this way around.
        var reversed = new BitArray(bitArray.Count);
        for (int i = 0; i < bitArray.Count; i++)
        {
            reversed[reversed.Length - 1 - i] = bitArray[i];
        }

        var len = Math.Min(64, reversed.Count);
        long n = 0;
        for (int i = 0; i < len; i++)
        {
            if (reversed.Get(i))
                n |= 1L << i;
        }
        return n;
    }

    /// <summary>
    /// Parses a hexadecimal string (0-9, A-F) into a BitArray.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static BitArray HexToBitArray(string input)
    {
        // Set up the output array.
        var bitArray = new BitArray(input.Length * 4);

        for (var i = 0; i < input.Length; i++)
        {
            // Convert the character to an integer of 0-15.
            int c = (input[i] - '0');
            if (c > 9)
            {
                c -= 7;
            }

            // Assign the four bits.
            bitArray[i * 4 + 0] = (c & 0b1000) > 0;
            bitArray[i * 4 + 1] = (c & 0b0100) > 0;
            bitArray[i * 4 + 2] = (c & 0b0010) > 0;
            bitArray[i * 4 + 3] = (c & 0b0001) > 0;
        }

        return bitArray;
    }

    /// <summary>
    /// Returns a segment of the input BitArray.
    /// </summary>
    /// <param name="bitArray"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static BitArray GetSegment(this BitArray bitArray, int index, int length)
    {
        var limit = Math.Min(index + length, bitArray.Length) - index;

        var output = new BitArray(length);
        for (var i = 0; i < limit; i++)
        {
            output[i] = bitArray[index + i];
        }
        return output;
    }

    /// <summary>
    /// Returns a bit representation of the BitArray.
    /// </summary>
    /// <param name="bitArray"></param>
    /// <returns></returns>
    public static string ToBitString(this BitArray bitArray)
    {
        var sb = new StringBuilder(bitArray.Length);
        for (var i = 0; i < bitArray.Length; i++)
        {
            sb.Append(bitArray[i] ? '1' : '0');
        }
        return sb.ToString();
    }

}
