using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day08 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var buffer = new InfiniteStringBuffer(input[0]);

        var nodes = input.Skip(2).ToDictionary(
            line => line.Split(" ")[0],
            line =>
            {
                var parts = line.Split(" ");
                return (
                    Left: parts[2].TrimStart('(').TrimEnd(','),
                    Right: parts[3].TrimEnd(')')
                );
            }
        );

        var currentNode = "AAA";
        while (currentNode != "ZZZ")
        {
            var node = nodes[currentNode];
            currentNode = buffer.GetNextChar() == 'L' ? node.Left : node.Right;
        }

        return (buffer.TotalMoves.ToString(), "");
    }

    private class InfiniteStringBuffer(string source)
    {
        private int _currentPosition;

        public int TotalMoves { get; private set; }

        public char GetNextChar()
        {
            var currentChar = source[_currentPosition];
            _currentPosition = (_currentPosition + 1) % source.Length;
            TotalMoves++;
            return currentChar;
        }
    }
}