namespace AdventOfCode.Y2022;

public class Day02 : Day
{
  private const int DrawScore = 3;
  private const int WinScore = 6;

  private enum Options
  {
    Rock = 1,
    Paper = 2,
    Scissors = 3
  }

  private readonly Dictionary<char, Options> _optionsMap = new()
  {
    { 'A', Options.Rock },
    { 'B', Options.Paper },
    { 'C', Options.Scissors },
    { 'X', Options.Rock },
    { 'Y', Options.Paper },
    { 'Z', Options.Scissors }
  };

  public override (int part1, int? part2) Solve(string[] input)
  {
    var totalScore = 0;
    foreach (var s in input)
    {
      var opponentPick = _optionsMap[s[0]];
      var myPick = _optionsMap[s[2]];

      totalScore += (int)myPick;
      
      if (opponentPick == myPick)
      {
        totalScore += DrawScore;
        continue;
      }

      if ((myPick == Options.Rock && opponentPick == Options.Scissors) ||
          (myPick == Options.Paper && opponentPick == Options.Rock) ||
          (myPick == Options.Scissors && opponentPick == Options.Paper))
      {
        totalScore += WinScore;
      }
    }

    return (totalScore, null);
  }
}