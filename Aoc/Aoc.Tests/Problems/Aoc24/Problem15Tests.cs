using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem15Tests : ProblemTests<Problem15>
{
    public const string SmallInput1 = """
                                     ########
                                     #..O.O.#
                                     ##@.O..#
                                     #...O..#
                                     #.#.O..#
                                     #...O..#
                                     #......#
                                     ########

                                     <^^>>>vv<v>>v<<
                                     """;

    public const string SmallInput2 = """
                                      ##########
                                      #..O..O.O#
                                      #......O.#
                                      #.OO..O.O#
                                      #..O@..O.#
                                      #O#..O...#
                                      #O..O..O.#
                                      #.OO.O.OO#
                                      #....O...#
                                      ##########

                                      <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                                      vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                                      ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                                      <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                                      ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                                      ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                                      >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                                      <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                                      ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                                      v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                                      """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(2028, SmallInput1);

        RunPart1(10092, SmallInput2);
    }

    [TestMethod]
    public void SolvePart2()
    {
        // In part 2 we are visually searching for a christmas tree.
        // No real testing here to do.
        RunPart2(9021, SmallInput2);
    }

    [TestMethod]
    public void ParseInput_Works()
    {
        var (map, moves) = Problem15.ParseInput(SmallInput1);

        map.Width.Should().Be(8);
        map.Height.Should().Be(8);

        map.Robot.Should().Be(new Point2D(2, 2));
        map.Walls.Should().HaveCount(30);
        map.Boxes.Should().HaveCount(6);

        moves.Should().HaveCount(15);
    }
}