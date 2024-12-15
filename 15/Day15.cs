using AdventOfCode2024._10;

namespace AdventOfCode2024._15;

using MoveSet = List<((int X, int Y), (int X, int Y), char)>;
using Result = (MoveResult Proceed, List<((int X, int Y) ActiveCell, (int X, int Y) Target, char Value)> moveSet);
using Day15Position = (int X, int Y);

public enum MoveResult
{
    Proceed,
    Cancel
}

public static class Day15
{
    private const char GuardSymbol = '@';
    private const char EmptySymbol = '.';
    private const char WallSymbol = '#';
    private const char BoxSymbol = 'O';

    public static void Run()
    {
        // Part 1
        {
            var (map, instructions) = ParseInput("./15/input.txt");

            var instructionQueue = new Queue<char>();
            instructions.ToList().ForEach(it => instructionQueue.Enqueue(it));

            // Print(map);

            while (instructionQueue.Count > 0)
            {
                var instruction = instructionQueue.Dequeue();
                var guardCell = map.CoordinatesOf(GuardSymbol);
                var (result, moveSet) = EvaluateMove(instruction, guardCell, map, []);
                if (result == MoveResult.Cancel) continue;
                moveSet.Reverse();
                foreach (var move in moveSet)
                {
                    map[move.Target.X, move.Target.Y] = move.Value;
                    map[move.ActiveCell.X, move.ActiveCell.Y] = EmptySymbol;
                }

                // Print(map);
            }

            var part1 = map.CoordinatesOfAll(BoxSymbol)
                .Select(pos => pos.X + (pos.Y * 100))
                .Sum();


            Console.WriteLine($"Part 1: {part1}");
        }

        // Part 2
        {
        }
    }

    private static void Print(char[,] map)
    {
        for (var y = 0; y < map.GetLength(0); ++y)
        {
            for (var x = 0; x < map.GetLength(1); ++x)
            {
                Console.Write(map[x, y]);
            }

            Console.WriteLine();
        }
    }

    private static Result EvaluateMove(
        char instruction,
        Day15Position activeCell,
        char[,] map,
        MoveSet moveSet
    )
    {
        var targetCell = instruction switch
        {
            '^' => (X: activeCell.X + 0, Y: activeCell.Y - 1),
            '>' => (X: activeCell.X + 1, Y: activeCell.Y + 0),
            'v' => (X: activeCell.X + 0, Y: activeCell.Y + 1),
            '<' => (X: activeCell.X - 1, Y: activeCell.Y + 0),
            _ => throw new ArgumentException($"Unknown instruction: {instruction}")
        };
        if (map[targetCell.X, targetCell.Y] == EmptySymbol)
        {
            moveSet.Add((activeCell, targetCell, map[activeCell.X, activeCell.Y]));
            return (MoveResult.Proceed, moveSet);
        }

        if (map[targetCell.X, targetCell.Y] == WallSymbol)
        {
            return (MoveResult.Cancel, []);
        }

        moveSet.Add((activeCell, targetCell, map[activeCell.X, activeCell.Y]));
        return EvaluateMove(instruction, targetCell, map, moveSet);
    }

    private static (char[,] map, char[] instructions) ParseInput(string filePath)
    {
        var lines = File
            .ReadAllText(filePath)
            .Split("\r\n\r\n");

        var mapData = lines[0];

        var map = mapData
            .Split("\r\n")
            .Select(row => row.ToCharArray().ToArray())
            .ToArray()
            .To2DArray();

        var instructions = lines[1]
            .Replace("\r", "")
            .Replace("\n", "")
            .ToCharArray();

        return (map, instructions);
    }
}

public static class Day15Extensions
{
    public static Day15Position CoordinatesOf<T>(this T[,] matrix, T value)
    {
        for (var y = 0; y < matrix.GetLength(0); ++y)
        for (var x = 0; x < matrix.GetLength(1); ++x)
        {
            if (matrix[x, y].Equals(value)) return (x, y);
        }

        return (-1, -1);
    }

    public static IEnumerable<Day15Position> CoordinatesOfAll<T>(this T[,] map, T value)
    {
        for (var y = 0; y < map.GetLength(0); ++y)
        for (var x = 0; x < map.GetLength(1); ++x)
        {
            if (map[x, y].Equals(value)) yield return (x, y);
        }
    }
}