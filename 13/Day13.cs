using System.Text.RegularExpressions;

namespace AdventOfCode2024._13;

using Solution = (int Cost, int APresses, int BPresses);
using Problem = (Button AButton, Button BButton, Position Target);

public record Position(ulong X, ulong Y);

public static class Day13
{
    public static void Run()
    {
        // Part 1
        {
            var part1 = ParseProblems("./13/input.txt")
                .Select(SolveForMachinePart1)
                .Where(solutions => solutions.Any())
                .Select(solutions => solutions.MinBy(solution => solution.Cost))
                .Sum(solution => solution.Cost);
            Console.WriteLine($"Part 1: {part1}");
        }

        // Part 2
        {
            var part2 = ParseProblems("./13/input.txt")
                .ToPart2Problems()
                .Select(SolveForMachinePart2)
                .Select(solutions => solutions.FirstOrDefault())
                .Sum(solution => solution.Cost);

            Console.WriteLine($"Part 2: {part2}");
        }
    }

    private static IEnumerable<Problem> ParseProblems(string filePath)
    {
        var input = File
            .ReadAllText(filePath)
            .Split("\r\n\r\n");

        foreach (var problemText in input)
        {
            var lines = problemText.Split("\r\n");
            var line1 = lines[0];
            var line2 = lines[1];
            var line3 = lines[2];

            var aX = ulong.Parse(Regex.Match(line1, @"(?<=X\+)[0-9]*").Value);
            var aY = ulong.Parse(Regex.Match(line1, @"(?<=Y\+)[0-9]*").Value);
            var bX = ulong.Parse(Regex.Match(line2, @"(?<=X\+)[0-9]*").Value);
            var bY = ulong.Parse(Regex.Match(line2, @"(?<=Y\+)[0-9]*").Value);
            var targetX = ulong.Parse(Regex.Match(line3, @"(?<=X\=)[0-9]*").Value);
            var targetY = ulong.Parse(Regex.Match(line3, @"(?<=Y\=)[0-9]*").Value);

            yield return (
                AButton: new Button(aX, aY),
                BButton: new Button(bX, bY),
                Target: new Position(targetX, targetY)
            );
        }
    }

    private static IEnumerable<Problem> ToPart2Problems(this IEnumerable<Problem> problems) => problems
        .Select(p => p with
            {
                Target = new Position(
                    p.Target.X + 10000000000000,
                    p.Target.Y + 10000000000000)
            }
        );


    public static IEnumerable<Solution> SolveForMachinePart1(Problem problem)
    {
        for (ulong aPresses = 0; aPresses < 100; aPresses++)
        {
            for (ulong bPresses = 0; bPresses < 100; bPresses++)
            {
                var cost = (aPresses * 3) + bPresses;
                var xHit = (aPresses * problem.AButton.XMove) + (bPresses * problem.BButton.XMove);
                var yHit = (aPresses * problem.AButton.YMove) + (bPresses * problem.BButton.YMove);
                var posHit = new Position(xHit, yHit);
                if (posHit == problem.Target)
                {
                    yield return (Cost: (int)cost, APresses: (int)aPresses, BPresses: (int)bPresses);
                }
            }
        }
    }

    public static IEnumerable<Solution> SolveForMachinePart2(Problem problem)
    {
        for (ulong aPresses = 0; aPresses < 100; aPresses++)
        {
            for (ulong bPresses = 0; bPresses < 100; bPresses++)
            {
                var cost = (aPresses * 3) + bPresses;
                var xHit = (aPresses * problem.AButton.XMove) + (bPresses * problem.BButton.XMove);
                var yHit = (aPresses * problem.AButton.YMove) + (bPresses * problem.BButton.YMove);
                var posHit = new Position(xHit, yHit);
                if (posHit == problem.Target)
                {
                    Console.WriteLine($"Solution found for problem: {problem}");
                    yield return (Cost: (int)cost, APresses: (int)aPresses, BPresses: (int)bPresses);
                }
            }
        }
    }
}

public class Button(ulong xMove, ulong yMove)
{
    public ulong XMove { get; } = xMove;
    public ulong YMove { get; } = yMove;
}