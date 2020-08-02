namespace Aoc19.Utils
{
    public static class NumberUtils
    {
        /// <summary>
        /// Returns a roll-over index for the input index and array size.
        /// Values larger than array size are rotated to the start of the array.
        /// Values less than zero are rotated back of the array.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int GetRolloverIndex(int index, int length)
        {
            var i = index + 0;
            // Negative index should roll to the back 
            while (i < 0) i = length + i;
            while (i >= length) i = i - length;
            return i;
        }
    }
}
