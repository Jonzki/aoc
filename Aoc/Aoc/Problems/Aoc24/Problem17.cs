namespace Aoc.Problems.Aoc24;

public class Problem17 : IProblem
{
    public object Solve1(string input)
    {
        var computer = ParseComputer(input);

        computer.Run();

        return string.Join(',', computer.GetOutput());
    }

    // TODO: Need a more proper algorithm to solve part 2.
    public object Solve2(string input)
    {
        // In part 2, we need to find a register 1 value that will produce the exact same output as the program itself.
        // Start by parsing a computer once for a template, and then an actual computer to run with.
        var template = ParseComputer(input);
        var computer = ParseComputer(input);

        const long limit = 500;

        Console.WriteLine("RegA;EndA;EndB;EndC");

        for (long registerA = 0; registerA < limit; ++registerA)
        {
            if (registerA % 100_000 == 0)
            {
                Console.WriteLine($"{100 * registerA / limit:F1} ...");
            }

            // Reset the computer with the input register.
            computer.Reset(registerA, template.RegisterB, template.RegisterC);

            // Run the computer until halting.
            computer.Run(limit: 100_000, stopIfOutputDoesNotMatch: true);

            // Compare the output
            if (CompareProgramToOutput(template.Program, computer.GetOutput()))
            {
                Console.WriteLine($"Initial RegA={registerA} produced the same output.");
                return registerA;
            }
        }

        Console.WriteLine($"Did not find a suitable RegA value with a limit of {limit}.");
        return -1;
    }

