namespace RC2K.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Splits input removing empty and trimming whitespace.
    /// </summary>
    public static string[] SplitClean(this string input, string separator) =>
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

    /// <summary>
    /// Replaces newlines with space and aggregates multiple spaces/tabs to single space.
    /// </summary>
    public static string Linearize(this string input) =>
        string.Join(" ",
            input.ReplaceLineEndings(" ")
                 .Replace("\t", " ")
                 .Split(" ", StringSplitOptions.RemoveEmptyEntries));
}
