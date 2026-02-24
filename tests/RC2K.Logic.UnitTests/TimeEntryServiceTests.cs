using Moq;
using RC2K.DataAccess.Interfaces.Repositories;
using RC2K.DomainModel;
using RC2K.Logic.Interfaces;
using RC2K.Logic.Interfaces.Fillers;
using Microsoft.Extensions.Logging;

namespace RC2K.Logic.UnitTests;

public class TimeEntryServiceTests
{
    private TimeEntryService _sut;
    private Mock<ITimeEntryRepository> _timeEntryRepositoryMock;
    private Mock<IVerifyInfoRepository> _verifyInfoRepositoryMock;
    private Mock<IPointsProvider> _pointsProviderMock;
    private Mock<IFillersBag> _fillersBagMock;
    private Mock<ILogger<TimeEntryService>> _loggerMock;
    private Mock<ITimeEntryFiller> _timeEntryFillerMock;

    [SetUp]
    public void Setup()
    {
        _timeEntryRepositoryMock = new Mock<ITimeEntryRepository>();
        _verifyInfoRepositoryMock = new Mock<IVerifyInfoRepository>();
        _pointsProviderMock = new Mock<IPointsProvider>();
        _fillersBagMock = new Mock<IFillersBag>();
        _loggerMock = new Mock<ILogger<TimeEntryService>>();
        _timeEntryFillerMock = new Mock<ITimeEntryFiller>();

        _fillersBagMock.Setup(x => x.TimeEntryFiller).Returns(_timeEntryFillerMock.Object);

        _sut = new(_timeEntryRepositoryMock.Object, _verifyInfoRepositoryMock.Object, 
                   _pointsProviderMock.Object, _fillersBagMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Get_WithoutCarId_CallsRepositoryAndFillsData()
    {
        //Arrange
        int stageId = 1;
        List<TimeEntry> timeEntries = [];
        _timeEntryRepositoryMock.Setup(x => x.GetByStageId(stageId)).Returns(Task.FromResult(timeEntries));
        _timeEntryFillerMock.Setup(x => x.FillRecursive(It.IsAny<TimeEntry>(), It.IsAny<FillingContext>(), _fillersBagMock.Object))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.Get(stageId);

        //Assert
        _timeEntryRepositoryMock.Verify(x => x.GetByStageId(stageId), Times.Once());
        Assert.That(result, Is.EqualTo(timeEntries));
    }

    [Test]
    public async Task Get_WithoutCarId_CallsRepositoryAndFillsData_WithData()
    {
        //Arrange
        int stageId = 1;
        TimeEntry timeEntry = new TimeEntry 
        { 
            Id = Guid.NewGuid(), 
            StageId = stageId,
            CarId = 5,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 30),
            UploadTime = DateTime.UtcNow
        };
        List<TimeEntry> timeEntries = [timeEntry];
        _timeEntryRepositoryMock.Setup(x => x.GetByStageId(stageId)).Returns(Task.FromResult(timeEntries));
        _timeEntryFillerMock.Setup(x => x.FillRecursive(It.IsAny<TimeEntry>(), It.IsAny<FillingContext>(), _fillersBagMock.Object))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.Get(stageId);

        //Assert
        _timeEntryRepositoryMock.Verify(x => x.GetByStageId(stageId), Times.Once());
        Assert.That(result, Is.EqualTo(timeEntries));
    }

    [Test]
    public async Task Get_WithCarId_CallsRepositoryWithCarIdAndFillsData()
    {
        //Arrange
        int stageId = 1;
        int carId = 5;
        List<TimeEntry> timeEntries = [];
        _timeEntryRepositoryMock.Setup(x => x.GetByStageIdAndCarId(stageId, carId)).Returns(Task.FromResult(timeEntries));
        _timeEntryFillerMock.Setup(x => x.FillRecursive(It.IsAny<TimeEntry>(), It.IsAny<FillingContext>(), _fillersBagMock.Object))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.Get(stageId, carId);

        //Assert
        _timeEntryRepositoryMock.Verify(x => x.GetByStageIdAndCarId(stageId, carId), Times.Once());
        Assert.That(result, Is.EqualTo(timeEntries));
    }

    [Test]
    public async Task Get_CallsFillFullData()
    {
        //Arrange
        int stageId = 1;
        TimeEntry timeEntry = new TimeEntry 
        { 
            Id = Guid.NewGuid(), 
            StageId = stageId,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 30),
            UploadTime = DateTime.UtcNow
        };
        List<TimeEntry> timeEntries = [timeEntry];
        _timeEntryRepositoryMock.Setup(x => x.GetByStageId(stageId)).Returns(Task.FromResult(timeEntries));
        _timeEntryFillerMock.Setup(x => x.FillRecursive(It.IsAny<TimeEntry>(), It.IsAny<FillingContext>(), _fillersBagMock.Object))
            .Returns(Task.CompletedTask);

        //Act
        await _sut.Get(stageId);

        //Assert
        _timeEntryFillerMock.Verify(x => x.FillRecursive(timeEntry, It.IsAny<FillingContext>(), _fillersBagMock.Object), Times.Once());
    }

    [Test]
    public async Task GetAllNotVerified_CallsRepositoryAndFillsData()
    {
        //Arrange
        List<TimeEntry> timeEntries = [];
        _timeEntryRepositoryMock.Setup(x => x.GetAllNotVerified()).Returns(Task.FromResult(timeEntries));
        _timeEntryFillerMock.Setup(x => x.FillRecursive(It.IsAny<TimeEntry>(), It.IsAny<FillingContext>(), _fillersBagMock.Object))
            .Returns(Task.CompletedTask);

        //Act
        var result = await _sut.GetAllNotVerified();

        //Assert
        _timeEntryRepositoryMock.Verify(x => x.GetAllNotVerified(), Times.Once());
        Assert.That(result, Is.EqualTo(timeEntries));
    }

