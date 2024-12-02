using AdventOfCode.Core;

namespace AdventOfCode.Y2022;

public class Day03 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var part1 = input.Select(s => s[..(s.Length / 2)]
                .Intersect(s[(s.Length / 2)..])
                .Sum(GetPriority))
            .Sum();

        var part2 = input
            .Chunk(3)
            .Select(group => group[0]
                .Intersect(group[1])
                .Intersect(group[2])
                .Sum(GetPriority))
            .Sum();

        return (part1.ToString(), part2.ToString());
    }

    private static int GetPriority(char c) => c - (char.IsLower(c) ? 96 : 38);
}