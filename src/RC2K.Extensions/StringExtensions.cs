namespace RC2K.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Splits input removing empty and trimming whitespace.
    /// </summary>
    public static string[] SplitClean(this string input, string separator) =>
        input.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
}
