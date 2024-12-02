using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day02 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var reports = input.Select(l => l.Split(" ").Select(int.Parse).ToList()).ToList();

        var safeReports = reports.Count(IsSafe);
        var safeReportsWithDampener = reports.Count(IsSafeWithDampener);

        return (safeReports.ToString(), safeReportsWithDampener.ToString());
    }

    private static bool IsSafe(List<int> report)
    {
        var pairs = report.Zip(report.Skip(1));
        var differences = pairs.Select(p => p.First - p.Second).ToList();

        return (differences.All(d => d > 0) || differences.All(d => d < 0))
               && differences.All(d => d is >= 1 and <= 3 or >= -3 and <= -1);
    }

    private static bool IsSafeWithDampener(List<int> report)
    {
        return Enumerable.Range(0, report.Count)
            .Select(i => report.Where((_, index) => index != i).ToList())
            .Any(IsSafe);
    }
}