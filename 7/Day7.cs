namespace AdventOfCode2024._7;

public static class Day7
{
    public static void Run()
    {
        var input = File.ReadAllText("./7/input.txt");
        var calibrations = input.Split("\r\n")
            .Where(r => r.Length > 0)
            .Select(r =>
            {
                var row = r.Split(':');
                var testValue = long.Parse(row[0]);
                var remainingNumbers = row[1]
                    .Split(' ')
                    .Where(it => it.Length > 0)
                    .Select(it => new Number(long.Parse(it)));
                return (testValue, remainingNumbers);
            });


        var part1 = calibrations.Sum(c =>
        {
            var testValue = c.testValue;
            var remainingNumbers = c.remainingNumbers;
            var remainingNumbersWithOperations = GetAllPossible(remainingNumbers);
            var canSum = remainingNumbersWithOperations.Any(rnwo => Evaluate(rnwo) == testValue);
            return canSum ? c.testValue : 0;
        });

        Console.WriteLine($"Part 1: {part1}");
    }

    private static long Evaluate(IEnumerable<MathsItem> items)
    {
        var output = items.Aggregate((NextOperator: new Operator('+'), Total: 0L), (aggregator, item) =>
        {
            if (item is Operator op)
            {
                return aggregator with { NextOperator = op };
            }

            if (item is Number number)
            {
                var newTotal = aggregator.NextOperator.Value switch
                {
                    '+' => aggregator.Total + number.Value,
                    '*' => aggregator.Total * number.Value,
                    _ => throw new Exception("Operator not handled")
                };
                return aggregator with { Total = newTotal };
            }

            throw new Exception("MathsItem type not handled");
        });
        return output.Total;
    }


    private static IEnumerable<IEnumerable<MathsItem>> GetAllPossible(IEnumerable<MathsItem> remainingNumbers)
    {
        var numbers = remainingNumbers.ToList();

        if (numbers.Count == 0)
        {
            yield break;
        }

        char[] operators = ['*', '+'];
        var operatorCount = numbers.Count - 1;

        foreach (var operatorCombination in GenerateOperatorCombinations(operators, operatorCount))
        {
            var result = new List<MathsItem>();
            for (var i = 0; i < numbers.Count; i++)
            {
                result.Add(numbers[i]);
                if (i < operatorCount)
                {
                    result.Add(new Operator(operatorCombination[i]));
                }
            }

            yield return result;
        }
    }

    private static IEnumerable<char[]> GenerateOperatorCombinations(char[] operators, long length)
    {
        if (length == 0)
        {
            yield return [];
            yield break;
        }

        foreach (var combination in GenerateOperatorCombinations(operators, length - 1))
        {
            foreach (var op in operators)
            {
                var newCombination = new char[length];
                Array.Copy(combination, newCombination, combination.Length);
                newCombination[length - 1] = op;
                yield return newCombination;
            }
        }

        if (length == 1)
        {
            foreach (var op in operators)
            {
                yield return [op];
            }
        }
    }


    private abstract record MathsItem;

    private record Operator(char Value) : MathsItem;

    private record Number(long Value) : MathsItem;
}