using System;
using System.Collections;

namespace Aoc.Utils;

public static class BitArrayUtils
{
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

        int output = 0;
        for (var i = 0; i < 32; ++i)
        {
            if (i >= bitArray.Length) break;

            if (bitArray[bitArray.Length - 1 - i])
            {
                output += (1 << i);
            }
        }
        return output;
    }
}
