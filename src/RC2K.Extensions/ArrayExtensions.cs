namespace RC2K.Extensions;

public static class ArrayExtensions
{
    public static void XorSwap(this int[] arr, int i, int j)
    {
        arr[i] ^= arr[j];
        arr[j] ^= arr[i];
        arr[i] ^= arr[j];
    }

    public static IEnumerable<T> RandomNonRepetitive<T>(this T[] arr, Random rnd, int k)
    {
        IEnumerable<int> indices = rnd.NonRepetitiveSequence(k, arr.Length);
        foreach (var i in indices)
        {
            yield return arr[i];
        }
    }
}
