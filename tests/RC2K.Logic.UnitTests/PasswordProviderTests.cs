namespace RC2K.Logic.UnitTests;

public class PasswordProviderTests
{
    private PasswordProvider _sut;

    [SetUp]
    public void Setup()
    {
        const int iterations = 5;
        const string salt = "salt";
        _sut = new(iterations, salt);
    }

    [Test]
    public void CalculatePasswordHash_ReturnsHashFromPassword()
    {
        //Arrange
        const string input = "P4$$w0rd";

        //Act
        var result = _sut.CalculatePasswordHash(input);

        //Assert
        Assert.That(result, Is.EqualTo("PLqDx1uvI61W0JXdlFiC7wj/6u5qC/JpOogAuTat2YaFDy3NilT6VnU8UoIakTFa98DraoDmy9+misgZRA0ofQ=="));
    }

    [Test]
    public void GenerateTemporaryPassword_ReturnsPasswordOfLength16WithAlphanumericCharacters()
    {
        //Arrange-Act
        var result = _sut.GenerateTemporaryPassword();

        //Assert
        Assert.That(result, Has.Length.EqualTo(16));
        Assert.That(result, Does.Match("^[\\da-zA-Z]*$"));
    }
}
