using Aoc.Problems.Aoc25;

namespace Aoc.Tests.Problems.Aoc25;

[TestClass]
public class Problem02Tests : ProblemTests<Problem02>
{
    [TestMethod]
    public void SolvePart1()
    {
        const string smallInput = """
                                  11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
                                  """;

        RunPart1(1227775554L, smallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        const string smallInput = """
                                  11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124
                                  """;

        RunPart2(4174379265L, smallInput);
    }

    [DataTestMethod]
    [DataRow(11L, true, "Seq: 1")]
    [DataRow(12L, false, "No repeats")]
    [DataRow(123123L, true, "Seq: 123")]
    [DataRow(1231231L, false, "Partial seq: 123")]
    [DataRow(123123123L, false, "Seq: 123 but should only be found twice")]
    public void IsInvalidId_WorksFor_BasicCases_Part1(long input, bool correctIsInvalid, string reason)
    {
        var isInvalid = Problem02.IsInvalidId(input);

        isInvalid.Should().Be(correctIsInvalid, reason);
    }

    [TestMethod]
    public void IdRange_FindInvalidIds_Works_Small()
    {
        var range = Problem02.IdRange.Parse("11-22");
        var invalidIds = range.FindInvalidIds();

        invalidIds.Should().HaveCount(2);
        invalidIds.Should().Contain(11L);
        invalidIds.Should().Contain(22L);
    }

    [TestMethod]
    public void IdRange_FindInvalidIds_Works_Large()
    {
        var range = Problem02.IdRange.Parse("1188511880-1188511890");
        var invalidIds = range.FindInvalidIds();

        invalidIds.Should().HaveCount(1);
        invalidIds.Should().Contain(1188511885L);
    }

    [DataTestMethod]
    [DataRow(11L, true, "Seq: 1")]
    [DataRow(12L, false, "No repeats")]
    [DataRow(123123L, true, "Seq: 123")]
    [DataRow(123123123L, true, "Seq: 123, 3 times")]
    public void IsInvalidId_WorksFor_BasicCases_Part2(long input, bool correctIsInvalid, string reason)
    {
        var isInvalid = Problem02.IsInvalidId(input, mode: 2);

        isInvalid.Should().Be(correctIsInvalid, reason);
    }
}