    [Test]
    public async Task Delete_DeletesAllTimeEntries()
    {
        //Arrange
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();
        TimeEntry te1 = new TimeEntry 
        { 
            Id = id1,
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 30),
            UploadTime = DateTime.UtcNow
        };
        TimeEntry te2 = new TimeEntry 
        { 
            Id = id2,
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 40),
            UploadTime = DateTime.UtcNow
        };
        List<TimeEntry> timeEntries = [te1, te2];
        
        _timeEntryRepositoryMock.Setup(x => x.Delete(id1.ToString())).Returns(Task.CompletedTask);
        _timeEntryRepositoryMock.Setup(x => x.Delete(id2.ToString())).Returns(Task.CompletedTask);

        //Act
        await _sut.Delete(timeEntries);

        //Assert
        _timeEntryRepositoryMock.Verify(x => x.Delete(id1.ToString()), Times.Once());
        _timeEntryRepositoryMock.Verify(x => x.Delete(id2.ToString()), Times.Once());
    }

    [Test]
    public async Task Delete_ContinuesOnError()
    {
        //Arrange
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();
        TimeEntry te1 = new TimeEntry 
        { 
            Id = id1,
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 30),
            UploadTime = DateTime.UtcNow
        };
        TimeEntry te2 = new TimeEntry 
        { 
            Id = id2,
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 40),
            UploadTime = DateTime.UtcNow
        };
        List<TimeEntry> timeEntries = [te1, te2];
        
        _timeEntryRepositoryMock.Setup(x => x.Delete(id1.ToString())).ThrowsAsync(new Exception("Delete failed"));
        _timeEntryRepositoryMock.Setup(x => x.Delete(id2.ToString())).Returns(Task.CompletedTask);

        //Act
        await _sut.Delete(timeEntries);

        //Assert
        _timeEntryRepositoryMock.Verify(x => x.Delete(id1.ToString()), Times.Once());
        _timeEntryRepositoryMock.Verify(x => x.Delete(id2.ToString()), Times.Once());
    }

    [Test]
    public async Task Verify_CreatesVerifyInfoAndUpdatesTimeEntries()
    {
        //Arrange
        Guid verifierId = Guid.NewGuid();
        string comment = "Test comment";
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();
        TimeEntry te1 = new TimeEntry 
        { 
            Id = id1,
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 30),
            UploadTime = DateTime.UtcNow
        };
        TimeEntry te2 = new TimeEntry 
        { 
            Id = id2,
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 40),
            UploadTime = DateTime.UtcNow
        };
        List<TimeEntry> timeEntries = [te1, te2];
        
        _verifyInfoRepositoryMock.Setup(x => x.Create(It.IsAny<VerifyInfo>())).Returns(Task.CompletedTask);
        _timeEntryRepositoryMock.Setup(x => x.Update(It.IsAny<TimeEntry>())).Returns(Task.CompletedTask);

        //Act
        await _sut.Verify(timeEntries, verifierId, comment);

        //Assert
        _verifyInfoRepositoryMock.Verify(x => x.Create(It.Is<VerifyInfo>(v => 
            v.VerifierId == verifierId && v.Comment == comment)), Times.Once());
        _timeEntryRepositoryMock.Verify(x => x.Update(te1), Times.Once());
        _timeEntryRepositoryMock.Verify(x => x.Update(te2), Times.Once());
        Assert.That(te1.VerifyInfoId, Is.Not.Null);
        Assert.That(te2.VerifyInfoId, Is.Not.Null);
        Assert.That(te1.VerifyInfoId, Is.EqualTo(te2.VerifyInfoId));
    }

    [Test]
    public void Verify_ThrowsWhenTimeEntryAlreadyVerified()
    {
        //Arrange
        Guid verifierId = Guid.NewGuid();
        string comment = "Test comment";
        Guid existingVerifyInfoId = Guid.NewGuid();
        TimeEntry te1 = new TimeEntry 
        { 
            Id = Guid.NewGuid(),
            StageId = 1,
            CarId = 1,
            DriverId = Guid.NewGuid(),
            Time = new TimeOnly(0, 5, 30),
            UploadTime = DateTime.UtcNow,
            VerifyInfoId = existingVerifyInfoId 
        };
        List<TimeEntry> timeEntries = [te1];

        //Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _sut.Verify(timeEntries, verifierId, comment));
    }

    [Test]
    public async Task Upload_CreatesTimeEntry()
    {
        //Arrange
        int stageId = 1;
        int carId = 5;
        Guid driverId = Guid.NewGuid();
        int min = 5;
        int sec = 30;
        int cc = 50;
        List<Proof> proofs = [];
        string labels = "test";
        
        _timeEntryRepositoryMock.Setup(x => x.GetByStageIdAndCarIdAndDriverIdAndTime(
            stageId, carId, driverId, It.IsAny<TimeOnly>())).Returns(Task.FromResult(new List<TimeEntry>()));
        _timeEntryRepositoryMock.Setup(x => x.Create(It.IsAny<TimeEntry>())).Returns(Task.CompletedTask);

        //Act
        var result = await _sut.Upload(stageId, carId, driverId, min, sec, cc, proofs, labels);

        //Assert
        Assert.That(result.Success, Is.True);
        _timeEntryRepositoryMock.Verify(x => x.Create(It.IsAny<TimeEntry>()), Times.Once());
    }

}
