using AdventOfCode.Core;

namespace AdventOfCode.Y2024;

public class Day05 : Day
{
    public override (string part1, string part2) Solve(string[] input, string _)
    {
        var graph = new PageDependencyGraph();
        input.Where(l => l.Contains('|'))
            .Select(l => l.Split('|'))
            .ToList()
            .ForEach(parts => graph.AddDependency(int.Parse(parts[0]), int.Parse(parts[1])));

        var pagesToProduce = input.Where(l => l.Contains(','))
            .Select(l => l.Split(',').Select(int.Parse).ToList())
            .ToList();

        var (valid, invalid) = graph.ValidateOrders(pagesToProduce);

        var part1 = valid.Sum(GetMiddleValue);
        var part2 = invalid
            .Select(order => graph.SortInvalidPages(order))
            .Sum(GetMiddleValue);

        return (part1.ToString(), part2.ToString());
    }

    private static int GetMiddleValue(List<int> list)
    {
        var middleIndex = list.Count / 2;
        return list.Count % 2 == 0
            ? (list[middleIndex - 1] + list[middleIndex]) / 2
            : list[middleIndex];
    }

    private class PageDependencyGraph
    {
        private readonly Dictionary<int, HashSet<int>> _dependencies = new();

        public void AddDependency(int from, int to)
        {
            if (!_dependencies.ContainsKey(from))
            {
                _dependencies[from] = new HashSet<int>();
            }

            if (!_dependencies.ContainsKey(to))
            {
                _dependencies[to] = new HashSet<int>();
            }

            _dependencies[from].Add(to);
        }

        public (List<List<int>> Valid, List<List<int>> Invalid) ValidateOrders(List<List<int>> orders)
        {
            var valid = orders.ToLookup(IsValidOrder);
            return (valid[true].ToList(), valid[false].ToList());
        }

        private bool IsValidOrder(List<int> order)
        {
            var positions = order
                .Select((page, index) => (page, index))
                .ToDictionary(x => x.page, x => x.index);

            return _dependencies.All(kvp =>
                !positions.ContainsKey(kvp.Key) ||
                kvp.Value.All(dep =>
                    !positions.ContainsKey(dep) ||
                    positions[kvp.Key] < positions[dep]
                )
            );
        }

        public List<int> SortInvalidPages(List<int> pages)
        {
            var result = new List<int>();
            var remaining = pages.ToHashSet();

            while (remaining.Count > 0)
            {
                var pageWithNoDeps = remaining.First(page =>
                    remaining.All(other =>
                        !_dependencies.ContainsKey(other) ||
                        !_dependencies[other].Contains(page)
                    )
                );

                result.Add(pageWithNoDeps);
                remaining.Remove(pageWithNoDeps);
            }

            return result;
        }
    }
}