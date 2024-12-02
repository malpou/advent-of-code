using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day07 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var part1 = CalculateWinnings(input, false);
        var part2 = CalculateWinnings(input, true);
        return (part1.ToString(), part2.ToString());
    }

    private static int CalculateWinnings(string[] input, bool useJokers)
    {
        var handGroups = new List<(int[] Score, int Bid)>[7];
        for (var i = 0; i < 7; i++)
        {
            handGroups[i] = [];
        }

        foreach (var line in input)
        {
            var parts = line.Split(" ");
            var cards = parts[0];
            var bid = int.Parse(parts[1]);
            var type = useJokers ? GetHandTypeWithJokers(cards) : GetHandType(cards);
            var score = GetHandScore(cards, useJokers);

            handGroups[type].Add((score, bid));
        }

        var sum = 0;
        var rank = 1;

        for (var type = 0; type < 7; type++)
        {
            handGroups[type].Sort((a, b) => Compare(a.Score, b.Score));
            foreach (var hand in handGroups[type])
            {
                sum += rank * hand.Bid;
                rank++;
            }
        }

        return sum;
    }

    private static int[] GetHandScore(string cards, bool useJokers) =>
        cards.Select(card => card switch
        {
            'J' => useJokers ? 1 : 10,
            '2' => 2,
            '3' => 3,
            '4' => 4,
            '5' => 5,
            '6' => 6,
            '7' => 7,
            '8' => 8,
            '9' => 9,
            'T' => 10,
            'Q' => 11,
            'K' => 12,
            'A' => 13,
            _ => throw new ArgumentOutOfRangeException(nameof(card), card, null)
        }).ToArray();

    private static int GetHandTypeWithJokers(string cards)
    {
        if (!cards.Contains('J'))
        {
            return GetHandType(cards);
        }

        var nonJokers = cards.Where(c => c != 'J').GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .ToList();

        var jokerCount = cards.Count(c => c == 'J');

        if (jokerCount is 5 or 4)
        {
            return 6; // Five of a kind
        }

        var firstGroupCount = nonJokers.Any() ? nonJokers.First().Count() : 0;
        return (firstGroupCount + jokerCount) switch
        {
            5 => 6, // Five of a kind
            4 => 5, // Four of a kind
            3 => nonJokers.Skip(1).Any() && nonJokers[1].Count() == 2 ? 4 : 3, // Full house or Three of a kind
            2 => nonJokers.Skip(1).Any() && nonJokers[1].Count() == 2 ? 2 : 1, // Two pair or One pair
            _ => 0 // High card
        };
    }

    private static int GetHandType(string cards)
    {
        var groups = cards.GroupBy(c => c)
            .Select(g => g.Count())
            .OrderByDescending(x => x)
            .ToList();

        return groups[0] switch
        {
            5 => 6, // Five of a kind
            4 => 5, // Four of a kind
            3 => groups[1] == 2 ? 4 : 3, // Full house or Three of a kind
            2 => groups[1] == 2 ? 2 : 1, // Two pair or One pair
            _ => 0 // High card
        };
    }

    private static int Compare(int[]? x, int[]? y)
    {
        if (x == null || y == null)
        {
            return 0;
        }

        return x.Zip(y)
            .Select(pair => pair.First.CompareTo(pair.Second))
            .FirstOrDefault(c => c != 0, 0);
    }
}