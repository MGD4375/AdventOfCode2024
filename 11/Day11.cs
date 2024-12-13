namespace AdventOfCode2024._11;

public static class Day11
{
    public static void Run()
    {
        var input = File.ReadAllText("./11/input.txt")
            .Split(' ')
            .Select(ulong.Parse)
            .ToList();


        // Part 1
        {
            var list = input;
            for (var iteration = 1; iteration <= 75; iteration++)
            {
                list = list.SelectMany((li, index) =>
                    {
                        if (li == 0)
                        {
                            return [1];
                        }

                        var length = li.ToString().Length;
                        if (length % 2 == 0)
                        {
                            var str = li.ToString();
                            var halfwayIndex = (length / 2);
                            var halfCount = (length / 2);
                            var a = str.Substring(0, halfCount);
                            var b = str.Substring(halfwayIndex, halfCount);
                            return
                            [
                                ulong.Parse(a),
                                ulong.Parse(b)
                            ];
                        }

                        return new List<ulong>() { li * 2024 };
                    })
                    .ToList();
                Console.WriteLine($"{iteration}, {list.Count}");
            }

            Console.WriteLine("Part 1: " + list.Count);
        }

        // Part 2
        {
            // To solve this, I think I'd need to look for repeating patterns, or work out the formula. It's basically an exponential series
        }
    }

    private static void Print(int iteration, List<ulong> list)
    {
        Console.Write($"Iteration {iteration}:");
        foreach (var item in list)
        {
            Console.Write($"{item}, ");
        }

        Console.WriteLine();
    }
}