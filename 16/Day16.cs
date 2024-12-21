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
            var map = ParseInput("./16/input.txt");
            var reindeer = map.CoordinatesOf('S');
            var score = RunPart1(map, (reindeer.X, reindeer.Y), '<');
            Console.WriteLine($"Part 1: {score}");
        }

        // Part 2
        {
        }
    }

    private static readonly char[] MovableSpots = ['.', 'E'];

    public static int RunPart1(
        char[,] map,
        Position startingPosition,
        char startingDirection
    )
    {
        var relativeOptions = new List<(char Direction, int X, int Y)>
        {
            ('>', +1, +0), // right
            ('v', +0, +1), // down
            ('<', -1, +0), // left
            ('^', +0, -1), // up
        };


        var queue = new PriorityQueue<(char Direction, Position Position, int Cost), int>();
        queue.Enqueue((startingDirection, startingPosition, 0), 1);
        var visited = new List<(char Direction, Position Position)>();

        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();
            if (visited.Contains((currentPosition.Direction, currentPosition.Position))) continue;

            if (map[currentPosition.Position.X, currentPosition.Position.Y] == 'E')
            {
                return currentPosition.Cost;
            }

            visited.Add((currentPosition.Direction, currentPosition.Position));

            //  Move Forward
            {
                var dir = relativeOptions.First(o => o.Direction == currentPosition.Direction);
                var target = (X: dir.X + currentPosition.Position.X, Y: dir.Y + currentPosition.Position.Y);
                if (MovableSpots.Contains(map[target.X, target.Y]))
                {
                    queue.Enqueue((
                            Direction: dir.Direction,
                            Position: target,
                            Cost: currentPosition.Cost + 1),
                        currentPosition.Cost + 1
                    );
                }
            }

            // Turn Left
            {
                var leftDir = TurnLeft(currentPosition.Direction);
                var dir = relativeOptions.First(o => o.Direction == leftDir);
                var target = (X: dir.X + currentPosition.Position.X, Y: dir.Y + currentPosition.Position.Y);
                if (MovableSpots.Contains(map[target.X, target.Y]))
                {
                    queue.Enqueue((
                            Direction: leftDir,
                            Position: currentPosition.Position,
                            Cost: currentPosition.Cost + 1000),
                        currentPosition.Cost + 1000
                    );
                }
            }

            // Turn Right
            {
                var rightDir = TurnRight(currentPosition.Direction);
                var dir = relativeOptions.First(o => o.Direction == rightDir);
                var target = (X: dir.X + currentPosition.Position.X, Y: dir.Y + currentPosition.Position.Y);
                if (MovableSpots.Contains(map[target.X, target.Y]))
                {
                    queue.Enqueue((
                            Direction: rightDir,
                            Position: currentPosition.Position,
                            Cost: currentPosition.Cost + 1000),
                        currentPosition.Cost + 1000
                    );
                }
            }
        }

        throw new Exception("No solution");
    }

    private static char TurnLeft(char direction) =>
        direction switch
        {
            '>' => '^',
            'v' => '>',
            '<' => 'v',
            '^' => '<',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

    private static char TurnRight(char direction) =>
        direction switch
        {
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            '^' => '>',
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };

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