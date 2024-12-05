namespace AdventOfCode2024._4;

public static class Day4
{
    public static void Run()
    {
        var input = File.ReadAllText("./4/input.txt");
        var grid = input
            .Split("\n")
            .Select(r => r
                .ToCharArray()
                .Where(c => c != '\r' && c != '\n')
                .ToList())
            .Where(r => r.Count > 0)
            .ToList();


        //part 1
        {
            var relativePositions = new List<List<(int X, int Y)>>()
            {
                new() { (X: 0, Y: 0), (X: +1, Y: -1), (X: +2, Y: -2), (X: +3, Y: -3) },
                new() { (X: 0, Y: 0), (X: +1, Y: +0), (X: +2, Y: +0), (X: +3, Y: +0) },
                new() { (X: 0, Y: 0), (X: +1, Y: +1), (X: +2, Y: +2), (X: +3, Y: +3) },

                new() { (X: 0, Y: 0), (X: +0, Y: -1), (X: +0, Y: -2), (X: +0, Y: -3) },
                new() { (X: 0, Y: 0), (X: +0, Y: +1), (X: +0, Y: +2), (X: +0, Y: +3) },

                new() { (X: 0, Y: 0), (X: -1, Y: -1), (X: -2, Y: -2), (X: -3, Y: -3) },
                new() { (X: 0, Y: 0), (X: -1, Y: +0), (X: -2, Y: +0), (X: -3, Y: +0) },
                new() { (X: 0, Y: 0), (X: -1, Y: +1), (X: -2, Y: +2), (X: -3, Y: +3) },
            };

            var total = 0;
            for (var colIndex = 0; colIndex < grid.Count; colIndex++)
            {
                for (var rowIndex = 0; rowIndex < grid[0].Count; rowIndex++)
                {
                    var x = colIndex;
                    var y = rowIndex;

                    var axesToCheck = relativePositions.Select(axis =>
                        axis.Select(rp => (rp.Item1 + x, rp.Item2 + y))
                    );
                    var axesInBounds = axesToCheck.Where(axis => axis.All(ap => IsWithinBounds(ap, grid)));
                    var words = axesInBounds.Select(
                        axis => axis.Aggregate("", (t, ap) => t + grid[ap.Item1][ap.Item2]));
                    var count = words.Count(output => output == "XMAS");

                    total += count;
                }
            }

            Console.WriteLine("Part 1: " + total);
        }

        //part 2 
        {
            var relativePositions = new List<List<(int X, int Y)>>()
            {
                new() { (X: -1, Y: -1), (X: +0, Y: +0), (X: +1, Y: +1) },
                new() { (X: +1, Y: +1), (X: +0, Y: +0), (X: -1, Y: -1) },

                new() { (X: -1, Y: +1), (X: +0, Y: +0), (X: +1, Y: -1) },
                new() { (X: +1, Y: -1), (X: +0, Y: +0), (X: -1, Y: +1) },
            };

            var total = 0;
            for (var colIndex = 0; colIndex < grid.Count; colIndex++)
            {
                for (var rowIndex = 0; rowIndex < grid[0].Count; rowIndex++)
                {
                    var x = colIndex;
                    var y = rowIndex;

                    var axesToCheck = relativePositions.Select(axis =>
                        axis.Select(rp => (rp.Item1 + x, rp.Item2 + y))
                    );
                    var axesInBounds = axesToCheck.Where(axis => axis.All(ap => IsWithinBounds(ap, grid)));
                    var words = axesInBounds.Select(
                        axis => axis.Aggregate("", (t, ap) => t + grid[ap.Item1][ap.Item2]));
                    var count = words.Count(output => output == "MAS") == 2 ? 1 : 0;

                    total += count;
                }
            }

            Console.WriteLine("Part 2: " + total);
        }
    }

    private static bool IsWithinBounds((int, int) ap, List<List<char>> grid)
    {
        return ap.Item1 >= 0 && ap.Item1 < grid[0].Count
                             && ap.Item2 >= 0 && ap.Item2 < grid.Count;
    }
}