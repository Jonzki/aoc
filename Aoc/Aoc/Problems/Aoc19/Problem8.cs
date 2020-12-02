using Aoc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aoc.Problems.Aoc19
{
    public class Problem8 : IProblem
    {
        public object Solve1(string input)
        {
            // "The image you received is 25 pixels wide and 6 pixels tall".
            var image = ParseImage(input, 25, 6);

            int minZeros = int.MaxValue;
            int[] minLayer = null;

            // "find the layer that contains the fewest 0 digits"
            foreach (var layer in image.Layers)
            {
                var zeros = layer.Count(x => x == 0);
                if (zeros < minZeros)
                {
                    minZeros = zeros;
                    minLayer = layer;
                }
            }

            // "On that layer, what is the number of 1 digits multiplied by the number of 2 digits?"
            var ones = minLayer.Count(x => x == 1);
            var twos = minLayer.Count(x => x == 2);

            return ones * twos;
        }

        public object Solve2(string input)
        {
            // "The image you received is 25 pixels wide and 6 pixels tall".
            var image = ParseImage(input, 25, 6);

            // Merge into one image.
            var merged = MergeLayers(image);

            // "Render" the image.
            Console.WriteLine(new string('-', image.Width));
            for (var y = 0; y < image.Height; ++y)
            {
                for (var x = 0; x < image.Width; ++x)
                {
                    var pixel = merged[ArrayUtils.GetIndex(x, y, image.Width)];
                    Console.Write(pixel ? "#" : " ");
                }
                Console.WriteLine("");
            }
            Console.WriteLine(new string('-', image.Width));

            return 0;
        }

        /// <summary>
        /// Parses an image from the given input.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Image ParseImage(string input, int width, int height)
        {
            var layers = new List<int[]>();

            // Count the amount of layers we're about to get.
            var layerCount = input.Length / (width * height);
            for (int i = 0; i < layerCount; ++i)
            {
                // Fill in the layer.
                var layerData = input.Substring(i * width * height, width * height).Select(c => c - '0').ToArray();
                layers.Add(layerData);
            }

            return new Image
            {
                Width = width,
                Height = height,
                Layers = layers
            };
        }

        /// <summary>
        /// Merges the input layers into a single "image" of pixels: true = white, false = black.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static bool[] MergeLayers(Image image)
        {
            bool[] merged = new bool[image.Width * image.Height];

            for (int y = 0; y < image.Height; ++y)
            {
                for (int x = 0; x < image.Width; ++x)
                {
                    var index = ArrayUtils.GetIndex(x, y, image.Width);
                    bool? color = null;
                    for (var l = 0; l < image.Layers.Count && color == null; ++l)
                    {
                        // 0 = black
                        if (image.Layers[l][index] == 0)
                        {
                            color = false;
                            break;
                        }
                        // 1 = white
                        if (image.Layers[l][index] == 1)
                        {
                            color = true;
                            break;
                        }
                        // 2 = transparent - continue to next layer.
                    }
                    merged[index] = color == true;
                }
            }

            return merged;
        }

        public record Image
        {
            public int Width { get; init; }

            public int Height { get; init; }

            public List<int[]> Layers { get; init; }
        }
    }
}