namespace Aoc.Utils
{
    public class IntCodeComputer
    {
        public IntCodeComputer(IEnumerable<long> program)
        {
            OriginalProgram = program.ToArray();

            Input = new Queue<long>();
            Output = new List<long>();
        }

        public IntCodeComputer(string program)
        {
            OriginalProgram = program.Split(',').Select(long.Parse).ToArray();
            Input = new Queue<long>();
            Output = new List<long>();
        }

        /// <summary>
        /// Copy of the original program.
        /// </summary>
        private long[] OriginalProgram { get; }

        private long[]? _executionMemory = null;

        /// <summary>
        /// Returns the full output array of the program.
        /// </summary>
        /// <returns></returns>
        public List<long> Output { get; private set; }

        /// <summary>
        /// Returns the last output of the program, or null if no output has been produced.
        /// </summary>
        /// <returns></returns>
        public long? GetLastOutput() => Output.Count > 0 ? Output.Last() : null;

        private Queue<long> Input { get; }

        /// <summary>
        /// Amounts of operations the computer has run.
        /// </summary>
        public int OperationCount { get; private set; } = 0;

        public ExecutionState State { get; private set; }

        private int _operationIndex = 0;

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
            // Reset if the execution memory has not been set up yet.
            if (_executionMemory == null)
            {
                Reset();
            }

            var memory = new long[_executionMemory!.Length];
            _executionMemory.CopyTo(memory, 0);
            return memory;
        }

        /// <summary>
        /// Resets the computer.
        /// </summary>
        public void Reset()
        {
            // Copy the program into "execution memory".
            _executionMemory = new long[OriginalProgram.Length];
            OriginalProgram.CopyTo(_executionMemory, 0);
        }

        /// <summary>
        /// Executes the computer until it halts or requires more input.
        /// </summary>
        /// <param name="input">Input buffer for the program.</param>
        /// <returns></returns>
        public ExecutionState Execute(params long[] input)
        {
            // Reset if the execution memory has not been set up yet.
            if (_executionMemory == null)
            {
                Reset();
            }

            // Push input into the buffer.
            foreach (var item in input) Input.Enqueue(item);

            for (var i = 0; i < MaxIterations; ++i)
            {
                State = DoOperation();
                ++OperationCount;
                if (State != ExecutionState.Running)
                {
                    return State;
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
        public ExecutionState DoOperation()
        {
            long? output = null;

            // Read operation code at index.
            var operation = _executionMemory![_operationIndex];
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
                    return _executionMemory[inputIndex + paramIndex + 1];
                }
                else
                {
                    return _executionMemory[_executionMemory[inputIndex + paramIndex + 1]];
                }
            }

            // Force-set next operation.
            int? nextOperationIndex = null;
            var paramCount = 0;
            if (opCode == 1) // 1 = Sum
            {
                paramCount = 3;
                var value1 = GetValue(ref _executionMemory, _operationIndex, 0);
                var value2 = GetValue(ref _executionMemory, _operationIndex, 1);
                var outPos = _executionMemory[_operationIndex + 3];
                _executionMemory[outPos] = value1 + value2;
            }
            else if (opCode == 2) // 2 = Multiply
            {
                paramCount = 3;
                var value1 = GetValue(ref _executionMemory, _operationIndex, 0);
                var value2 = GetValue(ref _executionMemory, _operationIndex, 1);
                var outPos = _executionMemory[_operationIndex + 3];
                _executionMemory[outPos] = value1 * value2;
            }
            else if (opCode == 3) // 3 = Input
            {
                paramCount = 1;

                if (Input.TryDequeue(out var input))
                {
                    var outPos = _executionMemory[_operationIndex + 1];
                    _executionMemory[outPos] = input;
                }
                else
                {
                    // No input available - return and wait for input.
                    // Return current operation index (rerun).
                    return ExecutionState.WaitingForInput;
                }
            }
            else if (opCode == 4) // 4 = Output
            {
                paramCount = 1;
                output = GetValue(ref _executionMemory, _operationIndex, 0);
            }
            else if (opCode == 5) // 5 = Jump-If-True
            {
                paramCount = 2;
                var value1 = GetValue(ref _executionMemory, _operationIndex, 0);
                var value2 = GetValue(ref _executionMemory, _operationIndex, 1);
                if (value1 != 0)
                {
                    nextOperationIndex = (int)value2;
                }
            }
            else if (opCode == 6) // 6 = Jump-If-False
            {
                paramCount = 2;
                var value1 = GetValue(ref _executionMemory, _operationIndex, 0);
                var value2 = GetValue(ref _executionMemory, _operationIndex, 1);
                if (value1 == 0)
                {
                    nextOperationIndex = (int)value2;
                }
            }
            else if (opCode == 7) // 7 = Less Than
            {
                paramCount = 3;
                var value1 = GetValue(ref _executionMemory, _operationIndex, 0);
                var value2 = GetValue(ref _executionMemory, _operationIndex, 1);
                var outPos = _executionMemory[_operationIndex + 3];
                _executionMemory[outPos] = value1 < value2 ? 1 : 0;
            }
            else if (opCode == 8) // 8 = Equals
            {
                paramCount = 3;
                var value1 = GetValue(ref _executionMemory, _operationIndex, 0);
                var value2 = GetValue(ref _executionMemory, _operationIndex, 1);
                var outPos = _executionMemory[_operationIndex + 3];
                _executionMemory[outPos] = value1 == value2 ? 1 : 0;
            }
            else if (opCode == 99)
            {
                // opCode 99 = halt application.
                return ExecutionState.Halted;
            }

            // Don't halt. Add output if defined, increment the operation index.
            if (output.HasValue)
            {
                Output.Add(output.Value);
            }
            _operationIndex = nextOperationIndex ?? _operationIndex + paramCount + 1;

            return ExecutionState.Running;
        }

        public enum ExecutionState
        {
            Running = 0,
            Halted = 1,
            WaitingForInput = 2
        }
    }
}
