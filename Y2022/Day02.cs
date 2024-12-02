using AdventOfCode.Core;

namespace AdventOfCode.Y2022;

public class Day02 : Day
{
    private readonly Dictionary<(Option opponent, Outcome desired), Option> _moveForOutcome = new()
    {
        { (Option.Rock, Outcome.Win), Option.Paper },
        { (Option.Rock, Outcome.Loss), Option.Scissors },
        { (Option.Rock, Outcome.Draw), Option.Rock },
        { (Option.Paper, Outcome.Win), Option.Scissors },
        { (Option.Paper, Outcome.Loss), Option.Rock },
        { (Option.Paper, Outcome.Draw), Option.Paper },
        { (Option.Scissors, Outcome.Win), Option.Rock },
        { (Option.Scissors, Outcome.Loss), Option.Paper },
        { (Option.Scissors, Outcome.Draw), Option.Scissors }
    };

    private readonly Dictionary<char, Option> _optionMap = new()
    {
        { 'A', Option.Rock },
        { 'B', Option.Paper },
        { 'C', Option.Scissors },
        { 'X', Option.Rock },
        { 'Y', Option.Paper },
        { 'Z', Option.Scissors }
    };

    private readonly Dictionary<char, Outcome> _outcomeMap = new()
    {
        { 'X', Outcome.Loss }, { 'Y', Outcome.Draw }, { 'Z', Outcome.Win }
    };

    private readonly Dictionary<(Option opponent, Option my), Outcome> _winningMoves = new()
    {
        { (Option.Rock, Option.Paper), Outcome.Win },
        { (Option.Rock, Option.Scissors), Outcome.Loss },
        { (Option.Paper, Option.Rock), Outcome.Loss },
        { (Option.Paper, Option.Scissors), Outcome.Win },
        { (Option.Scissors, Option.Rock), Outcome.Win },
        { (Option.Scissors, Option.Paper), Outcome.Loss }
    };

    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var part1 = 0;
        var part2 = 0;
        foreach (var s in input)
        {
            var opponentPick = _optionMap[s[0]];
            var myPick = _optionMap[s[2]];
            var desiredOutcome = _outcomeMap[s[2]];

            var outcome = DetermineOutcome(opponentPick, myPick);
            part1 += CalculateScore(myPick, outcome);

            var moveToMake = _moveForOutcome[(opponentPick, desiredOutcome)];
            part2 += CalculateScore(moveToMake, desiredOutcome);
        }

        return (part1.ToString(), part2.ToString());
    }

    private static int CalculateScore(Option myMove, Outcome outcome) => (int)myMove + (int)outcome;

    private Outcome DetermineOutcome(Option opponent, Option my) =>
        opponent == my ? Outcome.Draw : _winningMoves[(opponent, my)];

    private enum Outcome
    {
        Loss = 0,
        Draw = 3,
        Win = 6
    }

    private enum Option
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }
}