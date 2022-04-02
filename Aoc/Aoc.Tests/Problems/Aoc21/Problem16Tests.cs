﻿using Aoc.Problems.Aoc21;
using Aoc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aoc.Tests.Problems.Aoc21;

[TestClass]
public class Problem16Tests : ProblemTests<Problem16>
{
    protected override string SmallInput => "D2FE28";

    protected override object CorrectOutput1 => 40;

    protected override object CorrectOutput2 => 315;

    [DataTestMethod]
    [DataRow(6, "D2FE28")]
    [DataRow(16, "8A004A801A8002F478")]
    [DataRow(12, "620080001611562C8802118E34")]
    [DataRow(23, "C0015000016115A2E0802F182340")]
    [DataRow(31, "A0016C880162017C3686B18A3D4780")]
    public void Part1_SmallInput_Is_Correct(int correctOutput, string input) => RunPart1(correctOutput, input);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);

    [TestMethod]
    public void PacketParse_Handles_LiteralValue()
    {
        var input = "D2FE28";

        var bitArray = BitArrayUtils.HexToBitArray(input);

        var packet = Problem16.Packet.Parse(bitArray);

        Assert.IsNotNull(packet);
        Assert.AreEqual(6, packet.Version);
        Assert.AreEqual(4, packet.PacketType);

        Assert.AreEqual(2021, packet.LiteralValue);
    }

    [TestMethod]
    public void PacketParse_Handles_OperatorPacket1()
    {
        var input = "38006F45291200";
        var bitArray = BitArrayUtils.HexToBitArray(input);

        var packet = Problem16.Packet.Parse(bitArray);

        Assert.IsNotNull(packet);
        Assert.AreEqual(1, packet.Version);
        Assert.AreEqual(6, packet.PacketType);

        Assert.AreEqual(2, packet.Subpackets.Count);

        Assert.AreEqual(4, packet.Subpackets[0].PacketType);
        Assert.AreEqual(10, packet.Subpackets[0].LiteralValue);

        Assert.AreEqual(4, packet.Subpackets[1].PacketType);
        Assert.AreEqual(20, packet.Subpackets[1].LiteralValue);
    }

    [TestMethod]
    public void PacketParse_Handles_OperatorPacket2()
    {
        var input = "EE00D40C823060";
        var bitArray = BitArrayUtils.HexToBitArray(input);

        var packet = Problem16.Packet.Parse(bitArray);

        Assert.IsNotNull(packet);
        Assert.AreEqual(7, packet.Version);
        Assert.AreEqual(3, packet.PacketType);

        Assert.AreEqual(3, packet.Subpackets.Count);

        // Numbers in the subpackets are 1-3 - check with a loop.
        for (var i = 0; i < 3; ++i)
        {
            Assert.AreEqual(4, packet.Subpackets[i].PacketType);
            Assert.AreEqual(i + 1, packet.Subpackets[i].LiteralValue);
        }
    }


}