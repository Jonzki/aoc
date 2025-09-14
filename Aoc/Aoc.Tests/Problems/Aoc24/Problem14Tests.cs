using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem14Tests : ProblemTests<Problem14>
{
    public const string SmallInput = """
                                     p=0,4 v=3,-3
                                     p=6,3 v=-1,-3
                                     p=10,3 v=-1,2
                                     p=2,0 v=2,-1
                                     p=0,0 v=1,3
                                     p=3,0 v=-2,-2
                                     p=7,6 v=-1,-3
                                     p=3,0 v=-1,-2
                                     p=9,3 v=2,3
                                     p=7,3 v=-1,2
                                     p=2,4 v=2,-3
                                     p=9,5 v=-3,-3
                                     """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(12, SmallInput, problem =>
        {
            problem.Width = 11;
            problem.Height = 7;
        });
    }

    [TestMethod]
    public void SolvePart2()
    {
        // In part 2 we are visually searching for a christmas tree.
        // No real testing here to do.
    }

    [TestMethod]
    public void ParseRobot_Works()
    {
        const string input = "p=6,3 v=-1,-3";
        var robot = Problem14.ParseRobot(input);

        robot.Should().NotBeNull();
        robot.Position.Should().Match<Point2D>(p => p.PositionEquals(6, 3));
        robot.Velocity.Should().Match<Point2D>(p => p.PositionEquals(-1, -3));
    }

    [TestMethod]
    public void RobotWrapping_Works()
    {
        const int w = 11, h = 7;

        var robot = Problem14.ParseRobot("p=2,4 v=2,-3");

        // Initial position
        robot.Position.Should().Be(new Point2D(2, 4));

        // After 1 second:
        robot.Simulate(1, w, h);
        robot.Position.Should().Be(new Point2D(4, 1));

        // After 2 seconds:
        robot.Simulate(2, w, h);
        robot.Position.Should().Be(new Point2D(6, 5));

        // After 3 seconds:
        robot.Simulate(3, w, h);
        robot.Position.Should().Be(new Point2D(8, 2));

        // After 4 seconds:
        robot.Simulate(4, w, h);
        robot.Position.Should().Be(new Point2D(10, 6));

        // After 5 seconds:
        robot.Simulate(5, w, h);
        robot.Position.Should().Be(new Point2D(1, 3));
    }
}