using System.Collections.Generic;
using System.Linq;
using Aoc.Problems.Aoc24;
using Aoc.Utils;

namespace Aoc.Tests.Problems.Aoc24;

[TestClass]
public class Problem16Tests : ProblemTests<Problem16>
{
    public const string SmallInput1 = """
                                     ###############
                                     #.......#....E#
                                     #.#.###.#.###.#
                                     #.....#.#...#.#
                                     #.###.#####.#.#
                                     #.#.#.......#.#
                                     #.#.#####.###.#
                                     #...........#.#
                                     ###.#.#####.#.#
                                     #...#.....#.#.#
                                     #.#.#.###.#.#.#
                                     #.....#...#.#.#
                                     #.###.#.#.#.#.#
                                     #S..#.....#...#
                                     ###############
                                     """;

    public const string SmallInput2 = """
                                      #################
                                      #...#...#...#..E#
                                      #.#.#.#.#.#.#.#.#
                                      #.#.#.#...#...#.#
                                      #.#.#.#.###.#.#.#
                                      #...#.#.#.....#.#
                                      #.#.#.#.#.#####.#
                                      #.#...#.#.#.....#
                                      #.#.#####.#.###.#
                                      #.#.#.......#...#
                                      #.#.###.#####.###
                                      #.#.#...#.....#.#
                                      #.#.#.#####.###.#
                                      #.#.#.........#.#
                                      #.#.#.#########.#
                                      #S#.............#
                                      #################
                                      """;

    // Mini input 1: Going right first should be the optimal path (one less turn).
    public const string MiniInput1 = """
                                    #####
                                    #..E#
                                    #.#.#
                                    #S..#
                                    #####
                                    """;

    // Mini input 2: Both routes should be equally good,
    // but we have a simple intersection to test.
    public const string MiniInput2 = """
                                     #######
                                     #.....#
                                     #.#.#.#
                                     #S..#E#
                                     #######
                                     """;

    // Mini input 3: Upper route is worse.
    public const string MiniInput3 = """
                                     #######
                                     #...#.#
                                     #.#...#
                                     #.#.#.#
                                     #S..#E#
                                     #######
                                     """;

    // Single possible path, just to see that the navigation works
    public const string TrivialInput = """
                                #######
                                #...#E#
                                #.#.#.#
                                #S#...#
                                #######
                                """;

    public const string SmallLoopInput = """
                                         #######
                                         ###..E#
                                         ###.###
                                         ##...##
                                         ##.#.##
                                         ##...##
                                         ###.###
                                         #S..###
                                         #######
                                         """;

    [TestMethod]
    public void SolvePart1()
    {
        RunPart1(7036, SmallInput1);

        RunPart1(11048, SmallInput2);
    }

    [TestMethod]
    public void SolvePart2()
    {
        RunPart2(45, SmallInput1);

        //RunPart2(64, SmallInput2);
    }

    [TestMethod]
    public void ResolvePaths_WorksWith_Trivial()
    {
        var map = Problem16.ParseMap(TrivialInput);

        var paths = Problem16.ResolvePaths(map, true);

        paths.Should().HaveCount(1);
    }

    [TestMethod]
    public void CalculateVisited_WorksWith_Trivial()
    {
        // Single possible path, just to see that the navigation works
        const string trivialInput = """
                                    #######
                                    #...#E#
                                    #.#.#.#
                                    #S#...#
                                    #######
                                    """;

        var map = Problem16.ParseMap(trivialInput);

        // We calculated the best score of 5010 in the other test.
        var paths = Problem16.ResolvePaths(map, false, 5010);

        paths.Should().HaveCount(1);

        var visited = paths.First().CalculateVisited(map);

        visited.Should().HaveCount(11);
    }

    [TestMethod]
    public void CalculateVisited_WorksWith_SimpleLoop()
    {
        var map = Problem16.ParseMap(SmallLoopInput);

        var paths = Problem16.ResolvePaths(map, false);

        // Two possible paths of equal scores.
        paths.Should().HaveCount(2);

        var visited = new HashSet<Point2D>();
        foreach (var path in paths)
        {
            visited.AddRange(path.CalculateVisited(map));
        }

        // All map points should have been visited.
        visited.Should().HaveCount(map.Positions.Count);
    }

    [TestMethod]
    public void Part2_WorksWith_MiniInput()
    {
        // 2/3 paths should be the same cost.
        const string input = """
                                    #######
                                    ##...E#
                                    ##.####
                                    #...###
                                    #.#.###
                                    #.....#
                                    ##.##.#
                                    #S....#
                                    #######
                                    """;

        RunPart2(16, input);
    }

    [TestMethod]
    public void ResolvePaths2_WorksWith_Trivial()
    {
        var map = Problem16.ParseMap(TrivialInput);
        var paths = Problem16.ResolvePaths(map);
        paths.Should().HaveCount(1);
    }

    [TestMethod]
    public void ResolvePaths2_WorksWith_TwoRoutes()
    {
        var map = Problem16.ParseMap(SmallLoopInput);
        var paths = Problem16.ResolvePaths(map);
        paths.Should().HaveCount(2);
    }

}