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
            var p2Answer = pageLists
                .Where(pl => !IsValid(pl, pageOrderingRules))
                .Select((it, _) => CorrectList(it, pageOrderingRules))
                .Select(pl => pl[pl.Count / 2])
                .Sum();

            Console.WriteLine("Part 2: " + p2Answer);
        }
    }

    private static List<int> CorrectList(List<int> pages, List<(int, int)> pageOrderingRules)
    {
        var workingList = new List<int>(pages);
        var failingRule = GetFailingRule(pageOrderingRules, workingList);
        while (failingRule != (0, 0))
        {
            Console.WriteLine($"Attempting to fix ({failingRule.Item1}|{failingRule.Item2})");
            var indexA = workingList.IndexOf(failingRule.Item1);
            var indexB = workingList.IndexOf(failingRule.Item2);
            (workingList[indexA], workingList[indexB]) = (workingList[indexB], workingList[indexA]);
            failingRule = GetFailingRule(pageOrderingRules, workingList);
        }

        return workingList;
    }

    private static (int, int) GetFailingRule(List<(int, int)> pageOrderingRules, List<int> workingList)
    {
        return pageOrderingRules
            .Where(r => workingList.Contains(r.Item1) && workingList.Contains(r.Item2))
            .FirstOrDefault(r => workingList.IndexOf(r.Item1) > workingList.IndexOf(r.Item2));
    }


    private static bool IsValid(List<int> list, IEnumerable<(int, int)> pageOrderingRules)
    {
        return pageOrderingRules
            .Where(r => list.Contains(r.Item1) && list.Contains(r.Item2))
            .All(rule => list.IndexOf(rule.Item1) < list.IndexOf(rule.Item2));
    }
}