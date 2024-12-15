using System.Runtime.Intrinsics.X86;

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
            var trailHeads = map.Cells.IndicesOfMatches(0);
            var paths = trailHeads.Select(trailHead => map.Cells.FindPaths2(trailHead));
            var countOfUniquePaths = paths.Sum();
            Console.WriteLine($"Part 2: {countOfUniquePaths}");
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
        Cells = data.Select(row => row.ToArray()).ToArray().To2DArrayOld();
    }

    public int[,] Cells { get; private set; }
}

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

            var visitedCells = RelativeNeighbourPositions
                .Select(pos => (X: pos.X + x, Y: pos.Y + y))
                .Where(pos => pos.X >= 0 && pos.X < map.GetLength(1) && pos.Y >= 0 && pos.Y < map.GetLength(0))
                .Select(pos => (Pos: pos, Cell: map[pos.X, pos.Y]))
                .Where(it => it.Cell == currentCellValue + 1)
                .Where(it => !visited.Contains(it))
                .ToList();

            visited.AddRange(visitedCells);
            visitedCells.Select(it => it.Pos).ToList().ForEach(it => cellQueue.Enqueue((it.X, it.Y)));
        }

        return visited;
    }

    public static int FindPaths2(this int[,] map, (int X, int Y) trailHead)
    {
        var cellQueue = new Queue<(int X, int Y)>();
        cellQueue.Enqueue(trailHead);
        var uniquePaths = 0;
        while (cellQueue.Count != 0)
        {
            var (x, y) = cellQueue.Dequeue();
            var currentCellValue = map[x, y];

            var visitedCells = RelativeNeighbourPositions
                .Select(pos => (X: pos.X + x, Y: pos.Y + y))
                .Where(pos => pos.X >= 0 && pos.X < map.GetLength(1) && pos.Y >= 0 && pos.Y < map.GetLength(0))
                .Select(pos => (Pos: pos, Cell: map[pos.X, pos.Y]))
                .Where(it => it.Cell == currentCellValue + 1)
                .ToList();

            uniquePaths += visitedCells.Count(it => it.Cell == 9);

            visitedCells
                .Select(it => it.Pos)
                .ToList()
                .ForEach(it => cellQueue.Enqueue((it.X, it.Y)));
        }

        return uniquePaths;
    }
}