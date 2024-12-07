using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AdventOfCode2024._5;

public static class Day6
{
    public static void Run()
    {
        var map = ReadMapData();


        //part 1
        {
            (_, map) = RunMap(map);

            var part1 = map
                .SelectMany(r => r.Select(c => c))
                .Count(c => c.HasBeenVisited);


            Console.WriteLine("Part 1: " + part1);
        }

        //part 2 
        {
            var firstMap = ReadMapData();
            var (_, firstRun) = RunMap(firstMap);

            var potentialBlockers = firstRun.SelectMany(r => r.Select(c => c)).Where(c => c.HasBeenVisited);

            var results = new List<(bool, List<List<Cell>>)>();
            foreach (var cell in potentialBlockers)
            {
                var newMap = ReadMapData();
                var impactedCell = newMap.SelectMany(r => r.Select(c => c)).First(c => c.Position == cell.Position);
                impactedCell.Content = CellContents.Blocker;
                var valueTuple = RunMap(newMap);
                results.Add(valueTuple);
                if (valueTuple.Item1)
                {
                    Console.WriteLine(valueTuple);
                }
            }

            var answer = results.Count(r => r.Item1);

            Console.WriteLine("Part 2: " + answer);
        }
    }

    private static List<List<Cell>> ReadMapData()
    {
        return File.ReadAllText("./6/test-input.txt")
            .Split("\r\n")
            .Select((row, rIndex) => row
                .ToCharArray()
                .Select((ch, colIndex) => new Cell(ch, rIndex, colIndex))
                .ToList())
            .ToList();
    }

    private static (bool isStuckInLoop, List<List<Cell>> map) RunMap(List<List<Cell>> map)
    {
        var isStuckInLoop = false;
        while (GuardIsInMap(map) && !isStuckInLoop)
        {
            (isStuckInLoop, map) = Tick(map);
        }

        return (isStuckInLoop, map);
    }

    private static (bool IsStuckInLoop, List<List<Cell>> Map) Tick(List<List<Cell>> map)
    {
        var cells = map.SelectMany(x => x.Select(y => y));
        var guardCell = cells.First(c => c.Content == CellContents.Guard);

        var targetCell = GetGuardTargetCell(guardCell, cells);


        if (targetCell is null)
        {
            guardCell.GuardFacing = null;
            guardCell.Content = CellContents.Empty;
            guardCell.HasBeenVisited = true;
            return (false, map);
        }

        if (targetCell.Content == CellContents.Blocker)
        {
            guardCell.GuardFacing = guardCell.GuardFacing switch
            {
                GuardFacing.Up => GuardFacing.Right,
                GuardFacing.Right => GuardFacing.Down,
                GuardFacing.Down => GuardFacing.Left,
                GuardFacing.Left => GuardFacing.Up,
                _ => throw new ArgumentOutOfRangeException()
            };
            return (false, map);
            ;
        }

        targetCell.Content = CellContents.Guard;
        targetCell.GuardFacing = guardCell.GuardFacing;

        guardCell.GuardFacing = null;
        guardCell.Content = CellContents.Empty;

        guardCell.HasBeenVisited = true;
        guardCell.DirectionGuardWasFacingWhenHeVisited = guardCell.GuardFacing;

        if (targetCell.HasBeenVisited && targetCell.DirectionGuardWasFacingWhenHeVisited ==
            guardCell.DirectionGuardWasFacingWhenHeVisited)
        {
            return (true, map);
        }

        return (false, map);
    }

    private static Cell? GetGuardTargetCell(Cell guard, IEnumerable<Cell> cells)
    {
        return guard.GuardFacing switch
        {
            GuardFacing.Up => cells.FirstOrDefault(c =>
                guard.Position.X + 0 == c.Position.X && guard.Position.Y - 1 == c.Position.Y),
            GuardFacing.Right => cells.FirstOrDefault(c =>
                guard.Position.X + 1 == c.Position.X && guard.Position.Y - 0 == c.Position.Y),
            GuardFacing.Down => cells.FirstOrDefault(c =>
                guard.Position.X + 0 == c.Position.X && guard.Position.Y + 1 == c.Position.Y),
            GuardFacing.Left => cells.FirstOrDefault(c =>
                guard.Position.X - 1 == c.Position.X && guard.Position.Y + 0 == c.Position.Y),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static bool GuardIsInMap(List<List<Cell>> map) =>
        map.SelectMany(row => row.Select(c => c))
            .Any(c => c.Content == CellContents.Guard);

    public enum CellContents
    {
        Empty,
        Blocker,
        Guard
    }

    public enum GuardFacing
    {
        Up,
        Right,
        Down,
        Left,
    }


    private class Cell
    {
        public (int X, int Y) Position { get; init; }
        public bool HasBeenVisited { get; set; } = false;
        public GuardFacing? DirectionGuardWasFacingWhenHeVisited { get; set; } = null;
        public CellContents Content { get; set; }
        public GuardFacing? GuardFacing { get; set; }

        public Cell(char value, int rowIndex, int colIndex)
        {
            Position = (colIndex, rowIndex);
            Content = value switch
            {
                '#' => CellContents.Blocker,
                '.' => CellContents.Empty,
                '^' => CellContents.Guard,
                _ => throw new Exception()
            };
            GuardFacing = value switch
            {
                '^' => Day6.GuardFacing.Up,
                _ => null
            };
        }
    }
}