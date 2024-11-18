using AdventOfCode.Core;

namespace AdventOfCode.Y2022;

internal class Day06 : Day
{
    private readonly int[] _markerLengths = [4, 14];

    public override (string part1, string part2) Solve(string[] _, string input)
    {
        var results = _markerLengths
            .Select(length => FindMarkerPosition(input, length))
            .ToArray();

        return (results[0].ToString(), results[1].ToString());
    }

    private static int FindMarkerPosition(string input, int markerLength)
    {
        for (var i = 0; i <= input.Length - markerLength; i++)
        {
            if (input.Substring(i, markerLength).ToHashSet().Count == markerLength)
            {
                return i + markerLength;
            }
        }

        return -1;
    }
}