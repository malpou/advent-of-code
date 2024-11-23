using AdventOfCode.Core;

namespace AdventOfCode.Y2023;

public class Day09 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var sequences = input.Select(line => line.Split(" ").Select(int.Parse));
        var extrapolatedValues = sequences.Select(ExtrapolateOuterValues).ToList();

        return (
            extrapolatedValues.Sum(v => v.Next).ToString(),
            extrapolatedValues.Sum(v => v.Previous).ToString()
        );
    }

    private static (int Previous, int Next) ExtrapolateOuterValues(IEnumerable<int> sequence)
    {
        var pyramid = new List<List<int>> { sequence.ToList() };

        while (pyramid.Last().Any(x => x != 0))
        {
            var currentSequence = pyramid.Last();
            var differences = Enumerable.Range(0, currentSequence.Count - 1)
                .Select(i => currentSequence[i + 1] - currentSequence[i])
                .ToList();
            pyramid.Add(differences);
        }

        pyramid[^1] = [0, .. pyramid.Last(), 0];
        for (var i = pyramid.Count - 2; i >= 0; i--)
        {
            var currentSeq = pyramid[i];
            var lowerSeq = pyramid[i + 1];

            var previousValue = currentSeq.First() - lowerSeq.First();
            var nextValue = currentSeq.Last() + lowerSeq.Last();

            pyramid[i] = [previousValue, .. currentSeq, nextValue];
        }

        var firstSequence = pyramid.First();
        return (firstSequence.First(), firstSequence.Last());
    }
}