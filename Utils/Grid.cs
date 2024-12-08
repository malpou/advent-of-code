namespace AdventOfCode.Utils;

/// <summary>
///     Extension methods for working with 2D grids represented as arrays
/// </summary>
public static class GridExtensions
{
    /// <summary>
    ///     Searches for a pattern in a grid starting from a point and moving in a direction
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid</typeparam>
    /// <param name="grid">The 2D grid to search in</param>
    /// <param name="start">The starting point for the pattern search</param>
    /// <param name="direction">The direction to search in</param>
    /// <param name="pattern">The sequence of elements to match</param>
    /// <returns>true if the pattern is found starting at the point and following the direction, false otherwise</returns>
    public static bool FindPattern<T>(this T[][] grid, Point start, Vector direction, T[] pattern)
        where T : IEquatable<T>
    {
        var current = start;

        foreach (var target in pattern)
        {
            if (!current.IsInBounds() || !grid[current.Y][current.X].Equals(target))
            {
                return false;
            }

            current += direction;
        }

        return true;
    }

    /// <summary>
    ///     Gets an array of elements along a diagonal path between two offset points from a starting position
    /// </summary>
    /// <typeparam name="T">The type of elements in the grid</typeparam>
    /// <param name="grid">The 2D grid to get elements from</param>
    /// <param name="start">The central point from which offsets are applied</param>
    /// <param name="startOffset">The vector offset for the start of the diagonal</param>
    /// <param name="endOffset">The vector offset for the end of the diagonal</param>
    /// <returns>An array of elements along the diagonal path, or empty if the path is invalid or out of bounds</returns>
    public static T[] GetDiagonalElements<T>(this T[][] grid, Point start, Vector startOffset, Vector endOffset)
        where T : IEquatable<T>
    {
        var startPoint = start + startOffset;
        var endPoint = start + endOffset;

        if (!startPoint.IsInBounds() || !endPoint.IsInBounds())
        {
            return [];
        }

        var direction = new Vector(Math.Sign(endOffset.X - startOffset.X),
            Math.Sign(endOffset.Y - startOffset.Y));

        var steps = Math.Abs(endOffset.X - startOffset.X);
        if (steps != Math.Abs(endOffset.Y - startOffset.Y))
        {
            return []; // Not same diagonal
        }

        var result = new T[steps + 1];
        var current = startPoint;

        for (var i = 0; i <= steps; i++)
        {
            if (!current.IsInBounds())
            {
                return [];
            }

            result[i] = grid[current.Y][current.X];
            current += direction;
        }

        return result;
    }

    /// <summary>
    ///     Converts an array of strings to a 2D char array
    /// </summary>
    /// <param name="input">Array of strings to convert</param>
    /// <returns>A 2D array of characters representing the grid</returns>
    public static char[][] ToCharGrid(this string[] input) => input.Select(row => row.ToCharArray()).ToArray();
}