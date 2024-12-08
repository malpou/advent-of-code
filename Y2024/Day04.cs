using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day04 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var grid = input.ToCharGrid();
        var (cols, rows) = (grid[0].Length, grid.Length);
        var xmasCount = 0;
        var masCrossCount = 0;

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            var p = new Point(x, y, cols, rows);

            if (grid[y][x] == 'X')
            {
                xmasCount += Vector.EightDirections.Count(d => grid.FindPattern(p, d, "XMAS".ToCharArray()));
            }

            if (grid[y][x] != 'A')
            {
                continue;
            }

            var diagonal1 = grid.GetDiagonalElements(p, Vector.UpLeft, Vector.DownRight);
            var diagonal2 = grid.GetDiagonalElements(p, Vector.UpRight, Vector.DownLeft);

            if (diagonal1.Length != 3 || diagonal2.Length != 3)
            {
                continue;
            }

            string[] validMasPatterns = ["MAS", "SAM"];
            if (validMasPatterns.Contains(new string(diagonal1)) &&
                validMasPatterns.Contains(new string(diagonal2)))
            {
                masCrossCount++;
            }
        }

        return (xmasCount.ToString(), masCrossCount.ToString());
    }
}