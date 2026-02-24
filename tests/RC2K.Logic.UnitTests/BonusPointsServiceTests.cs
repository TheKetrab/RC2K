using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;

namespace RC2K.Logic.UnitTests;

public class BonusPointsServiceTests
{
    private BonusPointsService _sut;
    private Mock<IBonusPointsRepository> _bonusPointsRepositoryMock;
    private Mock<IFillersBag> _fillersBagMock;
    private Mock<IBonusPointsFiller> _bonusPointsFillerMock;

    [SetUp]
    public void Setup()
    {
        _bonusPointsRepositoryMock = new Mock<IBonusPointsRepository>();
        _fillersBagMock = new Mock<IFillersBag>();
        _bonusPointsFillerMock = new Mock<IBonusPointsFiller>();

        _fillersBagMock.Setup(x => x.BonusPointsFiller).Returns(_bonusPointsFillerMock.Object);

        _sut = new(_bonusPointsRepositoryMock.Object, _fillersBagMock.Object);
    }

    [Test]
    public async Task GetAll_CallsBonusPointsRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<BonusPoints> bonusPoints = [];
        _bonusPointsRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(bonusPoints));

        //Act
        List<BonusPoints> result = await _sut.GetAll();

        //Assert
        _bonusPointsRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        Assert.That(result, Is.EqualTo(bonusPoints));
    }

    [Test]
    public async Task GetAll_CallsFillFullData()
    {
        //Arrange
        BonusPoints bp = new BonusPoints { Id = Guid.NewGuid(), DriverId = Guid.NewGuid() };
        List<BonusPoints> bonusPoints = [bp];
        _bonusPointsRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(bonusPoints));
        _bonusPointsFillerMock.Setup(x => x.FillRecursive(It.IsAny<BonusPoints>(), It.IsAny<FillingContext>(), _fillersBagMock.Object))
            .Returns(Task.CompletedTask);

        //Act
        await _sut.GetAll();

        //Assert
        _bonusPointsFillerMock.Verify(x => x.FillRecursive(bp, It.IsAny<FillingContext>(), _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public async Task Create_CallsBonusPointsRepositoryCreate()
    {
        //Arrange
        BonusPoints bonusPoints = new BonusPoints { Id = Guid.NewGuid(), DriverId = Guid.NewGuid() };

        //Act
        await _sut.Create(bonusPoints);

        //Assert
        _bonusPointsRepositoryMock.Verify(x => x.Create(bonusPoints), Times.Once());
    }
}
