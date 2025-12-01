using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem04Tests : ProblemTests<Problem04>
{
    private const string SmallInput = """"
                                      MMMSXXMASM
                                      MSAMXMSMSA
                                      AMXSXMAAMM
                                      MSAMASMSMX
                                      XMASAMXAMM
                                      XXAMMXXAMA
                                      SMSMSASXSS
                                      SAXAMASAAA
                                      MAMMMXMMMM
                                      MXMXAXMASX
                                      """";

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(18, SmallInput);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(9, SmallInput);
    }

    [TestMethod]
    public void ScanWord_WorksFor_Simple()
    {
        var mapString = """
                        XMAS
                        MM..
                        A.A.
                        S..S
                        """.RemoveString("\r\n");
        char[,] map = ArrayUtils.To2D(mapString.ToCharArray(), 4, 4);

        // Horizontal:
        Problem04.ScanWord(map, 0, 0, 1, 0).Should().BeTrue();

        // Vertical:
        Problem04.ScanWord(map, 0, 0, 0, 1).Should().BeTrue();

        // Diagonal down-right:
        Problem04.ScanWord(map, 0, 0, 1, 1).Should().BeTrue();

        // Out of bounds:
        Problem04.ScanWord(map, 0, 0, -1, -1).Should().BeFalse();
    }
}