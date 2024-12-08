namespace AdventOfCode2024._7;

public static class Day8
{
    public record Position(int X, int Y)
    {
        public Position DistanceTo(Position other) => new(other.X - X, other.Y - Y);
        public Position Add(Position other) => new(other.X + X, other.Y + Y);
        public Position MultipliedBy(int multiplier) => new(X * multiplier, Y * multiplier);
    };

    public record Cell(Position Position, char Content);

    public static void Run()
    {
        var input = File.ReadAllText("./8/input.txt");
        var map = input.Split("\r\n")
            .Where(r => r.Length > 0)
            .Select((row, rIndex) => row
                .Select((cellContent, cIndex) =>
                    new Cell(new Position(cIndex, rIndex), cellContent))
                .ToList())
            .ToList();

        // Part 1
        {
            var groups = map
                .SelectMany(r => r.Select(c => c))
                .GroupBy(c => c.Content)
                .Where(c => c.Key != '.');

            var distinctNodePositions = groups.Select(group =>
                {
                    var frequency = group.Key;
                    var items = group.Select(it => it);
                    var cartesianProduct = items
                        .SelectMany(itA => items, (itA, itB) => (itA, itB))
                        .Where(product => product.itA != product.itB);

                    return cartesianProduct
                        .Select(pair =>
                        {
                            var distance = pair.itA.Position.DistanceTo(pair.itB.Position);
                            return pair.itB.Position.Add(distance);
                        });
                })
                .SelectMany(nodePerGroup => nodePerGroup.Select(n => n))
                .Distinct()
                .Where(node => IsInBounds(map, node));


            var total = distinctNodePositions.Count();

            Console.WriteLine($"Part 1: {total}");
        }

        // Part 2
        {
            var groups = map
                .SelectMany(r => r.Select(c => c))
                .GroupBy(c => c.Content)
                .Where(c => c.Key != '.');

            var distinctNodePositions = groups.Select(group =>
                {
                    var frequency = group.Key;
                    var items = group.Select(it => it);
                    var cartesianProduct = items
                        .SelectMany(itA => items, (itA, itB) => (itA, itB))
                        .Where(product => product.itA != product.itB);

                    return cartesianProduct
                        .SelectMany(pair =>
                        {
                            var targetLocations = new List<Position>();
                            targetLocations.Add(pair.itA.Position);
                            targetLocations.Add(pair.itB.Position);
                            var lastItemWasInBounds = true;
                            var multiplier = 0;
                            var distance = pair.itA.Position.DistanceTo(pair.itB.Position);
                            while (lastItemWasInBounds)
                            {
                                multiplier += 1;
                                var resonantDistance = distance.MultipliedBy(multiplier);
                                var targetLocation = pair.itB.Position.Add(resonantDistance);
                                var targetIsInBounds = IsInBounds(map, targetLocation);
                                if (targetIsInBounds) targetLocations.Add(targetLocation);
                                else lastItemWasInBounds = false;
                            }

                            return targetLocations;
                        });
                })
                .SelectMany(nodePerGroup => nodePerGroup.Select(n => n))
                .Distinct();

            // PrintPositions(map, distinctNodePositions.ToList());

            var total = distinctNodePositions.Count();

            Console.WriteLine($"Part 2: {total}");
        }
    }

    private static bool IsInBounds(List<List<Cell>> map, Position node)
    {
        var maxX = map.First().Count;
        var maxY = map.Count;
        return node.X < maxX && node.Y < maxY && node.X >= 0 && node.Y >= 0;
    }

    private static void PrintPositions(List<List<Cell>> map, List<Position> positions)
    {
        var maxX = map.First().Count;
        var maxY = map.Count;

        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                Console.Write(positions.Any(p => p == new Position(x, y)) ? "0" : ".");
            }

            Console.WriteLine();
        }
    }
}