using System.Text.RegularExpressions;
using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day03 : Day
{
    public override (string part1, string part2) Solve(string[] _, string input)
    {
        var pattern = @"(do\(\)|don't\(\)|mul\((\d+),(\d+)\))";

        var isEnabled = true;
        var part1 = 0;
        var part2 = 0;
        foreach (Match match in Regex.Matches(input, pattern))
        {
            var instruction = match.Groups[0].Value;

            if (instruction == "do()")
            {
                isEnabled = true;
            }
            else if (instruction == "don't()")
            {
                isEnabled = false;
            }
            else if (instruction.StartsWith("mul"))
            {
                var num1 = int.Parse(match.Groups[2].Value);
                var num2 = int.Parse(match.Groups[3].Value);

                var value = num1 * num2;
                part1 += value;

                if (isEnabled)
                {
                    part2 += value;
                }
            }
        }

        return (part1.ToString(), part2.ToString());
    }
}