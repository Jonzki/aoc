using Aoc.Problems.Aoc22;

namespace Aoc.Tests.Problems.Aoc22;

[TestClass]
public class Problem07Tests : ProblemTests<Problem07>
{
    const string SmallInput = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k";

    const long CorrectOutput1 = 95437;

    const long CorrectOutput2 = 24933642;

    [TestMethod]
    public void Part1_SmallInput_Is_Correct() => RunPart1(CorrectOutput1, SmallInput);

    [TestMethod]
    public void Part2_SmallInput_Is_Correct() => RunPart2(CorrectOutput2, SmallInput);
}
