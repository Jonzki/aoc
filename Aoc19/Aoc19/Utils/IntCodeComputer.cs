namespace Aoc19.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IntCodeComputer
    {
        public IntCodeComputer(IEnumerable<long> program)
        {
            OriginalProgram = program.ToArray();
        }

        public IntCodeComputer(string program)
        {
            this.OriginalProgram = program.Split(',').Select(long.Parse).ToArray();
        }

        private long[] OriginalProgram { get; }

        private long[] _executionMemory;

        private ExecutionState State { get; set; }

        /// <summary>
        /// Hard limit for program execution.
        /// </summary>
        private const long MaxIterations = 1_000_000;

        /// <summary>
        /// Returns the current state of the execution memory.
        /// </summary>
        /// <returns></returns>
        public long[] GetExecutionMemory()
        {
            var memory = new long[_executionMemory.Length];
            _executionMemory.CopyTo(memory, 0);
            return memory;
        }

        /// <summary>
        /// Returns the full output array of the program.
        /// </summary>
        /// <returns></returns>
        public List<long> GetOutput() => State?.Output;

        /// <summary>
        /// Executes the computer. Returns all output produced by it.
        /// </summary>
        /// <param name="input">Input buffer for the program.</param>
        /// <returns></returns>
        public long? Execute(params long[] input)
        {
            // Copy the program into "execution memory".
            _executionMemory = new long[OriginalProgram.Length];
            OriginalProgram.CopyTo(_executionMemory, 0);

            var inputBuffer = new Queue<long>(input);

            State = new ExecutionState();
            // Push input into the buffer.
            foreach (var item in input) State.InputBuffer.Enqueue(item);


            long? operationOutput;
            for (var i = 0; i < MaxIterations; ++i)
            {
                (State.ShouldHalt, State.OperationIndex, operationOutput) = DoOperation(ref _executionMemory, State.OperationIndex, inputBuffer);
                if (operationOutput.HasValue) State.Output.Add(operationOutput.Value);
                if (State.ShouldHalt)
                {
                    return State.Output.Count > 0 ? State.Output.Last() : (long?)null;
                }
            }
            // Hard-limit reached.
            throw new InvalidOperationException("MaxIterations reached in IntCodeComputer.");
        }

        /// <summary>
        /// Runs one operation on the input array.
        /// </summary>
        /// <param name="array">Array to operate in.</param>
        /// <param name="operationIndex">Index of the operation code.</param>
        /// <returns>True if program should halt; false otherwise.</returns>
        public static (bool shouldHalt, int nextOperationIndex, long? output) DoOperation(ref long[] array, int operationIndex, Queue<long> inputBuffer)
        {
            long? output = null;

            // Read operation code at index.
            var operation = array[operationIndex];
            // Parse operation code and parameter modes.
            var opCode = operation % 100;

            var immediate = new bool[]
            {
                (operation/100) % 10 == 1,
                (operation/1000) % 10 == 1,
                (operation/10000) % 10 == 1
            };

            // Helper for finding a value from execution memory.
            long GetValue(ref long[] array, int inputIndex, int paramIndex)
            {
                if (immediate[paramIndex])
                {
                    return array[inputIndex + paramIndex + 1];
                }
                else
                {
                    return array[array[inputIndex + paramIndex + 1]];
                }
            }

            // Force-set next operation.
            int? nextOperationIndex = null;
            var paramCount = 0;
            if (opCode == 1) // 1 = Sum
            {
                paramCount = 3;
                var value1 = GetValue(ref array, operationIndex, 0);
                var value2 = GetValue(ref array, operationIndex, 1);
                var outPos = array[operationIndex + 3];
                array[outPos] = value1 + value2;
            }
            else if (opCode == 2) // 2 = Multiply
            {
                paramCount = 3;
                var value1 = GetValue(ref array, operationIndex, 0);
                var value2 = GetValue(ref array, operationIndex, 1);
                var outPos = array[operationIndex + 3];
                array[outPos] = value1 * value2;
            }
            else if (opCode == 3) // 3 = Input
            {
                paramCount = 1;

                var input = inputBuffer?.Dequeue();
                if (!input.HasValue)
                {
                    // Fall back to CLI input.
                    Console.Write("No input available in buffer - please input a number: ");
                    input = long.Parse(Console.ReadLine());
                }
                var outPos = array[operationIndex + 1];
                array[outPos] = input.Value;
            }
            else if (opCode == 4) // 4 = Output
            {
                paramCount = 1;
                output = GetValue(ref array, operationIndex, 0);
            }
            else if (opCode == 5) // 5 = Jump-If-True
            {
                paramCount = 2;
                var value1 = GetValue(ref array, operationIndex, 0);
                var value2 = GetValue(ref array, operationIndex, 1);
                if (value1 != 0)
                {
                    nextOperationIndex = (int)value2;
                }
            }
            else if (opCode == 6) // 6 = Jump-If-False
            {
                paramCount = 2;
                var value1 = GetValue(ref array, operationIndex, 0);
                var value2 = GetValue(ref array, operationIndex, 1);
                if (value1 == 0)
                {
                    nextOperationIndex = (int)value2;
                }
            }
            else if (opCode == 7) // 7 = Less Than
            {
                paramCount = 3;
                var value1 = GetValue(ref array, operationIndex, 0);
                var value2 = GetValue(ref array, operationIndex, 1);
                var outPos = array[operationIndex + 3];
                array[outPos] = value1 < value2 ? 1 : 0;
            }
            else if (opCode == 8) // 8 = Equals
            {
                paramCount = 3;
                var value1 = GetValue(ref array, operationIndex, 0);
                var value2 = GetValue(ref array, operationIndex, 1);
                var outPos = array[operationIndex + 3];
                array[outPos] = value1 == value2 ? 1 : 0;
            }
            else if (opCode == 99)
            {
                // opCode 99 = halt application.
                return (true, operationIndex, output);
            }

            // Don't halt.
            return (false, nextOperationIndex ?? operationIndex + paramCount + 1, output);
        }

        public class ExecutionState
        {
            public ExecutionState()
            {
                OperationIndex = 0;
                ShouldHalt = false;
                Output = new List<long>();
                InputBuffer = new Queue<long>();
            }

            public int OperationIndex { get; set; }

            public bool ShouldHalt { get; set; }

            public List<long> Output { get; }

            public Queue<long> InputBuffer { get; }
        }
    }
}