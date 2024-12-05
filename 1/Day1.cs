namespace AdventOfCode2024._1;

public static class Day1
{
    public static void Run()
    {
        // shared
        var input = File.ReadAllText("./1/day-one-input.txt").Split('\n').Where(it => it.Length > 0);
        var left = input.Select(row => int.Parse(row.Split("   ")[0])).Order();
        var right = input.Select(row => int.Parse(row.Split("   ")[1])).Order();

        // part 1
        {
            var part1 = left.Zip(right)
                .Select(row => Math.Abs(row.First - row.Second))
                .Sum();

            Console.WriteLine("Part 1: " + part1);
        }

        // Part 2
        {
            var part2 = left.Select(l => right.Count(r => r == l) * l).Sum();
            Console.WriteLine("Part 2: " + part2);
        }
    }
}