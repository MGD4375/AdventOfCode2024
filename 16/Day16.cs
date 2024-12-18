using System.Collections;
using AdventOfCode2024._10;

namespace AdventOfCode2024._16;

using Position = (int X, int Y);
using FlowMapItem = (
    char Direction,
    int X,
    int Y,
    char Value,
    int Cost,
    bool? IsVisited
    );

public static class Day16
{
    public static void Run()
    {
        // Part 1
        {
            var map = ParseInput("./16/test-input.txt");
            var reindeer = map.CoordinatesOf('S');
            var flowMap = BuildFlowMap(map, (reindeer.X, reindeer.Y), '<');
            PrintFlowMap(map, flowMap);
            // var score = outcomes.MinBy(o => o.Cost);
            // Console.WriteLine($"Part 1: {score}");
        }

        // Part 2
        {
        }
    }

    private static readonly char[] MovableSpots = ['.', 'E'];

    public static FlowMapItem[,] BuildFlowMap(
        char[,] map,
        Position startingPosition,
        char startingDirection
    )
    {
        var queue = new Queue<(char Direction, Position Position, int Cost)>();
        queue.Enqueue((startingDirection, startingPosition, 0));
        var flowMap = new FlowMapItem[map.GetLength(0), map.GetLength(1)];
        var holdingPen = new List<FlowMapItem>();
        while (queue.Any() || holdingPen.Any())
        {
            var current = queue.Dequeue();
            var relativeOptions = new List<(char Direction, int X, int Y)>
            {
                ('>', +1, +0), // right
                ('<', -1, +0), // left
                ('v', +0, +1), // down
                ('^', +0, -1), // up
            };

            var potentialMoves = relativeOptions
                .Select(pos => (Direction: pos.Direction, X: current.Position.X + pos.X, Y: current.Position.Y + pos.Y))
                .Where(pos => IsInBounds(map, (pos.X, pos.Y)))
                .Select(pos => (
                        Direction: pos.Direction,
                        X: pos.X, Y: pos.Y,
                        Value: map[pos.X, pos.Y],
                        Cost: startingDirection == pos.Direction ? current.Cost + 1 : current.Cost + 1001,
                        IsVisited: flowMap[pos.X, pos.Y].IsVisited ?? false
                    )
                )
                .Where(t => MovableSpots.Contains(t.Value))
                .Where(option => !option.IsVisited)
                .ToList();

            potentialMoves.ForEach(it =>
            {
                it.IsVisited = true;
                flowMap[it.X, it.Y] = it;
                if (it.Value != 'E')
                {
                    queue.Enqueue((it.Direction, (it.X, it.Y), it.Cost));
                }
            });
        }

        return flowMap;
    }

    private static bool IsInBounds(char[,] map, Position pos) =>
        pos.X >= 0
        && pos.X < map.GetLength(0)
        && pos.Y >= 0
        && pos.Y < map.GetLength(1);

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

    private static void PrintFlowMap(char[,] map, FlowMapItem[,] flowMap)
    {
        for (var y = 0; y < map.GetLength(1); ++y)
        {
            for (var x = 0; x < map.GetLength(0); ++x)
            {
                if (flowMap[x, y].IsVisited ?? false)
                {
                    Console.Write(flowMap[x, y].Direction);
                }
                else
                {
                    Console.Write(map[x, y]);
                }
            }

            Console.WriteLine();
        }
    }

    private static void PrintPath(char[,] map, List<Position> currentPath)
    {
        for (var y = 0; y < map.GetLength(1); ++y)
        {
            for (var x = 0; x < map.GetLength(0); ++x)
            {
                if (currentPath.Contains(new Position(x, y)))
                {
                    Console.Write("^");
                }
                else
                {
                    Console.Write(map[x, y]);
                }
            }

            Console.WriteLine();
        }
    }


    private static char[,] ParseInput(string filePath)
    {
        return File
            .ReadLines(filePath)
            .Select(row => row.ToCharArray().ToArray())
            .ToArray()
            .To2DArray();
    }
}

public static class Day16Extensions
{
    public static Position CoordinatesOf<T>(this T[,] matrix, T value)
    {
        for (var x = 0; x < matrix.GetLength(0); ++x)
        for (var y = 0; y < matrix.GetLength(1); ++y)
        {
            if (matrix[x, y].Equals(value)) return (x, y);
        }

        return (-1, -1);
    }
}