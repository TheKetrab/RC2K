using Moq;
using RC2K.DataAccess.Interfaces;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Resources;

namespace RC2K.Logic.UnitTests;

public class CarServiceTests
{
    private CarService _sut;
    private Mock<IRallyUoW> _rallyUoWMock;
    private Mock<ICarRepository> _carRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _carRepositoryMock = new Mock<ICarRepository>();
        _rallyUoWMock = new Mock<IRallyUoW>();
        
        _rallyUoWMock.Setup(x => x.Cars).Returns(_carRepositoryMock.Object);

        _sut = new(_rallyUoWMock.Object);
    }

    [Test]
    public async Task GetAll_CallsCarRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<Car> cars = [];
        _carRepositoryMock.Setup(x => x.GetAll()).Returns(Task.FromResult(cars));

        //Act
        List<Car> result = await _sut.GetAll();

        //Assert
        _carRepositoryMock.Verify(x => x.GetAll(), Times.Once());
        Assert.That(result, Is.EqualTo(cars));
    }

    [Test]
    public async Task GetAllByClass_CallsCarRepositoryAndReturnsItsResult()
    {
        //Arrange
        int carClass = 5;
        List<Car> cars = [];
        _carRepositoryMock.Setup(x => x.GetAllByClass(carClass)).Returns(Task.FromResult(cars));

        //Act
        List<Car> result = await _sut.GetAllByClass(carClass);

        //Assert
        _carRepositoryMock.Verify(x => x.GetAllByClass(carClass), Times.Once());
        Assert.That(result, Is.EqualTo(cars));
    }

    [Test]
    public void IsA8InternalCarId_ReturnsTrueForA8Cars()
    {
        //Arrange
        var cars = Cars.GetAll().ToList();
        int id = cars.IndexOf(cars.Single(c => c.Name == "Seat Cordoba WRC"));

        //Act
        bool result = _sut.IsA8InternalCarId(id);

        //Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void IsA8InternalCarId_ReturnsFalseForNonA8Cars()
    {
        //Arrange
        var cars = Cars.GetAll().ToList();
        int id = cars.IndexOf(cars.Single(c => c.Name == "Nissan Almera"));

        //Act
        bool result = _sut.IsA8InternalCarId(id);

        //Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetBonusCars_CallsCarRepositoryAndReturnsItsResult()
    {
        //Arrange
        List<Car> cars = [];
        _carRepositoryMock.Setup(x => x.GetBonusCars()).Returns(Task.FromResult(cars));

        //Act
        List<Car> result = await _sut.GetBonusCars();

        //Assert
        _carRepositoryMock.Verify(x => x.GetBonusCars(), Times.Once());
        Assert.That(result, Is.EqualTo(cars));
    }
}
