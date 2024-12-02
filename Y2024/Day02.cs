using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day02 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var reports = input.Select(l => l.Split(" ").Select(int.Parse).ToArray()).ToArray();

        var safeReports1 = 0;
        var safeReports2 = 0;
        foreach (var report in reports)
        {
            var differences = GetDifferences(report);

            if (IsConsistent(differences) && differences.All(IsValidDifference))
            {
                safeReports1++;
                safeReports2++;
            }
            else if (IsSafeWithDampener(report))
            {
                safeReports2++;
            }
        }

        return (safeReports1.ToString(), safeReports2.ToString());
    }

    private static List<int> GetDifferences(int[] numbers)
    {
        var differences = new List<int>();
        for (var i = 0; i < numbers.Length - 1; i++)
        {
            differences.Add(numbers[i] - numbers[i + 1]);
        }

        return differences;
    }

    private static bool IsValidDifference(int d)
    {
        return d is >= 1 and <= 3 or >= -3 and <= -1;
    }

    private static bool IsConsistent(List<int> differences)
    {
        return differences.All(d => d > 0) || differences.All(d => d < 0);
    }

    private static bool IsSafeWithDampener(int[] report)
    {
        return report.Select((_, i) => report.Where((_, index) => index != i).ToArray())
            .Select(GetDifferences)
            .Any(differences => IsConsistent(differences) && differences.All(IsValidDifference));
    }
}