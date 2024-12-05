using System.Text.RegularExpressions;

namespace AdventOfCode2024._5;

public static class Day5
{
    public static void Run()
    {
        var input = File.ReadAllText("./5/input.txt");

        var pageOrderingRules = Regex
            .Matches(input, @"[0-9]*\|[0-9]*")
            .Select(match => match.Value.Split('|'))
            .Select(it => (int.Parse(it[0]), int.Parse(it[1])))
            .ToList();

        var pageLists = Regex
            .Matches(input, @".*\,.*")
            .Select(it => it.Value
                .Split(',')
                .Select(int.Parse)
                .ToList())
            .ToList();

        var orderLookup = GenerateOrderLookup(pageOrderingRules);

        //part 1
        {
            var part1Answer = pageLists
                .Where(pl => IsValid(pl, pageOrderingRules))
                .Select(pl => pl[pl.Count / 2])
                .Sum();

            Console.WriteLine("Part 1: " + part1Answer);
        }

        //part 2 
        {
            var incorrectlyOrderedLists = pageLists
                .Where(pl => !IsValid(pl, pageOrderingRules));
        }
    }

    private static Dictionary<int, int> GenerateOrderLookup(
        List<(int, int)> pageOrderingRules
    )
    {
        var possiblePages = pageOrderingRules
            .Select(r => r.Item1)
            .Concat(pageOrderingRules.Select(r => r.Item2))
            .Distinct()
            .ToList();

        var orderedList = new List<int>();
        foreach (var item in possiblePages)
        {
            for (var orderedListIndex = 0; orderedListIndex < orderedList.Count + 1; orderedListIndex++)
            {
                var newList = new List<int>(orderedList);
                newList.Insert(orderedListIndex, item);
                var isValid = IsValid(newList, pageOrderingRules);

                if (!isValid)
                {
                    continue;
                }


                orderedList = newList;
                break;
            }
        }

        return orderedList
            .Select((item, index) => new { Item = item, Index = index })
            .ToDictionary(it => it.Item, it => it.Index);
    }

    private static bool IsValid(List<int> list, IEnumerable<(int, int)> pageOrderingRules)
    {
        return pageOrderingRules
            .Where(r => list.Contains(r.Item1) && list.Contains(r.Item2))
            .All(rule => list.IndexOf(rule.Item1) <= list.IndexOf(rule.Item2));
    }
}