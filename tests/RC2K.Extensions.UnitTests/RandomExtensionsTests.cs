namespace RC2K.Extensions.UnitTests;

public class RandomExtensionsTests
{
    [Test]
    public void NonRepetitiveSequence_K_IsMuchLowerThan_N_ReturnsValidSequence()
    {
        //Arrange
        const int n = 20;
        const int k = 5;

        //Act
        var result = Random.Shared.NonRepetitiveSequence(k, n).ToList();

        //Assert
        Assert.That(result, Has.Count.EqualTo(k));
        AssertSequenceIsDistinct(result);
    }

    [Test]
    public void NonRepetitiveSequence_K_IsLittleLowerThan_N_ReturnsValidSequence()
    {
        //Arrange
        const int n = 20;
        const int k = 18;

        //Act
        var result = Random.Shared.NonRepetitiveSequence(k, n).ToList();

        //Assert
        Assert.That(result, Has.Count.EqualTo(k));
        AssertSequenceIsDistinct(result);
    }

    [Test]
    public void NonRepetitiveSequence_K_IsGreaterThan_N_ThrowsException() =>
        Assert.Throws<ArgumentException>(() => Random.Shared.NonRepetitiveSequence(30, 20));

    private static void AssertSequenceIsDistinct(IEnumerable<int> enumerable)
    {
        var list = enumerable.OrderBy(x => x).ToList();

        int prev = -1;
        for (int i = 0; i < list.Count; i++)
        {
            Assert.That(list[i], Is.GreaterThan(prev));
            prev = list[i];
        }
    }
}
