using System;

namespace Aoc.Tests.Problems;

/// <summary>
/// Base class for defining small input tests for parts 1 and 2 of a Problem.
/// </summary>
/// <typeparam name="TProblem"></typeparam>
[TestClass]
public abstract class ProblemTests<TProblem> where TProblem : IProblem, new()
{
    protected void RunPart1(object correctOutput, string input, Action<TProblem>? beforeTest = null)
        => RunPart1(correctOutput.GetType(), correctOutput, input, beforeTest);

    protected void RunPart1<TOut>(TOut correctOutput, string input, Action<TProblem>? beforeTest = null)
        => RunPart1(typeof(TOut), correctOutput, input, beforeTest);

    private void RunPart1(Type correctOutputType, object? correctOutput, string input, Action<TProblem>? beforeTest = null)
    {
        var problem = new TProblem();

        beforeTest?.Invoke(problem);

        var output = problem.Solve1(input);

        // If the assertion and output types are both int or long, compare both as long.
        if (output is int or long && correctOutput is int or long)
        {
            var outLong = long.Parse(output.ToString()!);
            var correctLong = long.Parse(correctOutput.ToString()!);
            outLong.Should().Be(correctLong);
            return;
        }

        // Check for type equality first (typically int vs. long).
        output.GetType().FullName.Should().Be(correctOutputType.FullName, "the Types of expected and actual outputs must match.");

        // Then proceed with normal equality check.
        output.Should().Be(correctOutput);
    }

    protected void RunPart2(object correctOutput, string input, Action<TProblem>? beforeTest = null)
        => RunPart2(correctOutput.GetType(), correctOutput, input, beforeTest);

    protected void RunPart2<TOut>(TOut correctOutput, string input, Action<TProblem>? beforeTest = null)
        => RunPart2(typeof(TOut), correctOutput, input, beforeTest);

    private void RunPart2(Type correctOutputType, object? correctOutput, string input, Action<TProblem>? beforeTest = null)
    {
        var problem = new TProblem();

        beforeTest?.Invoke(problem);

        var output = problem.Solve2(input);

        // If the assertion and output types are both int or long, compare both as long.
        if (output is int or long && correctOutput is int or long)
        {
            var outLong = long.Parse(output.ToString()!);
            var correctLong = long.Parse(correctOutput.ToString()!);
            outLong.Should().Be(correctLong);
            return;
        }

        // Check for type equality first (typically int vs. long).
        output.GetType().FullName.Should().Be(correctOutputType.FullName, "the Types of expected and actual outputs must match.");

        // Then proceed with normal equality check.
        output.Should().Be(correctOutput);
    }
}