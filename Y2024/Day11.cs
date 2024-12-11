using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day11 : Day
{
    public override (string part1, string part2) Solve(string[] _, string input)
    {
        var stones = input.Split(" ").Select(long.Parse)
            .GroupBy(x => x)
            .ToDictionary(g => g.Key, g => (long)g.Count());

        var part1 = 0L;

        for (var i = 1; i <= 75; i++)
        {
            var next = new Dictionary<long, long>();

            foreach (var (stone, count) in stones)
            {
                if (stone == 0)
                {
                    AddCount(next, 1, count);
                    continue;
                }

                var str = stone.ToString();
                if (str.Length % 2 == 0)
                {
                    var mid = str.Length / 2;
                    AddCount(next, long.Parse(str[..mid]), count);
                    AddCount(next, long.Parse(str[mid..]), count);
                }
                else
                {
                    AddCount(next, stone * 2024, count);
                }
            }

            if (i == 25)
            {
                part1 = stones.Values.Sum();
            }

            stones = next;
        }

        return (part1.ToString(), stones.Values.Sum().ToString());
    }

    private static void AddCount(Dictionary<long, long> dict, long key, long count)
    {
        if (dict.TryGetValue(key, out var existing))
        {
            dict[key] = existing + count;
        }
        else
        {
            dict[key] = count;
        }
    }
}