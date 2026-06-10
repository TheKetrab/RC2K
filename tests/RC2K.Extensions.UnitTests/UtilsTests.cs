using RC2K.DomainModel;

namespace RC2K.Extensions.UnitTests;

public class UtilsTests
{
    [TestCase('s', Direction.Simulation)]
    [TestCase('a', Direction.Arcade)]
    public void CharToDirection_ValidInput_ConvertsProperly(char c, Direction d) =>
        Assert.That(Utils.Utils.CharToDirection(c), Is.EqualTo(d));

    [Test]
    public void CharToDirection_InvalidInput_ThrowsException() =>
        Assert.Throws<ArgumentException>(() => Utils.Utils.CharToDirection('0'));

    [TestCase(Direction.Simulation, 's')]
    [TestCase(Direction.Arcade, 'a')]
    public void DirectionToChar_ValidInput_ConvertsProperly(Direction d, char c) =>
        Assert.That(Utils.Utils.DirectionToChar(d), Is.EqualTo(c));

    [Test]
    public void DirectionToChar_InvalidInput_ThrowsException() =>
        Assert.Throws<ArgumentException>(() => Utils.Utils.DirectionToChar((Direction)100));

    [TestCase('i', ProofType.Image)]
    [TestCase('t', ProofType.Twitch)]
    [TestCase('y', ProofType.Youtube)]
    [TestCase('r', ProofType.Replay)]
    [TestCase('?', ProofType.Unknown)]
    public void DeserializeProof_WithVerticalBar_ConvertsToTypeAndSelectsUrl(char c, ProofType expectedProofType)
    {
        //Arrange
        string input = $"{c}|https://example.com";

        //Act
        var result = Utils.Utils.DeserializeProof(input);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Type, Is.EqualTo(expectedProofType));
            Assert.That(result.Url, Is.EqualTo("https://example.com"));
        }
    }

    [Test]
    public void DeserializeProof_NoVerticalBar_EntireInputIsUrl()
    {
        //Arrange
        const string input = "hello_world";

        //Act
        var result = Utils.Utils.DeserializeProof(input);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Type, Is.EqualTo(ProofType.Unknown));
            Assert.That(result.Url, Is.EqualTo(input));
        }
    }

}
