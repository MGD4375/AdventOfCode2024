namespace AdventOfCode2024._10;

public static class Day10
{
    public static void Run()
    {
        var input = File.ReadAllText("./10/input.txt");
        var data = input.Split("\r\n")
            .Select(r => r
                .ToCharArray()
                .Select(c => int.Parse($"{c.ToString()}"))
            );

        var map = new Map(data);
        Print(map);

        // Part 1
        {
            var trailHeads = map.Cells.IndicesOfMatches(0);
            var paths = trailHeads.Select(trailHead => map.Cells.FindPaths(trailHead));
            var trailHeadsThatPassPeak = paths.SelectMany(path => path.Where(c => c.Value == 9));
            Console.WriteLine($"Part 1: {trailHeadsThatPassPeak.Count()}");
        }

        // Part 2
        {
            Console.WriteLine($"Part 2:");
        }
    }


    private static void Print(Map map)
    {
        for (var yIndex = 0; yIndex < map.Cells.GetLength(0); yIndex++)
        {
            for (var xIndex = 0; xIndex < map.Cells.GetLength(1); xIndex++)
            {
                Console.Write(map.Cells[yIndex, xIndex]);
            }

            Console.WriteLine();
        }
    }
}

public class Map
{
    public Map(IEnumerable<IEnumerable<int>> data)
    {
        Cells = data.Select(row => row.ToArray()).ToArray().To2DArray();
    }

    public int[,] Cells { get; private set; }
}

// Extension method to convert jagged array to 2D array
public static class Extensions
{
    private static readonly List<(int X, int Y)> RelativeNeighbourPositions =
    [
        (-1, +0),
        (+1, +0),
        (+0, -1),
        (+0, +1)
    ];

    public static IEnumerable<(int X, int Y)> IndicesOfMatches<T>(this T[,] map, T value)
    {
        for (var x = 0; x < map.GetLength(0); ++x)
        for (var y = 0; y < map.GetLength(1); ++y)
            if (map[x, y]!.Equals(value))
                yield return (x, y);
    }

    public static List<((int X, int Y), int Value)> FindPaths(this int[,] map, (int X, int Y) trailHead)
    {
        var cellQueue = new Queue<(int X, int Y)>();
        cellQueue.Enqueue(trailHead);
        var visited = new List<((int X, int Y), int Value)>();
        while (cellQueue.Count != 0)
        {
            var (x, y) = cellQueue.Dequeue();
            var currentCellValue = map[x, y];

            var nextCells = RelativeNeighbourPositions
                .Select(pos => (X: pos.X + x, Y: pos.Y + y))
                .Where(pos => pos.X >= 0 && pos.X < map.GetLength(1) && pos.Y >= 0 && pos.Y < map.GetLength(0))
                .Select(pos => (Pos: pos, Cell: map[pos.X, pos.Y]))
                .Where(it => it.Cell == currentCellValue + 1)
                .Where(it => !visited.Contains(it))
                .ToList();

            visited.AddRange(nextCells);
            nextCells.Select(it => it.Pos).ToList().ForEach(it => cellQueue.Enqueue((it.X, it.Y)));
        }

        return visited;
    }

    public static T[,] To2DArray<T>(this T[][] source)
    {
        var result = new T[source.Length, source[0].Length];
        for (var i = 0; i < source.Length; i++)
        for (var j = 0; j < source[i].Length; j++)
            result[i, j] = source[i][j];
        return result;
    }
}

// public static class MapExtensions
// {

// }