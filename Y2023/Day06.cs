using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day06 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var times = ExtractNumbersFromString(input[0]);
        var distances = ExtractNumbersFromString(input[1]);

        long part1 = 1;
        for (var i = 0; i < times.Count; i++)
        {
            part1 *= NumberOfWinningHoldTimes(times[i], distances[i]);
        }

        var singleTime = ConcatenateNumbers(input[0]);
        var singleDistance = ConcatenateNumbers(input[1]);
        var part2 = NumberOfWinningHoldTimes(singleTime, singleDistance);

        return (part1.ToString(), part2.ToString());
    }

    private static long NumberOfWinningHoldTimes(long raceTime, long distanceRecord)
    {
        long wins = 0;
        for (long holdTime = 0; holdTime <= raceTime; holdTime++)
        {
            var distance = holdTime * (raceTime - holdTime);
            if (distance > distanceRecord)
            {
                wins++;
            }
        }

        return wins;
    }

    private static long ConcatenateNumbers(string input)
    {
        var numbers = new StringBuilder();
        string[] parts = input.Split(' ');

        foreach (var part in parts)
        {
            if (int.TryParse(part, out _))
            {
                numbers.Append(part);
            }
        }

        return long.Parse(numbers.ToString());
    }

    private static List<int> ExtractNumbersFromString(string input)
    {
        var numbers = new List<int>();
        string[] parts = input.Split(' ');

        foreach (var part in parts)
        {
            if (int.TryParse(part, out var number))
            {
                numbers.Add(number);
            }
        }

        return numbers;
    }
}