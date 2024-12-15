using AdventOfCode2024._10;

namespace AdventOfCode2024._15;

using MoveSet = List<((int X, int Y), (int X, int Y), char)>;
using Result = (MoveResult Proceed, List<((int X, int Y) ActiveCell, (int X, int Y) Target, char Value)> MoveSet);
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
    private const char BoxLeftSymbol = '[';
    private const char BoxRightSymbol = ']';

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
                var (result, moveSet) = EvaluateMove(instruction, guardCell, map);
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
            var (map, instructions) = ParseInputPart2("./15/input.txt");

            var instructionQueue = new Queue<char>();
            instructions.ToList().ForEach(it => instructionQueue.Enqueue(it));

            while (instructionQueue.Count > 0)
            {
                var instruction = instructionQueue.Dequeue();
                var guardCell = map.CoordinatesOf(GuardSymbol);
                var (result, moveSet) = EvaluateMove(instruction, guardCell, map);
                if (result == MoveResult.Cancel) continue;
                moveSet.Reverse();
                foreach (var move in moveSet)
                {
                    map[move.Target.X, move.Target.Y] = move.Value;

                    var foo = moveSet.Any(it =>
                        it.Target.X == move.ActiveCell.X && it.Target.Y == move.ActiveCell.Y);

                    map[move.ActiveCell.X, move.ActiveCell.Y] = foo
                        ? moveSet.First(it => it.Target.X == move.ActiveCell.X && it.Target.Y == move.ActiveCell.Y)
                            .Value
                        : EmptySymbol;
                }

                // Print(map);
            }

            var part1 = map.CoordinatesOfAll(BoxLeftSymbol)
                .Select(pos => pos.X + (pos.Y * 100))
                .Sum();


            Console.WriteLine($"Part 2: {part1}");
        }
    }


    private static void Print(char[,] map)
    {
        for (var y = 0; y < map.GetLength(1); ++y)
        {
            for (var x = 0; x < map.GetLength(0); ++x)
            {
                Console.Write(map[x, y]);
            }

            Console.WriteLine();
        }
    }

    private static Result EvaluateMove(
        char instruction,
        Day15Position activeCell,
        char[,] map
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


        if (map[targetCell.X, targetCell.Y] == WallSymbol)
        {
            return (MoveResult.Cancel, []);
        }

        var moveSet = new MoveSet() { (activeCell, targetCell, map[activeCell.X, activeCell.Y]) };
        var nextMoves = new List<Day15Position>();
        if (map[targetCell.X, targetCell.Y] != EmptySymbol)
        {
            nextMoves.Add(targetCell);
        }

        if (map[targetCell.X, targetCell.Y] == BoxLeftSymbol && !new[] { '<', '>' }.Contains(instruction))
        {
            nextMoves.Add((targetCell.X + 1, targetCell.Y));
        }
        else if (map[targetCell.X, targetCell.Y] == BoxRightSymbol && !new[] { '<', '>' }.Contains(instruction))
        {
            nextMoves.Add((targetCell.X - 1, targetCell.Y));
        }

        var results = nextMoves.Select(next => EvaluateMove(instruction, next, map));
        moveSet.AddRange(results.SelectMany(it => it.MoveSet));
        var result = (
            Proceed: results.Any(it => it.Proceed == MoveResult.Cancel) ? MoveResult.Cancel : MoveResult.Proceed,
            MoveSet: moveSet
        );

        return result;
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

    private static (char[,] map, char[] instructions) ParseInputPart2(string filePath)
    {
        var lines = File
            .ReadAllText(filePath)
            .Split("\r\n\r\n");

        var mapData = lines[0];

        var map = mapData
            .Split("\r\n")
            .Select(row => row
                .SelectMany(ch => ch switch
                {
                    WallSymbol => new[] { WallSymbol, WallSymbol },
                    BoxSymbol => new[] { BoxLeftSymbol, BoxRightSymbol },
                    EmptySymbol => new[] { EmptySymbol, EmptySymbol },
                    GuardSymbol => new[] { GuardSymbol, EmptySymbol },
                    _ => throw new ArgumentOutOfRangeException(nameof(ch), ch, null)
                })
                .ToArray()
            )
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
        for (var x = 0; x < matrix.GetLength(0); ++x)
        for (var y = 0; y < matrix.GetLength(1); ++y)
        {
            if (matrix[x, y].Equals(value)) return (x, y);
        }

        return (-1, -1);
    }

    public static IEnumerable<Day15Position> CoordinatesOfAll<T>(this T[,] map, T value)
    {
        for (var x = 0; x < map.GetLength(0); ++x)
        for (var y = 0; y < map.GetLength(1); ++y)
        {
            if (map[x, y].Equals(value)) yield return (x, y);
        }
    }
}