    /// <summary>
    /// Checks if the two input byte arrays are equivalent.
    /// </summary>
    /// <param name="program"></param>
    /// <param name="output"></param>
    /// <returns></returns>
    private bool CompareProgramToOutput(byte[] program, byte[] output)
    {
        if (program.Length != output.Length)
        {
            return false;
        }

        for (var i = 0; i < program.Length; ++i)
        {
            if (program[i] != output[i])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Parses a Computer from the input text.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static Computer ParseComputer(string input)
    {
        var lines = input.SplitLines();

        // Input is expected to contain 5 lines.
        if (lines.Length != 5)
        {
            throw new InvalidOperationException("Bad computer input, was expecting 5 lines.");
        }

        // First three are the registers.
        var registerValues = new int[3];
        for (var i = 0; i < 3; ++i)
        {
            char register = (char)('A' + i);

            var registerPrefix = $"Register {register}: ";
            if (!lines[i].StartsWith(registerPrefix))
            {
                throw new InvalidOperationException($"Could not read Register {register}: expected prefix not found.");
            }

            var registerText = lines[i].Replace(registerPrefix, "");
            if (int.TryParse(registerText, out var temp))
            {
                registerValues[i] = temp;
            }
            else
            {
                throw new InvalidOperationException($"Could not read Register {register}: could not parse integer value.");
            }
        }

        // Fourth line is just an empty one. Last row contains the program.
        const string programPrefix = "Program: ";
        if (!lines[4].StartsWith(programPrefix))
        {
            throw new InvalidOperationException("Could not read Program: expected prefix not found.");
        }

        var programValues = lines[4]
            .Substring(programPrefix.Length)
            .Split(',')
            .Select(x => byte.TryParse(x, out var temp) ? temp : throw new InvalidOperationException("Could not read Program: one or more values did not parse into a byte."))
            .ToArray();

        return new Computer(programValues, registerValues[0], registerValues[1], registerValues[2]);
    }

    public struct Computer(byte[] program, long registerA, long registerB, long registerC)
    {
        /// <summary>
        /// Contains the program (ordered set of instructions).
        /// </summary>
        public readonly byte[] Program = program;

        public int InstructionPointer { get; private set; } = 0;

        /// <summary>
        /// Contains the program output.
        /// </summary>
        public byte[] Output = new byte[program.Length + 1];

        private int _outputIndex = 0;

        public bool IsHalted => InstructionPointer >= Program.Length;

        // Registers A-C, can hold any integer.
        private long _registerA = registerA;

        public long RegisterA => _registerA;

        private long _registerB = registerB;
        public long RegisterB => _registerB;

        private long _registerC = registerC;
        public long RegisterC => _registerC;

        /// <summary>
        /// Runs the computer until it halts.
        /// Allows for an optional iteration limit.
        /// </summary>
        public void Run(int limit = 10_000, bool stopIfOutputDoesNotMatch = false)
        {
            var i = 0;
            for (; i < limit; ++i)
            {
                if (IsHalted)
                {
                    //Console.WriteLine($"Program is halted after {i} iterations.");
                    break;
                }
                var opCode = RunSingle();

                // Analyze the output if the opCode was "out"
                if (opCode == OpCode.Out && stopIfOutputDoesNotMatch)
                {
                    if (_outputIndex == Program.Length)
                    {
                        // Output length has exceeded the program length - this RegA cannot be our solution.
                        return;
                    }

                    // Output may be shorter at this point than our program.
                    for (var j = _outputIndex; j >= 0; --j)
                    {
                        if (Program[j] != Output[j])
                        {
                            // Outputs are mismatched - stop.
                            return;
                        }
                    }
                }
            }
            if (i == limit)
            {
                Console.WriteLine("Computer stopped due to iteration limit reached.");
            }
        }

        /// <summary>
        /// Runs a single operation on the computer.
        /// </summary>
        /// <returns>The OpCode that was processed.</returns>
        public OpCode RunSingle()
        {
            // Make sure we are not halted.
            if (IsHalted)
            {
                throw new InvalidOperationException("Program is halted.");
            }

            var opCode = (OpCode)Program[InstructionPointer];
            // 7 operations to process.
            switch (opCode)
            {
                case OpCode.Adv:
                    Adv();
                    break;

                case OpCode.Bxl:
                    Bxl();
                    break;

                case OpCode.Bst:
                    Bst();
                    break;

                case OpCode.Jnz:
                    Jnz();
                    break;

                case OpCode.Bxc:
                    Bxc();
                    break;

                case OpCode.Out:
                    Out();
                    break;

                case OpCode.Bdv:
                    Bdv();
                    break;

                case OpCode.Cdv:
                    Cdv();
                    break;

                default:
                    throw new NotImplementedException(
                        $"No operation implemented for instruction {Program[InstructionPointer]}.");
            }

            return opCode;
        }

        /// <summary>
        /// Returns the program output as a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetOutput()
        {
            return Output[.._outputIndex];
        }

        /// <summary>
        /// Resets the computer with the given input register values.
        /// </summary>
        /// <param name="registerA"></param>
        /// <param name="registerB"></param>
        /// <param name="registerC"></param>
        public void Reset(long registerA, long registerB, long registerC)
        {
            InstructionPointer = 0;
            _outputIndex = 0;
            Array.Fill<byte>(Output, 0);

            _registerA = registerA;
            _registerB = registerB;
            _registerC = registerC;
        }

        public void Adv()
        {
            // The denominator is found by raising 2 to the power of the instruction's combo operand.
            var denominator = Math.Pow(2, GetComboOperand());

            // The numerator is the value in the A register.
            // The result of the division is truncated and then written to the A register.
            _registerA = (int)(_registerA / denominator);

            // All operations expect jumps should increment the instruction pointer by two.
            InstructionPointer += 2;
        }

        public void Bxl()
        {
            // The bxl instruction (opcode 1) calculates the bitwise XOR of register B and the instruction's literal operand,
            // then stores the result in register B.
            _registerB = _registerB ^ GetLiteralOperand();

            InstructionPointer += 2;
        }

        public void Bst()
        {
            // The bst instruction (opcode 2) calculates the value of its combo operand modulo 8,
            // then writes that value to the B register.
            _registerB = GetComboOperand() % 8;

            InstructionPointer += 2;
        }

        public void Jnz()
        {
            // JNZ (Jump if Not Zero) will do the normal +2 increment if the A register is zero.
            if (_registerA == 0)
            {
                InstructionPointer += 2;
            }
            else
            {
                // Otherwise if will jump to the position indicated by its literal operand.
                InstructionPointer = GetLiteralOperand();
            }
        }

        public void Bxc()
        {
            // The bxc instruction (opcode 4) calculates the bitwise XOR of register B and register C,
            // then stores the result in register B.
            // The operand is simply ignored.
            _registerB = _registerB ^ _registerC;

            InstructionPointer += 2;
        }

        public void Out()
        {
            // Out instruction calculates the combo operand value modulo 8 and outputs it.
            Output[_outputIndex++] = (byte)(GetComboOperand() % 8);

            InstructionPointer += 2;
        }

        public void Bdv()
        {
            // The bdv instruction works exactly like the adv instruction,
            // expect that the result is stored in the B register.
            // To keep the functionality the same, we can grab the register A value,
            var regA = _registerA;

            // Apply the Adv instruction,
            Adv();

            // Copy the A register (calculation output) to B,
            _registerB = _registerA;

            // then restore the A register.
            _registerA = regA;
        }

        public void Cdv()
        {
            // The cdv works like the adv, but saves to the C register.
            // Do the same switcharoo as with Bdv.
            var regA = _registerA;
            // Apply the Adv instruction,
            Adv();

            // Copy the A register (calculation output) to C,
            _registerC = _registerA;

            // then restore the A register.
            _registerA = regA;
        }

        /// <summary>
        /// Returns the operand for the current instruction pointer in literal mode.
        /// </summary>
        /// <returns></returns>
        private int GetLiteralOperand()
        {
            // Pull the operand from the position right after the instruction pointer.
            if (InstructionPointer == Program.Length - 1)
            {
                throw new InvalidOperationException(
                    "Instruction Pointer is at last Program position - cannot read Operand.");
            }

            // Literal operand should be returned as is.
            return Program[InstructionPointer + 1];
        }

        /// <summary>
        /// Returns the operand for the current instruction pointer in combo mode.
        /// </summary>
        /// <returns></returns>
        private long GetComboOperand()
        {
            // Pull the operand from the position right after the instruction pointer.
            if (InstructionPointer == Program.Length - 1)
            {
                throw new InvalidOperationException(
                    "Instruction Pointer is at last Program position - cannot read Operand.");
            }

            // Start by reading the operand as is.
            var value = Program[InstructionPointer + 1];

            // Combo operands are processed as follows
            return value switch
            {
                // Combo operands 0-3 represent literal values 0 through 3.
                0 => 0,
                1 => 1,
                2 => 2,
                3 => 3,
                // Combo operands 4-6 represent the values of registers A-C respectively.
                4 => _registerA,
                5 => _registerB,
                6 => _registerC,
                // Combo operand is reserved and will not appear in valid programs.
                7 => throw new InvalidOperationException("Encountered reserved operand 7"),
                // value is a byte at this point, just casted to integer.
                // Compiler does want all switch cases to be handled though.
                _ => throw new InvalidOperationException("How did we get a non-byte value after byte cast??")
            };
        }
    }

    public enum OpCode : byte
    {
        Adv = 0,
        Bxl = 1,
        Bst = 2,
        Jnz = 3,
        Bxc = 4,
        Out = 5,
        Bdv = 6,
        Cdv = 7
    }
}
