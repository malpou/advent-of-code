using System.Text;
using AdventOfCode.Core;

namespace AdventOfCode.Y2015;

public class Day10 : Day
{
    private const int Processes = 50;

    public override (string part1, string part2) Solve(string[] _, string input)
    {
        var part1 = -1;
        for (var i = 0; i < Processes; i++)
        {
            if (i == 40)
            {
                part1 = input.Length;
            }

            input = ProcessInput(input);
        }

        return (part1.ToString(), input.Length.ToString());
    }

    private static string ProcessInput(string input)
    {
        var output = new StringBuilder();
        var currentDigit = input[0];
        var count = 1;

        for (var i = 1; i < input.Length; i++)
        {
            var digit = input[i];
            if (digit == currentDigit)
            {
                count++;
            }
            else
            {
                output.Append(count).Append(currentDigit);
                currentDigit = digit;
                count = 1;
            }
        }

        output.Append(count).Append(currentDigit);
        return output.ToString();
    }
}