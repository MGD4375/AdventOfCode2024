namespace AdventOfCode2024._10;

public static class ProjectExtensions
{
    public static T[,] To2DArray<T>(this T[][] source)
    {
        var result = new T[source.Length, source[0].Length];
        for (var i = 0; i < source.Length; i++)
        for (var j = 0; j < source[i].Length; j++)
            result[i, j] = source[i][j];
        return result;
    }
}