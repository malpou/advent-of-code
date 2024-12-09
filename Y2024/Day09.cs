using AdventOfCode.Core;
using AdventOfCode.Utils;

namespace AdventOfCode.Y2024;

public class Day09 : Day
{
    public override (string part1, string part2) Solve(string[] _, string input)
    {
        var numbers = input.Select(c => int.Parse(c.ToString())).ToList();

        var disk1 = CreateDiskMap(numbers);
        var disk2 = CreateDiskMap(numbers);

        var checksum1 = CalculateChecksum(CompactDisk(disk1.ToList()));
        var checksum2 = CalculateChecksum(CompactDiskWholeFiles(disk2.ToList()));

        return (checksum1.ToString(), checksum2.ToString());
    }

    private static List<int> CreateDiskMap(List<int> numbers) =>
        numbers
            .CreateAlternatingSequence(
                (num, i) => Enumerable.Repeat(i / 2, num),
                (num, _) => Enumerable.Repeat(-1, num))
            .ToList();

    private static List<int> CompactDisk(List<int> disk)
    {
        while (true)
        {
            var firstGap = disk.IndexOf(-1);
            if (firstGap == -1)
            {
                break;
            }

            var lastFilePos = disk.Count - 1;
            while (lastFilePos >= 0 && disk[lastFilePos] == -1)
            {
                lastFilePos--;
            }

            if (lastFilePos <= firstGap)
            {
                break;
            }

            disk[firstGap] = disk[lastFilePos];
            disk[lastFilePos] = -1;
        }

        return disk;
    }

    private static List<int> CompactDiskWholeFiles(List<int> disk)
    {
        var maxFileId = disk.Max();
        var workingDisk = disk.ToList();

        for (var fileId = maxFileId; fileId >= 0; fileId--)
        {
            var currentFileId = fileId;
            var fileIndices = workingDisk.FindIndices(id => id == currentFileId);
            if (fileIndices.Count == 0)
            {
                continue;
            }

            var fileSize = fileIndices.Count;
            var fileStart = fileIndices[0];
            var bestSpaceStart = workingDisk.FindConsecutiveSequence(id => id == -1, fileSize, fileStart);

            if (bestSpaceStart != -1)
            {
                MoveFile(workingDisk, currentFileId, fileIndices, bestSpaceStart);
            }
        }

        return workingDisk;
    }

    private static void MoveFile(List<int> disk, int fileId, List<int> fileIndices, int newStart)
    {
        foreach (var index in fileIndices)
        {
            disk[index] = -1;
        }

        for (var i = 0; i < fileIndices.Count; i++)
        {
            disk[newStart + i] = fileId;
        }
    }

    private static long CalculateChecksum(List<int> disk) =>
        disk.Select((fileId, pos) => fileId == -1 ? 0 : (long)pos * fileId).Sum();
}