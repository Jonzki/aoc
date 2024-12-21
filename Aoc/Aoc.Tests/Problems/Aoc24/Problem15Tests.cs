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

    public const string SmallInput3 = """
                                      #######
                                      #...#.#
                                      #.....#
                                      #..OO@#
                                      #..O..#
                                      #.....#
                                      #######

                                      <vv<<^^<<^^
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
        // 105 + 207 + 306
        RunPart2(618, SmallInput3);

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

    [TestMethod]
    public void WideMovement_Works_Left()
    {
        const char left = '<';
        var input = """
                    #.O.O.@.

                    <
                    """;

        var (map, moves) = Problem15.ParseInput(input, wide: true);

        map.Draw("Initial");

        map.Width.Should().Be(16);
        map.Height.Should().Be(1);
        moves.Should().HaveCount(1);

        // Double wall
        map.Walls.Should().Contain(p => p.PositionEquals(0, 0));
        map.Walls.Should().Contain(p => p.PositionEquals(1, 0));

        // Two boxes at 4,0 and 8,0
        map.Boxes.Should().Contain(p => p.PositionEquals(4, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(8, 0));

        // Robot at 10,0
        map.Robot.Should().Be(new Point2D(12, 0));

        // Move left twice. Only robot should move.
        map.Move(left);
        map.Move(left);
        map.Robot.Should().Be(new Point2D(10, 0));

        map.Draw("Two moves should not move boxes");

        // Boxes still at same places
        map.Boxes.Should().Contain(p => p.PositionEquals(4, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(8, 0));

        // One more left. First box should move by one.
        map.Move(left);
        map.Draw("Left move should push first box");

        map.Robot.Should().Be(new Point2D(9, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(4, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(7, 0));

        // Next left still moves only the first box.
        map.Move(left);
        map.Draw("Left move should push first box");

        map.Robot.Should().Be(new Point2D(8, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(4, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(6, 0));

        // Next lefts moves both boxes
        map.Move(left);
        map.Draw("Left move should push both boxes");

        map.Robot.Should().Be(new Point2D(7, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(3, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(5, 0));

        // Next lefts moves both boxes
        map.Move(left);
        map.Draw("Left move should push both boxes");

        map.Robot.Should().Be(new Point2D(6, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(2, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(4, 0));

        // Final left should not move anything.
        map.Move(left);
        map.Draw("Final left should be blocked");

        map.Robot.Should().Be(new Point2D(6, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(2, 0));
        map.Boxes.Should().Contain(p => p.PositionEquals(4, 0));
    }

    [TestMethod]
    public void WideMovement_Works_Vertical()
    {
        const char up = '^';

        var map = new Problem15.Map
        {
            IsWide = true,
            Width = 5,
            Height = 4,
            Walls = { (0, 0), (1, 0), (2, 0), (3, 0), (4, 0) },
            Boxes = { (1, 2), (3, 2) },
            Robot = (3, 3)
        };
        map.Draw("Initial");

        map.Move(up);

        map.Draw("Should push right box up");
        map.Robot.Should().Be(new Point2D(3, 2));
        map.Boxes.Should().Contain(p => p.PositionEquals(1, 2));
        map.Boxes.Should().Contain(p => p.PositionEquals(3, 1));

        map.Move(up);

        map.Draw("Second up should be blocked");
        map.Robot.Should().Be(new Point2D(3, 2));
        map.Boxes.Should().Contain(p => p.PositionEquals(1, 2));
        map.Boxes.Should().Contain(p => p.PositionEquals(3, 1));

        // Remove one wall from the map.
        map.Walls.Remove(new Point2D(3, 0));

        map.Move(up);
        map.Draw("Single wall should still block");
        map.Robot.Should().Be(new Point2D(3, 2));
        map.Boxes.Should().Contain(p => p.PositionEquals(3, 1));

        // Remove the other wall from the map.
        map.Walls.Add(new Point2D(3, 0));
        map.Walls.Remove(new Point2D(4, 0));

        map.Move(up);
        map.Draw("Single wall should still block");
        map.Robot.Should().Be(new Point2D(3, 2));
        map.Boxes.Should().Contain(p => p.PositionEquals(3, 1));
    }

    [TestMethod]
    public void WideMovement_Pushes_BoxesCascade()
    {
        const char down = 'v';

        var map = new Problem15.Map
        {
            IsWide = true,
            Width = 4,
            Height = 6,
            Walls = { (0, 5), (1, 5), (2, 5), (3, 5), (4, 5), (2, 4) },
            Boxes = { (1, 1), (0, 2), (2, 2) },
            Robot = (2, 0)
        };
        map.Draw("Initial");

        map.Move(down);

        map.Draw("All boxes should move down once");
        map.Robot.Should().Be(new Point2D(2, 1));

        map.Boxes.Should().ContainEquivalentOf(new Point2D(1, 2));
        map.Boxes.Should().ContainEquivalentOf(new Point2D(0, 3));
        map.Boxes.Should().ContainEquivalentOf(new Point2D(2, 3));

        map.Move(down);

        map.Draw("One blocking wall should block all movement");
        map.Robot.Should().Be(new Point2D(2, 1));

        map.Boxes.Should().ContainEquivalentOf(new Point2D(1, 2));
        map.Boxes.Should().ContainEquivalentOf(new Point2D(0, 3));
        map.Boxes.Should().ContainEquivalentOf(new Point2D(2, 3));
    }
}