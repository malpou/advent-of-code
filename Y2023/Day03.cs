using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day03 : Day
{
    public override (string part1, string part2) Solve(string[] inputLines, string inputText)
    {
        var height = inputLines.Length;
        var width = inputLines[0].Length;

        var numbers = new List<(int row, int startCol, int endCol, int value)>();
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                if (!char.IsDigit(inputLines[row][col]))
                {
                    continue;
                }

                var startCol = col;
                var num = "";
                while (col < width && char.IsDigit(inputLines[row][col]))
                {
                    num += inputLines[row][col];
                    col++;
                }

                numbers.Add((row, startCol, col - 1, int.Parse(num)));
            }
        }

        long part1Sum = 0;
        foreach (var (row, startCol, endCol, value) in numbers)
        {
            var hasAdjacentSymbol = false;
            for (var r = Math.Max(0, row - 1); r <= Math.Min(height - 1, row + 1); r++)
            {
                for (var c = Math.Max(0, startCol - 1); c <= Math.Min(width - 1, endCol + 1); c++)
                {
                    if (inputLines[r][c] == '.' || char.IsDigit(inputLines[r][c]))
                    {
                        continue;
                    }

                    hasAdjacentSymbol = true;
                    break;
                }

                if (hasAdjacentSymbol)
                {
                    break;
                }
            }

            if (hasAdjacentSymbol)
            {
                part1Sum += value;
            }
        }

        long part2Sum = 0;
        for (var row = 0; row < height; row++)
        {
            for (var col = 0; col < width; col++)
            {
                if (inputLines[row][col] != '*')
                {
                    continue;
                }

                var adjacentNumbers = numbers.Where(n =>
                    Math.Abs(n.row - row) <= 1 &&
                    n.startCol <= col + 1 &&
                    n.endCol >= col - 1
                ).ToList();

                if (adjacentNumbers.Count == 2)
                {
                    part2Sum += (long)adjacentNumbers[0].value * adjacentNumbers[1].value;
                }
            }
        }

        return (part1Sum.ToString(), part2Sum.ToString());
    }
}