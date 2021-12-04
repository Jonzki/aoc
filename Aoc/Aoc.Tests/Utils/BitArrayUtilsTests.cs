using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Utils;

[TestClass]
public class BitArrayUtilsTests
{
    [DataTestMethod]
    [DataRow(0, "0000")]
    [DataRow(0b1010, "1010")]
    [DataRow(0b1111, "1111")]
    [DataRow(0b1000_0001, "10000001")]
    public void BitArrayToInt_Converts_Correctly(int correctOutput, string input)
    {
        var bitArray = BitArrayUtils.Parse(input);
        Assert.AreEqual(correctOutput, BitArrayUtils.ToInteger(bitArray));
    }
}
