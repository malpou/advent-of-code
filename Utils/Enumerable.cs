namespace AdventOfCode.Utils;

/// <summary>
///     Utility methods for working with IEnumerable collections
/// </summary>
public static class EnumerableUtils
{
    /// <summary>
    ///     Generates all unique pairs of elements from a collection
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="enumerable">The collection to generate pairs from</param>
    /// <returns>An enumerable of tuples containing all unique pairs of elements</returns>
    /// <remarks>
    ///     For a collection [1,2,3], returns [(1,2), (1,3), (2,3)]
    ///     Order within pairs and between pairs is preserved from the original collection
    /// </remarks>
    public static IEnumerable<(T, T)> GetUniquePairs<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        return from i in Enumerable.Range(0, list.Count)
            from j in Enumerable.Range(i + 1, list.Count - i - 1)
            select (list[i], list[j]);
    }
}