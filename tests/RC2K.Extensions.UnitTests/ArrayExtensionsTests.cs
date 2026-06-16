namespace RC2K.Extensions.UnitTests;

public class ArrayExtensionsTests
{
    [Test]
    public void XorSwap_SwapsIndices()
    {
        //Arrange
        int[] arr = [0, 1, 2, 3, 4];

        //Act
        arr.XorSwap(1, 3);

        //Assert
        Assert.That(arr, Is.EquivalentTo([0, 3, 2, 1, 4]));
    }

    [Test]
    public void RandomNonRepetitive_GetsFromShuffledArray()
    {
        //Arrange
        int[] arr = [0, 1, 2, 3];

        //Act
        var res = arr.RandomNonRepetitive(Random.Shared, 3).ToArray();

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(res.Length, Is.EqualTo(3));
            Assert.That(res[0] != res[1] && res[0] != res[2] && res[1] != res[2]);
        }
    }
}
