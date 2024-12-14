using System.Text.RegularExpressions;

namespace AdventOfCode2024._14;

public record Robot(Position Position, Velocity Velocity);

public record Position(int X, int Y)
{
    public static Position operator +(Position position, Velocity other) =>
        new(position.X + other.X, position.Y + other.Y);
};

public record Velocity(int X, int Y)
{
    public static Velocity operator *(Velocity vector, int other) =>
        new(vector.X * other, vector.Y * other);
};

public static class Day14
{
    public static void Run()
    {
        // Part 1
        {
            var input = ParseInput("./14/input.txt");
            // var (maxX, maxY) = (11, 7);
            var (maxX, maxY) = (101, 103);
            const int ticksToEvaluate = 100;
            var halfwayX = ((maxX - 1) / 2);
            var halfwayY = ((maxY - 1) / 2);

            var positions = input
                .Select(robot =>
                {
                    var totalDistance = robot.Velocity * ticksToEvaluate;
                    var restingPlace = robot.Position + totalDistance;
                    var finalX = (restingPlace.X % maxX + maxX) % maxX;
                    var finalY = (restingPlace.Y % maxY + maxY) % maxY;
                    return new Position(finalX, finalY);
                });

            // Print(positions, maxX, maxY);

            var part1 = positions
                .Where(position => position.X != halfwayX && position.Y != halfwayY)
                .GroupBy(robotPosition =>
                {
                    if (robotPosition.X < halfwayX && robotPosition.Y < halfwayY) return 1;
                    if (robotPosition.X < halfwayX && robotPosition.Y > halfwayY) return 2;
                    if (robotPosition.X > halfwayX && robotPosition.Y < halfwayY) return 3;
                    if (robotPosition.X > halfwayX && robotPosition.Y > halfwayY) return 4;
                    throw new InvalidOperationException();
                })
                .Select(group => group.Count())
                .Aggregate((a, b) => a * b);

            Console.WriteLine($"Part 1: {part1}");
        }

        // Part 2
        {
        }
    }

    public static void Print(IEnumerable<Position> positions, int maxX, int maxY)
    {
        var halfwayX = ((maxX - 1) / 2);
        var halfwayY = ((maxY - 1) / 2);

        for (var y = 0; y < maxY; y++)
        {
            Console.Write($"(Row {y}):\t");

            for (var x = 0; x < maxX; x++)
            {
                if (positions.Any(position => position.X == x && position.Y == y))
                    Console.Write('#'
                    );
                else if (x == halfwayX || y == halfwayY)
                    Console.Write(' ');
                else
                {
                    Console.Write('.');
                }
            }

            Console.WriteLine();
        }
    }

    private static IEnumerable<Robot> ParseInput(string filePath)
    {
        var lines = File
            .ReadAllText(filePath)
            .Split("\r\n");

        return lines.Select(line =>
        {
            var positionMatch = Regex.Match(line, @"(?<=p\=)[0-9]*,[0-9]*").Value.Split(',');
            var position = new Position(int.Parse(positionMatch[0]), int.Parse(positionMatch[1]));

            var velocityMatch = Regex.Match(line, @"(?<=v\=).*").Value.Split(',');
            var velocity = new Velocity(int.Parse(velocityMatch[0]), int.Parse(velocityMatch[1]));

            return new Robot(position, velocity);
        });
    }
}