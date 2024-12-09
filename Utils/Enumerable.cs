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

    /// <summary>
    ///     Returns all indices where elements match the given predicate
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="enumerable">The collection to search</param>
    /// <param name="predicate">The condition to match</param>
    /// <returns>List of indices where the predicate is true</returns>
    public static List<int> FindIndices<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate) =>
        enumerable
            .Select((item, index) => new { item, index })
            .Where(x => predicate(x.item))
            .Select(x => x.index)
            .ToList();

    /// <summary>
    ///     Creates alternating sequences based on even/odd indices
    /// </summary>
    /// <typeparam name="T">The type of elements in the source collection</typeparam>
    /// <typeparam name="TResult">The type of elements in the result</typeparam>
    /// <param name="source">The source collection</param>
    /// <param name="evenSelector">Function to generate sequences for even indices</param>
    /// <param name="oddSelector">Function to generate sequences for odd indices</param>
    /// <returns>Flattened sequence of alternating elements</returns>
    public static IEnumerable<TResult> CreateAlternatingSequence<T, TResult>(
        this IEnumerable<T> source,
        Func<T, int, IEnumerable<TResult>> evenSelector,
        Func<T, int, IEnumerable<TResult>> oddSelector) =>
        source
            .Select((item, index) => (index % 2 == 0 ? evenSelector : oddSelector)(item, index))
            .SelectMany(x => x);

    /// <summary>
    ///     Finds the start position of the first sequence of consecutive elements matching a predicate
    ///     that is at least the specified length
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="enumerable">The collection to search</param>
    /// <param name="predicate">The condition to match</param>
    /// <param name="minLength">Minimum length of consecutive matching elements</param>
    /// <param name="maxStartIndex">Maximum index to start searching from</param>
    /// <returns>Starting index of the first matching sequence, or -1 if none found</returns>
    public static int FindConsecutiveSequence<T>(
        this IEnumerable<T> enumerable,
        Func<T, bool> predicate,
        int minLength,
        int maxStartIndex = int.MaxValue)
    {
        var list = enumerable.ToList();
        var start = -1;
        var count = 0;

        for (var i = 0; i < Math.Min(list.Count, maxStartIndex); i++)
        {
            if (predicate(list[i]))
            {
                if (start == -1)
                {
                    start = i;
                }

                count++;
                if (count >= minLength)
                {
                    return start;
                }
            }
            else
            {
                start = -1;
                count = 0;
            }
        }

        return -1;
    }
}