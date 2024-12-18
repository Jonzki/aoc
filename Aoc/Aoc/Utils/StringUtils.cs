﻿namespace Aoc.Utils
{
    public static class StringUtils
    {
        public static string[] SplitLines(this string input) => input.Replace("\r", "").Split("\n");

        /// <summary>
        /// Removes all occurrences of the input text from the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="textToRemove"></param>
        /// <returns></returns>
        public static string RemoveString(this string input, string textToRemove) => input.Replace(textToRemove, string.Empty);

        /// <summary>
        /// Removes all occurrences of input texts from the string.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="textsToRemove"></param>
        /// <returns></returns>
        public static string RemoveStrings(this string input, params string[] textsToRemove)
        {
            string output = input;
            foreach (var t in textsToRemove)
            {
                output = RemoveString(output, t);
            }
            return output;
        }

        public static string RemoveAll(this string input, char character) => RemoveString(input, character.ToString());
    }
}
