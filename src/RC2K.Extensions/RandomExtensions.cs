namespace RC2K.Extensions;

public static class RandomExtensions
{
    public static IEnumerable<int> NonRepetitiveSequence(this Random rnd, int k, int n)
    {
        EnsureKIsLowerThanN(k, n);

        if (k < n / 2) return NonRepetitiveSequenceByHashSet(rnd, k, n);
        else return NonRepetitiveSequenceByAlgorithm(rnd, k, n);
    }

    public static IEnumerable<int> NonRepetitiveSequenceByHashSet(this Random rnd, int k, int n)
    {
        EnsureKIsLowerThanN(k, n);
        HashSet<int> used = [];

        for (int i = 0; i<k; i++)
        {
            int num;
            for (num = rnd.Next(n); used.Contains(num); num = rnd.Next(n));
            used.Add(num);
            yield return num;
        }
    }

    public static IEnumerable<int> NonRepetitiveSequenceByAlgorithm(this Random rnd, int k, int n)
    {
        EnsureKIsLowerThanN(k, n);
        int[] numbers = Enumerable.Range(0, n).ToArray();

        int end = n;
        for (int i = 0; i < k; i++)
        {
            int idx = rnd.Next(end);
            int num = numbers[idx];
            yield return num;
            numbers.XorSwap(idx, end - 1);
            end--;
        }
    }

    private static void EnsureKIsLowerThanN(int k, int n)
    {
        if (k > n)
            throw new ArgumentException($"k ({k}) cannot be greater than n ({n})");
    }
}
