namespace AdventOfCode2024._10;

public static class ProjectExtensions
{
    [Obsolete("I realised this is broken, but I keep it around because it worked for earlier problems.")]
    public static T[,] To2DArrayOld<T>(this T[][] source)
    {
        var result = new T[source.Length, source[0].Length];
        for (var i = 0; i < source.Length; i++)
        for (var j = 0; j < source[i].Length; j++)
            result[i, j] = source[i][j];
        return result;
    }

    public static T[,] To2DArray<T>(this T[][] source)
    {
        var result = new T[source[0].Length, source.Length];
        for (var y = 0; y < source.Length; y++)
        for (var x = 0; x < source[y].Length; x++)
            result[x, y] = source[y][x];
        return result;
    }
}