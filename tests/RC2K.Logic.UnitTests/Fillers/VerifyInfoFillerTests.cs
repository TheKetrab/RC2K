using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.Fillers.UnitTests;

public class VerifyInfoFillerTests
{
    private VerifyInfoFiller _sut;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IFillersBag> _fillersBagMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _fillersBagMock = new Mock<IFillersBag>();
        _sut = new(_userRepositoryMock.Object);
    }

    [Test]
    public async Task FillRecursive_VerifyInfoAlreadyInContext_VerifyInfoNotFilled()
    {
        //Arrange
        VerifyInfo verifyInfo = AnyVerifyInfo();
        FillingContext context = new();
        context.VerifyInfos.Add(verifyInfo.Id, verifyInfo);

        //Act
        await _sut.FillRecursive(verifyInfo, context, _fillersBagMock.Object);

        //Assert
        Assert.That(verifyInfo.Verifier, Is.Null);
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_VerifierAlreadyInContext_UseVerifierFromContext()
    {
        //Arrange
        User verifier = AnyUser();
        VerifyInfo verifyInfo = AnyVerifyInfo(verifier.Id);
        FillingContext context = new();
        context.Users.Add(verifier.Id, verifier);

        //Act
        await _sut.FillRecursive(verifyInfo, context, _fillersBagMock.Object);

        //Assert
        Assert.That(verifyInfo.Verifier, Is.EqualTo(verifier));
        _userRepositoryMock.Verify(x => x.GetById(It.IsAny<Guid>()), Times.Never());
    }

    [Test]
    public async Task FillRecursive_VerifierNotInContext_GetsVerifierAndFillsIt()
    {
        //Arrange
        User verifier = AnyUser();
        VerifyInfo verifyInfo = AnyVerifyInfo(verifier.Id);
        FillingContext context = new();
        _userRepositoryMock.Setup(x => x.GetById(verifier.Id)).ReturnsAsync(verifier);

        Mock<IUserFiller> userFillerMock = new Mock<IUserFiller>();
        _fillersBagMock.Setup(x => x.UserFiller).Returns(userFillerMock.Object);

        //Act
        await _sut.FillRecursive(verifyInfo, context, _fillersBagMock.Object);

        //Assert
        Assert.That(verifyInfo.Verifier, Is.EqualTo(verifier));
        _userRepositoryMock.Verify(x => x.GetById(verifier.Id), Times.Once());
        userFillerMock.Verify(x => x.FillRecursive(verifier, context, _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public void FillRecursive_VerifierNotInContextAndUnknownId_ThrowsException()
    {
        //Arrange
        VerifyInfo verifyInfo = AnyVerifyInfo();
        FillingContext context = new();

        //Act-Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _sut.FillRecursive(verifyInfo, context, _fillersBagMock.Object));
    }

    private static VerifyInfo AnyVerifyInfo(Guid? verifierId = null) => new VerifyInfo()
    {
        Id = Guid.NewGuid(),
        VerifierId = verifierId.HasValue ? verifierId.Value : Guid.NewGuid(),
        Comment = "",
        VerifyDate = DateTime.Now
    };

    private static User AnyUser() => new User()
    {
        Id = Guid.NewGuid(),
        Name = "",
        DriverId = Guid.NewGuid(),
        PasswordHash = "",
        Roles = [],
        Email = ""
    };
}
