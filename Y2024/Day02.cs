using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day02 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var reports = input.Select(l => l.Split(" ").Select(int.Parse).ToList());

        var safeReports = 0;
        var safeReportsWithDampener = 0;
        foreach (var report in reports)
        {
            var differences = GetDifferences(report);

            if (IsConsistent(differences) && differences.All(IsValidDifference))
            {
                safeReports++;
            }
            else if (IsSafeWithDampener(report))
            {
                safeReportsWithDampener++;
            }
        }

        return (safeReports.ToString(), (safeReports + safeReportsWithDampener).ToString());
    }

    private static List<int> GetDifferences(List<int> numbers)
    {
        var differences = new List<int>();
        for (var i = 0; i < numbers.Count - 1; i++)
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

    private static bool IsSafeWithDampener(List<int> report)
    {
        return report.Select((_, i) => report.Where((_, index) => index != i).ToList())
            .Select(GetDifferences)
            .Any(differences => IsConsistent(differences) && differences.All(IsValidDifference));
    }
}