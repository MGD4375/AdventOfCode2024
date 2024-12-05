using System.Text.RegularExpressions;

namespace AdventOfCode2024._3;

public static class Day3
{
    public static void Run()
    {
        var input = File.ReadAllText("./3/day-three-input.txt");
        //part 1
        {
            var mul = Regex.Matches(input, @"mul\([0-9]*,[0-9]*\)");
            var pairs = mul.Select(ToNumPair);
            var answer = pairs.Select(pair => pair.Item1 * pair.Item2).Sum();
            Console.WriteLine(answer);
        }

        //part 2 
        {
            var matches = Regex.Matches(input, @"don't\(\)|do\(\)|mul\([0-9]*,[0-9]*\)");
            var commandList = new List<(int, int)>();
            var isCurrentlyDo = true;
            foreach (Match match in matches)
            {
                if (match.Value == "do()")
                {
                    isCurrentlyDo = true;
                    continue;
                }

                if (match.Value == "don't()")
                {
                    isCurrentlyDo = false;
                    continue;
                }

                if (isCurrentlyDo)
                {
                    commandList.Add(ToNumPair(match));
                }
            }

            var answer = commandList.Sum(c => c.Item1 * c.Item2);
            Console.WriteLine(answer);
        }
    }

    private static (int, int) ToNumPair(Match row)
    {
        var spl = row.Value.Split('(', ',', ')');
        return (int.Parse(spl[1]), int.Parse(spl[2]));
    }
}