using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day08 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var nodes = input.Skip(2)
            .Select(line => line.Split(" "))
            .ToDictionary(
                parts => parts[0],
                parts => (
                    Left: parts[2].TrimStart('(').TrimEnd(','),
                    Right: parts[3].TrimEnd(')')
                ));

        var part1Steps = CountStepsToDestination(
            input[0],
            "AAA",
            node => node == "ZZZ",
            nodes);

        var startingNodes = nodes.Keys.Where(node => node.EndsWith('A')).ToList();
        var individualCycleLengths = startingNodes.Select(startNode =>
            CountStepsToDestination(
                input[0],
                startNode,
                node => node.EndsWith('Z'),
                nodes)
        );

        var stepsUntilSync = individualCycleLengths.Aggregate(LeastCommonMultiple);

        return (part1Steps.ToString(), stepsUntilSync.ToString());
    }

    private static long CountStepsToDestination(
        string instructions,
        string startNode,
        Func<string, bool> isDestination,
        Dictionary<string, (string Left, string Right)> nodes)
    {
        var buffer = new InfiniteStringBuffer(instructions);
        var currentNode = startNode;

        while (!isDestination(currentNode))
        {
            var instruction = buffer.GetNextChar();
            currentNode = instruction == 'L' ? nodes[currentNode].Left : nodes[currentNode].Right;
        }

        return buffer.TotalMoves;
    }

    private static long LeastCommonMultiple(long a, long b)
    {
        return Math.Abs(a * b) / GreatestCommonDivisor(a, b);
    }

    private static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            var temp = b;
            b = a % b;
            a = temp;
        }

        return a;
    }

    private class InfiniteStringBuffer(string source)
    {
        private int _currentPosition;

        public long TotalMoves { get; private set; }

        public char GetNextChar()
        {
            var currentChar = source[_currentPosition];
            _currentPosition = (_currentPosition + 1) % source.Length;
            TotalMoves++;
            return currentChar;
        }
    }
}