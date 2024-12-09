namespace AdventOfCode2024._7;

public static class Day9
{
    private const char Empty = (char)65535;

    public static void Run()
    {
        var input = File.ReadAllText("./9/input.txt");

        // Part 1
        {
            var view = input.SelectMany((ch, index) =>
                {
                    var number = int.Parse(ch.ToString());
                    return index % 2 == 1
                        ? Enumerable.Range(1, number).Select(_ => Empty)
                        : Enumerable.Range(1, number).Select(_ => (char)(index / 2));
                })
                .ToList();

            var leftIndex = 0;
            var rightIndex = view.Count - 1;
            while (leftIndex < rightIndex)
            {
                var rightItem = view[rightIndex];
                while (rightItem == Empty && leftIndex < rightIndex)
                {
                    rightIndex--;
                    rightItem = view[rightIndex];
                }

                var leftItem = view[leftIndex];
                while (leftItem != Empty && leftIndex < rightIndex)
                {
                    leftIndex++;
                    leftItem = view[leftIndex];
                }

                (view[leftIndex], view[rightIndex]) = (view[rightIndex], view[leftIndex]);
            }

            var output = view
                .Where(ch => ch != Empty)
                .Select((ch, index) => (long)ch * index)
                .Sum();

            Console.WriteLine($"Part 1: {output}");
        }

        // Part 2
        {
            Console.WriteLine($"Part 2:");
        }
    }
}