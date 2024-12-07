using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day07 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var results = input.AsParallel().Select(l =>
        {
            var parts = l.Split(":", StringSplitOptions.TrimEntries);
            var result = long.Parse(parts.First());
            var values = parts.Last().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

            var part1Value = values.CanMakeResult(result, '+', '*') ? result : 0;
            var part2Value = values.CanMakeResult(result, '+', '*', '|') ? result : 0;

            return (part1: part1Value, part2: part2Value);
        }).ToList();

        var part1 = results.Sum(r => r.part1);
        var part2 = results.Sum(r => r.part2);

        return (part1.ToString(), part2.ToString());
    }
}

internal static class EquationExtensions
{
    public static bool CanMakeResult(this List<long> values, long target, params char[] ops)
    {
        if (values.Count < 2 || ops.Length == 0)
        {
            return false;
        }

        return TryOperators(new char[values.Count - 1], 0, values, target, ops);
    }

    private static bool TryOperators(char[] ops, int pos, List<long> values, long target, char[] allowed)
    {
        if (pos == ops.Length)
        {
            return Calculate(values, ops) == target;
        }

        foreach (var op in allowed)
        {
            ops[pos] = op;
            if (TryOperators(ops, pos + 1, values, target, allowed))
            {
                return true;
            }
        }

        return false;
    }

    private static long Calculate(List<long> values, char[] ops)
    {
        var result = values[0];

        for (var i = 0; i < ops.Length; i++)
        {
            result = ops[i] switch
            {
                '+' => result + values[i + 1],
                '*' => result * values[i + 1],
                '|' => long.Parse($"{result}{values[i + 1]}"),
                _ => throw new ArgumentException($"Invalid operator: {ops[i]}")
            };
        }

        return result;
    }
}