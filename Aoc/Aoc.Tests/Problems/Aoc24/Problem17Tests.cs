using Aoc.Problems.Aoc24;
using System;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem17Tests : ProblemTests<Problem17>
{
    public const string SmallInput1 = """
                                     Register A: 729
                                     Register B: 0
                                     Register C: 0

                                     Program: 0,1,5,4,3,0
                                     """;

    public const string SmallInput2 = """
                                      Register A: 2024
                                      Register B: 0
                                      Register C: 0

                                      Program: 0,3,5,4,3,0
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        const string correctOutput = "4,6,3,5,6,3,5,2,1,0";

        RunPart1(correctOutput, SmallInput1);
    }

    [TestMethod]
    public void SolvePart2()
    {
        const int correctOutput = 117440;

        RunPart2(correctOutput, SmallInput2);
    }

    [TestMethod]
    public void ParseComputer_WorksWith_SmallInput()
    {
        // Parsing the small input should not throw.
        var computer = Problem17.ParseComputer(SmallInput1);

        // Registers should be as expected.
        computer.RegisterA.Should().Be(729);
        computer.RegisterB.Should().Be(0);
        computer.RegisterC.Should().Be(0);

        // Program should also be as expected.
        byte[] expectedProgram = [0, 1, 5, 4, 3, 0];

        computer.Program.Should().BeEquivalentTo(expectedProgram);
    }

    [TestMethod]
    public void ComputerAdv_Works()
    {
        // Combo operand 2 returns 2 -> denominator is 2^2 = 4.
        // Division ends up 8/4 = 2
        var computer = new Problem17.Computer([0, 2], 8, 0, 0);
        computer.RunSingle();
        computer.RegisterA.Should().Be(2);

        // Combo operand 6 returns value of RegisterC -> denominator is 2^3 = 8.
        // Division ends up 8/8 = 1
        computer = new Problem17.Computer([0, 6], 8, 2, 3);
        computer.RunSingle();
        computer.RegisterA.Should().Be(1);
    }

    [TestMethod]
    public void ComputerBxl_Works()
    {
        // opcode 1 = bxl
        // Use binary values here to better visualize the XOR operation.
        var computer = new Problem17.Computer([1, 0b010], 0, 0b011, 0);
        computer.RunSingle();
        computer.RegisterB.Should().Be(0b001);
    }

    [TestMethod]
    public void ComputerBst_Works()
    {
        // Combo operand 4 pulls the value from register A.
        var computer = new Problem17.Computer([2, 4], 9, 0, 0);
        computer.RunSingle();
        computer.RegisterB.Should().Be(1);

        // Combo operand 3 is used as is.
        computer = new Problem17.Computer([2, 3], 8, 99, 0);
        computer.RunSingle();
        computer.RegisterB.Should().Be(3);
    }

    [TestMethod]
    public void ComputerJnz_Works()
    {
        // JNZ will not jump if the A register is zero. In this case we should end up in a halted computer.
        var computer = new Problem17.Computer([3, 0], 0, 0, 0);
        computer.RunSingle();
        computer.IsHalted.Should().BeTrue();

        // If the register is not zero, the operator should take the literal operand and jump to that position - 2 in this case.
        computer = new Problem17.Computer([3, 2, 1, 0], 1, 0, 0);
        computer.RunSingle();
        computer.IsHalted.Should().BeFalse();
        computer.InstructionPointer.Should().Be(2);

        // This means that with a small program (less than 8 instructions) it's technically possible to set the instruction pointer to the last instruction,
        // breaking the computer on the next operation.
        computer = new Problem17.Computer([3, 3, 1, 0], 1, 0, 0);
        computer.RunSingle();
        computer.IsHalted.Should().BeFalse();
        computer.InstructionPointer.Should().Be(3);

        Assert.ThrowsException<InvalidOperationException>(() => computer.RunSingle());
    }

    [TestMethod]
    public void ComputerBxc_Works()
    {
        // bxc calculates XOR of registers B and C and saves this to register B.
        var computer = new Problem17.Computer([4, 0], 0, 0b10001, 0b01110);
        computer.RunSingle();
        computer.RegisterB.Should().Be(0b11111);
    }

    [TestMethod]
    public void ComputerOut_Works()
    {
        // The out instruction (opcode 5) calculates the value of its combo operand modulo 8,
        // then outputs that value.
        // bxc calculates XOR of registers B and C and saves this to register B.
        var computer = new Problem17.Computer([(byte)Problem17.OpCode.Out, 2], 0, 0, 0);
        computer.RunSingle();

        // The single combo operand value (modulo 8) should be outputted.
        computer.Output.Should().HaveCount(1).And.Contain(2);
    }

    [TestMethod]
    public void ComputerBdv_Works()
    {
        // bdv and cdv work exactly like the adv division operator,
        // expect that the output is written to registers B and C respectively.
        // Since we've tested adv separately, here we just need to verify that the correct registers are updated.

        var computer = new Problem17.Computer([(byte)Problem17.OpCode.Bdv, 2], 8, 0, 0);
        computer.RunSingle();

        // Register A should be untouched.
        computer.RegisterA.Should().Be(8);

        // Result (8 / 2^2) = 2 should be in register B.
        computer.RegisterB.Should().Be(2);

        // Register C should be untouched as well.
        computer.RegisterC.Should().Be(0);
    }

    [TestMethod]
    public void ComputerCdv_Works()
    {
        // bdv and cdv work exactly like the adv division operator,
        // expect that the output is written to registers B and C respectively.
        // Since we've tested adv separately, here we just need to verify that the correct registers are updated.

        var computer = new Problem17.Computer([(byte)Problem17.OpCode.Cdv, 2], 8, 0, 0);
        computer.RunSingle();

        // Register A should be untouched.
        computer.RegisterA.Should().Be(8);

        // Register B should be untouched as well.
        computer.RegisterB.Should().Be(0);

        // Result (8 / 2^2) = 2 should be in register C.
        computer.RegisterC.Should().Be(2);
    }
}