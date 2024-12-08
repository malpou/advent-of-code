using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day04 : Day
{
    private static readonly (int dirX, int dirY)[] _directions =
    {
        (-1, -1), (-1, 0), (-1, 1), // Up-left, Up, Up-right
        (0, -1), (0, 1), // Left, Right
        (1, -1), (1, 0), (1, 1) // Down-left, Down, Down-right
    };

    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var xmasCount = 0;
        var masCrossCount = 0;

        for (var row = 0; row < input.Length; row++)
        for (var col = 0; col < input[0].Length; col++)
        {
            if (input[row][col] == 'X')
            {
                xmasCount += _directions.Count(d => CheckWord(input, row, col, d.dirX, d.dirY));
            }

            if (input[row][col] == 'A')
            {
                masCrossCount += CheckMasCross(input, row, col) ? 1 : 0;
            }
        }

        return (xmasCount.ToString(), masCrossCount.ToString());
    }

    private bool CheckMasCross(string[] grid, int startX, int startY)
    {
        var diagonal1 = GetDiagonalString(grid, startX - 1, startY - 1, startX + 1, startY + 1);
        var diagonal2 = GetDiagonalString(grid, startX - 1, startY + 1, startX + 1, startY - 1);

        string[] validMasPatterns = { "MAS", "SAM" };

        return validMasPatterns.Contains(diagonal1) && validMasPatterns.Contains(diagonal2);
    }

    private bool CheckWord(string[] grid, int startX, int startY, int directionX, int directionY)
    {
        var target = "XMAS";

        for (var i = 0; i < target.Length; i++)
        {
            var (x, y) = (startX + (i * directionX), startY + (i * directionY));
            if (!IsValidPosition(x, y, grid.Length, grid[0].Length) || grid[x][y] != target[i])
            {
                return false; // Word is out of bounds or not mathing target
            }
        }

        return true;
    }

    private string GetDiagonalString(string[] grid, int startX, int startY, int endX, int endY)
    {
        if (!IsValidPosition(startX, startY, grid.Length, grid[0].Length) ||
            !IsValidPosition(endX, endY, grid.Length, grid[0].Length))
        {
            return ""; // Start or end out of bounds
        }

        var diagonalLengthX = endX - startX;
        var diagonalLengthY = endY - startY;

        if (Math.Abs(diagonalLengthX) != Math.Abs(diagonalLengthY))
        {
            return ""; // Not same diagonal
        }

        var stepX = Math.Sign(diagonalLengthX);
        var stepY = Math.Sign(diagonalLengthY);
        var steps = Math.Abs(diagonalLengthX) + 1;

        var result = new StringBuilder(steps);

        for (var i = 0; i < steps; i++)
        {
            var (x, y) = (startX + (i * stepX), startY + (i * stepY));

            if (!IsValidPosition(x, y, grid.Length, grid[0].Length))
            {
                return ""; // Path is out of bounds
            }

            result.Append(grid[x][y]);
        }

        return result.ToString();
    }

    private bool IsValidPosition(int x, int y, int rows, int cols) => x >= 0 && x < rows && y >= 0 && y < cols;
}