namespace AdventOfCode.Utils;

public static class EnumerableUtils
{
    public static IEnumerable<(T, T)> GetUniquePairs<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        return from i in Enumerable.Range(0, list.Count)
            from j in Enumerable.Range(i + 1, list.Count - i - 1)
            select (list[i], list[j]);
    }
}