namespace AdventOfCode.Utils;

public static class GridExtensions
{
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

            current = current.AddVector(direction);
        }

        return true;
    }

    public static T[] GetDiagonalElements<T>(this T[][] grid, Point start, Vector startOffset, Vector endOffset)
        where T : IEquatable<T>
    {
        var startPoint = start.AddVector(startOffset);
        var endPoint = start.AddVector(endOffset);

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
            current = current.AddVector(direction);
        }

        return result;
    }

    public static char[][] ToCharGrid(this string[] input) => input.Select(row => row.ToCharArray()).ToArray();
}