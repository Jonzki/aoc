using Aoc.Problems.Aoc24;

namespace Aoc.Tests.Problems.Aoc24
{
    [TestClass]
    public class Problem09Tests : ProblemTests<Problem09>
    {
        public const string SmallInput = "2333133121414131402";

        public const string MiniInput = "12345";

        [TestMethod]
        public void SolvePart1()
        {
            RunPart1(1928L, SmallInput);
        }

        [TestMethod]
        public void SolvePart2()
        {
            RunPart2(2858L, SmallInput);
        }

        [TestMethod]
        public void ParseInput_ProducesCorrectDisk()
        {
            var disk = Problem09.ParseInput(MiniInput);

            var diskString = Problem09.DiskToString(disk);

            diskString.Should().Be("0..111....22222");
        }

        [TestMethod]
        public void ParseFiles_ParsesCorrectFiles()
        {
            // Start with the same MiniInput, parse into a Disk.
            // This was tested previously.
            var disk = Problem09.ParseInput(MiniInput);

            var files = Problem09.ParseFiles(disk);

            // Should get 3 files.
            files.Should().HaveCount(3);

            files.Should().ContainInOrder(
                // File 0: position 0, 1 block.
                new Problem09.File()
                {
                    Id = 0,
                    Position = 0,
                    Size = 1
                },
                // File 1: position 3, 3 blocks.
                new Problem09.File()
                {
                    Id = 1,
                    Position = 3,
                    Size = 3
                },
                // File 2: position 10, 5 blocks
                new Problem09.File()
                {
                    Id = 2,
                    Position = 10,
                    Size = 5
                });
        }

        [TestMethod]
        public void ParseFiles_ParsesSmallInput()
        {
            // Start with the same MiniInput, parse into a Disk.
            // This was tested previously.
            var disk = Problem09.ParseInput(SmallInput);

            var files = Problem09.ParseFiles(disk);

            // Should get 10 files.
            files.Should().HaveCount(10);

            files.Should().Contain([
                new Problem09.File
                {
                    Id = 0,
                    Position = 0,
                    Size = 2
                },
                new Problem09.File
                {
                    Id = 1,
                    Position = 5,
                    Size = 3
                },
                new Problem09.File
                {
                    Id = 9,
                    Position = 40,
                    Size = 2
                }
            ]);
        }

        [TestMethod]
        public void FilesToDisk_Works()
        {
            var disk = Problem09.ParseInput(MiniInput);

            var files = Problem09.ParseFiles(disk);

            var outputDisk = new int[disk.Length];

            Problem09.FilesToDisk(outputDisk, files);

            outputDisk.Should().BeEquivalentTo(disk);
        }

        [TestMethod]
        public void CompressPart2_Works()
        {
            var disk = Problem09.ParseInput(SmallInput);

            Problem09.CompressPart2(disk);

            var diskString = Problem09.DiskToString(disk);

            diskString.Should().Be("00992111777.44.333....5555.6666.....8888..");
        }
    }
}