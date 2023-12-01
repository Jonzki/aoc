using System.Collections;

namespace Aoc.Problems.Aoc21;

public class Problem16 : IProblem
{
    public object Solve1(string input)
    {
        var bitArray = BitArrayUtils.HexToBitArray(input);

        var packet = Packet.Parse(bitArray);

        return packet.VersionSum();
    }

    public object Solve2(string input)
    {
        var bitArray = BitArrayUtils.HexToBitArray(input);

        var packet = Packet.Parse(bitArray);

        return packet.CalculateValue();
    }

    public enum PacketType
    {
        LiteralValue = 4
    };

    public class Packet
    {
        public int Version { get; set; } = -1;

        public int PacketType { get; set; } = -1;

        public int LengthType { get; set; } = -1;

        public List<Packet> Subpackets { get; set; } = new();

        // Type-specific output.
        public long LiteralValue { get; set; } = -1;

        /// <summary>
        /// Parses a Packet from the input BitArray.
        /// </summary>
        /// <param name="bitArray"></param>
        /// <returns></returns>
        public static Packet Parse(BitArray bitArray) => Parse(bitArray, new Cursor());

        /// <summary>
        /// Parses a Packet from the input BitArray.
        /// </summary>
        /// <param name="bitArray"></param>
        /// <param name="cursor">Cursor instance, holding the current index.</param>
        /// <returns></returns>
        internal static Packet Parse(BitArray bitArray, Cursor cursor)
        {
            // Start by reading the packet header.
            var packet = new Packet();
            packet.Version = cursor.ConsumeSegment(bitArray, 3).ToInteger();
            packet.PacketType = cursor.ConsumeSegment(bitArray, 3).ToInteger();

            // Handle different packet types.
            if (packet.PacketType == 4)
            {
                // Read segments of 5 bits until we reach the end, or encounter a zero-padded one.
                var numberBits = new List<bool>();

                while (true)
                {
                    var segment = cursor.ConsumeSegment(bitArray, 5);

                    // Make sure the segment is 5 items long.
                    if (segment.Length != 5)
                    {
                        throw new InvalidOperationException($"Invalid segment of length {segment.Length} at position {cursor.Index}.");
                    }

                    // Add in the number bits.
                    for (var j = 1; j < segment.Length; j++) numberBits.Add(segment[j]);

                    // Stop reading if the leading bit was a zero.
                    if (segment[0] == false)
                    {
                        break;
                    }
                }

                // We now have a list of bits. Convert to integer.
                packet.LiteralValue = new BitArray(numberBits.ToArray()).ToLong();
            }
            else
            {
                // Enter into Operator mode. Pick the length type ID.
                packet.LengthType = cursor.ConsumeSegment(bitArray, 1)[0] ? 1 : 0;

                // If the length type ID is 0, then the next 15 bits are a number
                // that represents the total length in bits of the sub-packets contained by this packet.
                if (packet.LengthType == 0)
                {
                    var subPacketBits = cursor.ConsumeSegment(bitArray, 15).ToInteger();
                    var endPos = cursor.Index + subPacketBits;
                    while (cursor.Index < endPos)
                    {
                        packet.Subpackets.Add(Parse(bitArray, cursor));
                    }
                }
                else if (packet.LengthType == 1)
                {
                    var subPacketCount = cursor.ConsumeSegment(bitArray, 11).ToInteger();
                    for (var i = 0; i < subPacketCount; i++)
                    {
                        packet.Subpackets.Add(Parse(bitArray, cursor));
                    }
                }
            }

            return packet;
        }

        /// <summary>
        /// Recursively calculates the sum of Versions of the packet hierarchy.
        /// </summary>
        /// <returns></returns>
        public int VersionSum() => Version + Subpackets.Sum(p => p.VersionSum());

        /// <summary>
        /// Calculates the (part 2) value for the Packet.
        /// </summary>
        /// <returns></returns>
        public long CalculateValue()
        {
            return PacketType switch
            {
                // type ID 0 are sum packets
                0 => Subpackets.Sum(p => p.CalculateValue()),
                // type ID 1 are "product" packets
                1 => CalculateProduct(),
                // type ID 2 are "minimum" packets
                2 => Subpackets.Min(p => p.CalculateValue()),
                // type ID 3 are "maximum" packets
                3 => Subpackets.Max(p => p.CalculateValue()),
                // type ID 4 is the literal value.
                4 => LiteralValue,
                // type ID 5 are "greater than" packets
                5 => Subpackets[0].CalculateValue() > Subpackets[1].CalculateValue() ? 1L : 0L,
                // type ID 6 are "less than" packets
                6 => Subpackets[0].CalculateValue() < Subpackets[1].CalculateValue() ? 1L : 0L,
                // type ID 7 are "equal to" packets
                7 => Subpackets[0].CalculateValue() == Subpackets[1].CalculateValue() ? 1L : 0L,
                _ => throw new InvalidOperationException($"Unrecognized Packet Type {PacketType}.")
            };
        }

        private long CalculateProduct()
        {
            var result = 1L;
            foreach (var packet in Subpackets)
            {
                result *= packet.CalculateValue();
            }
            return result;
        }
    }

    public class Cursor
    {
        public Cursor()
        {
            Index = 0;
        }

        public int Index { get; set; } = 0;

        /// <summary>
        /// Returns a segment of given length starting at the current cursor position, then increments the cursor by the returned length.
        /// </summary>
        /// <param name="inputArray"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public BitArray ConsumeSegment(BitArray inputArray, int length)
        {
            var output = inputArray.GetSegment(Index, length);
            Index += output.Length;
            return output;
        }

    }
}
