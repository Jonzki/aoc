using Aoc.Utils;

namespace Aoc.Tests.Utils;

[TestClass]
public class BitArrayUtilsTests
{
    [DataTestMethod]
    [DataRow(0, "0000")]
    [DataRow(0b0001, "1")]
    [DataRow(0b0001, "0001")]
    [DataRow(0b1010, "1010")]
    [DataRow(0b0110, "0110")]
    [DataRow(0b0110, "110")]
    [DataRow(0b1111, "1111")]
    [DataRow(0b1000_0001, "10000001")]
    public void BitArrayToInt_Converts_Correctly(int correctOutput, string input)
    {
        var bitArray = BitArrayUtils.Parse(input);
        Assert.AreEqual(correctOutput, BitArrayUtils.ToInteger(bitArray));
    }

    [DataTestMethod]
    [DataRow("110100101111111000101000", "D2FE28")]
    public void HexToBitArray_Converts_Correctly(string correctOutput, string input)
    {
        // Parse the correct output.
        var correctArray = BitArrayUtils.Parse(correctOutput);

        var hexOutput = BitArrayUtils.HexToBitArray(input);

        CollectionAssert.AreEqual(correctArray, hexOutput);
    }

    [TestMethod]
    public void GetSegment_Returns_Segment()
    {
        var input = BitArrayUtils.Parse("000111000");

        var correctOutput = BitArrayUtils.Parse("000");
        var segment = input.GetSegment(0, 3);
        CollectionAssert.AreEqual(correctOutput, segment);

        correctOutput = BitArrayUtils.Parse("111");
        segment = input.GetSegment(3, 3);
        CollectionAssert.AreEqual(correctOutput, segment);
    }
}