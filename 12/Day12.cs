namespace AdventOfCode2024._12;

public static class Day12Extensions
{
    public static List<Plot> AllPlots(this Garden garden) => garden.Plots.SelectMany(r => r.Select(p => p)).ToList();
}

public class Garden
{
    public List<List<Plot>> Plots { get; }

    public Garden(List<List<Plot>> plots)
    {
        Plots = plots;
    }
}

public class Plot
{
    public Position Position { get; set; }
    public char Value { get; set; }
    public int? RegionId { get; set; }

    public Plot(Position position, char value, int? regionId)
    {
        Position = position;
        Value = value;
        RegionId = regionId;
    }
}

public record Position(int X, int Y);

public static class Day12
{
    public static void Run()
    {
        var input = File.ReadAllText("./12/input.txt")
            .Split("\r\n")
            .Select((row, rIndex) => row.ToCharArray().ToList()
                .Select((plot, colIndex) => new Plot(new Position(rIndex, colIndex), plot, null)
                ).ToList())
            .ToList();

        var garden = new Garden(input);


        // Part 1
        {
            var relativeNeighbours = new List<Position>
            {
                new(-1, -0),
                new(-0, -1),
                new(+1, -0),
                new(-0, +1),
            };

            // while there are un-regiioned plots
            var currentKeyPlot = garden.AllPlots().FirstOrDefault(p => p.RegionId is null);
            var currentRegionId = 0;
            while (currentKeyPlot is not null)
            {
                currentRegionId++;

                currentKeyPlot.RegionId = currentRegionId;
                var queue = new Queue<Plot>();
                queue.Enqueue(currentKeyPlot);

                while (queue.Count > 0)
                {
                    var currentPlot = queue.Dequeue();
                    var nextPlots = relativeNeighbours
                        .Select(pos => new Position(currentPlot.Position.X + pos.X, currentPlot.Position.Y + pos.Y))
                        .Where(pos => pos is { X: >= 0, Y: >= 0 }
                                      && pos.X < garden.Plots.First().Count
                                      && pos.Y < garden.Plots.Count)
                        .Select(pos => garden.Plots[pos.X][pos.Y])
                        .Where(p => p.RegionId == null && p.Value == currentPlot.Value);

                    nextPlots
                        .ToList()
                        .ForEach(p =>
                        {
                            p.RegionId = currentRegionId;
                            queue.Enqueue(p);
                        });
                }

                currentKeyPlot = garden.AllPlots().FirstOrDefault(p => p.RegionId is null);
            }

            var regions = garden.AllPlots().GroupBy(r => r.RegionId);
            var foo = regions.Select(region =>
            {
                var area = region.Count();
                var edges = region.Select(plot =>
                    {
                        return 4 - relativeNeighbours
                            .Select(pos => new Position(plot.Position.X + pos.X, plot.Position.Y + pos.Y))
                            .Where(pos => pos is { X: >= 0, Y: >= 0 }
                                          && pos.X < garden.Plots.First().Count
                                          && pos.Y < garden.Plots.Count)
                            .Select(pos => garden.Plots[pos.X][pos.Y])
                            .Count(p => p.RegionId == plot.RegionId);
                    })
                    .Sum();

                return (Value: region.First().Value, Area: area, Edges: edges);
            });

            foreach (var bar in foo)
            {
                Console.WriteLine(bar);
            }

            var zax = foo.Sum(it => it.Area * it.Edges);
            Console.WriteLine("Part 1: " + zax);

            // Select from plots
            //  area
            // edges where not plot type
            //
            // select from tuple
            // area * edges
            //  sum and print :)
        }

        // Part 2
        {
            // Part 2 asks how many continuous edges there are. I haven't got an elegant way of getting that answer quickly.🤔
        }
    }
}