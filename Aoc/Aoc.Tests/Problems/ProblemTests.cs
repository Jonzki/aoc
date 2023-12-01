namespace Aoc.Tests.Problems;

/// <summary>
/// Base class for defining small input tests for parts 1 and 2 of a Problem.
/// </summary>
/// <typeparam name="TProblem"></typeparam>
[TestClass]
public abstract class ProblemTests<TProblem> where TProblem : IProblem, new()
{
    protected void RunPart1(object correctOutput, string input)
    {
        var output = new TProblem().Solve1(input);

        // Check for type equality first (typically int vs. long).
        Assert.AreEqual(correctOutput.GetType().FullName, output.GetType().FullName, "The Types of expected and actual outputs must match.");

        // Then proceed with normal equality check.
        Assert.AreEqual(correctOutput, output);
    }

    protected void RunPart2(object correctOutput, string input)
    {
        var output = new TProblem().Solve2(input);

        // Check for type equality first (typically int vs. long).
        Assert.AreEqual(correctOutput.GetType().FullName, output.GetType().FullName, "The Types of expected and actual outputs must match.");

        // Then proceed with normal equality check.
        Assert.AreEqual(correctOutput, output);
    }
}
