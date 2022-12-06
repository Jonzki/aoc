namespace Aoc.Utils;

public static class ObjectUtils
{
    public static bool IsAnyOf<T>(this T obj, params T[] objects) => objects.Contains(obj);
}
