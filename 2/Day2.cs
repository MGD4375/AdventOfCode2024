namespace AdventOfCode2024._2;

public static class Day2
{
    public static void Run()
    {
        var reports = File.ReadAllText("./2/day-two-input.txt")
            .Split('\n')
            .Where(it => it.Length > 0)
            .Select(row => row.Split(" ").Select(int.Parse));

        //  Part 1
        {
            var pairs = reports.Select(ToPairs);
            var part1 = pairs.Count(PairsAreSafe);

            Console.WriteLine(part1);
        }

        //  Get pairs
        var part2 = reports.Count(report => GetAllPossibleDampenedPairs(report).Any(PairsAreSafe));

        Console.WriteLine(part2);
    }

    private static IEnumerable<IEnumerable<(int first, int second)>> GetAllPossibleDampenedPairs(
        IEnumerable<int> report)
    {
        var possibleReportsAfterDampening = new List<List<int>>();
        for (var i = 0; i < report.Count(); i++)
        {
            var newList = new List<int>(report);
            newList.RemoveAt(i);
            possibleReportsAfterDampening.Add(newList);
        }

        var pairs = possibleReportsAfterDampening.Select(ToPairs);
        return pairs;
    }

    private static IEnumerable<(int first, int second)> ToPairs(IEnumerable<int> report) =>
        report.Skip(1).Zip(report, (second, first) => Pair(first, second));

    private static (int first, int second) Pair(int first, int second) => (first, second);

    private static bool PairsAreSafe(IEnumerable<(int first, int second)> report) =>
        ReportIsSafeAscending(report) || ReportIsSafeDescending(report);

    private static bool ReportIsSafeDescending(IEnumerable<(int first, int second)> report) =>
        report.All(pair => pair.first - pair.second > -4 && pair.first - pair.second < 0);

    private static bool ReportIsSafeAscending(IEnumerable<(int first, int second)> report) =>
        report.All(pair => pair.first - pair.second < 4 && pair.first - pair.second > 0);
}