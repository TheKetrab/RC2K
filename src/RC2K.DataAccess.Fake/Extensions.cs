using Bogus;

namespace RC2K.DataAccess.Fake;

public static class RandomizerExtensions
{
    public static int Id(this Randomizer randomizer, int cnt) =>
        randomizer.Int(0, cnt-1) + 1;

    public static int FromArray(this Randomizer randomizer, int[] array) =>
        array[randomizer.Int(0, array.Length-1)];
}
