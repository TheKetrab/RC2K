using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests.Mappers;

public class TimeEntryMapperTests
{
    private TimeEntryMapper _timeEntryMapper;

    [SetUp]
    public void Setup()
    {
        _timeEntryMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        TimeEntryModel model = AnyTimeEntryModel();

        //Act
        var result = _timeEntryMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.StageId, Is.EqualTo(model.StageId));
            Assert.That(result.CarId, Is.EqualTo(model.CarId));
            Assert.That(result.DriverId, Is.EqualTo(model.DriverId));
            Assert.That(result.Time.Hour, Is.EqualTo(1));
            Assert.That(result.Time.Minute, Is.EqualTo(30));
            Assert.That(result.Time.Second, Is.EqualTo(45));
            Assert.That(result.VerifyInfoId, Is.EqualTo(model.VerifyInfoId));
            Assert.That(result.Labels, Is.EqualTo(model.Labels));
            Assert.That(result.UploadTime.Date, Is.EqualTo(new DateTime(2024, 6, 15).Date));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        TimeEntry timeEntry = AnyTimeEntry();

        //Act
        var result = _timeEntryMapper.ToCosmosModel(timeEntry);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(timeEntry.Id));
            Assert.That(result.StageId, Is.EqualTo(timeEntry.StageId));
            Assert.That(result.CarId, Is.EqualTo(timeEntry.CarId));
            Assert.That(result.DriverId, Is.EqualTo(timeEntry.DriverId));
            Assert.That(result.Time, Is.EqualTo(544500)); // 1:30:45 in centiseconds = 1*3600*100 + 30*60*100 + 45*100 = 544500
            Assert.That(result.VerifyInfoId, Is.EqualTo(timeEntry.VerifyInfoId));
            Assert.That(result.Labels, Is.EqualTo(timeEntry.Labels));
            Assert.That(result.UploadTime, Is.Not.Empty);
        }
    }

    [Test]
    public void ToDomainModel_WithProofs_MapsCorrectly()
    {
        //Arrange
        TimeEntryModel model = new TimeEntryModel()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            StageId = 1,
            CarId = 8,
            DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            Time = 544500,
            UploadTime = "15/06/2024",
            VerifyInfoId = Guid.Parse("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
            Labels = "test",
            Proofs = new List<string> { "i|https://example.com/image.jpg", "y|https://youtube.com/watch?v=123" }
        };

        //Act
        var result = _timeEntryMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Proofs, Has.Count.EqualTo(2));
            Assert.That(result.Proofs[0].Type, Is.EqualTo(ProofType.Image));
            Assert.That(result.Proofs[0].Url, Is.EqualTo("https://example.com/image.jpg"));
            Assert.That(result.Proofs[1].Type, Is.EqualTo(ProofType.Youtube));
            Assert.That(result.Proofs[1].Url, Is.EqualTo("https://youtube.com/watch?v=123"));
        }
    }

    [Test]
    public void ToCosmosModel_WithProofs_MapsCorrectly()
    {
        //Arrange
        TimeEntry timeEntry = new TimeEntry()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            StageId = 1,
            CarId = 8,
            DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            Time = new TimeOnly(1, 30, 45),
            UploadTime = new DateTime(2024, 6, 15, 10, 30, 0),
            VerifyInfoId = Guid.Parse("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
            Labels = "test",
            Proofs = new List<Proof>
            {
                new Proof { Type = ProofType.Image, Url = "https://example.com/image.jpg" },
                new Proof { Type = ProofType.Replay, Url = "https://example.com/replay.bin" }
            }
        };

        //Act
        var result = _timeEntryMapper.ToCosmosModel(timeEntry);

        //Assert
        Assert.That(result.Proofs, Is.Not.Null);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Proofs!, Has.Count.EqualTo(2));
            Assert.That(result.Proofs![0], Is.EqualTo("i|https://example.com/image.jpg"));
            Assert.That(result.Proofs![1], Is.EqualTo("r|https://example.com/replay.bin"));
        }
    }

    [Test]
    public void ToDomainModel_WithoutProofs_MapsCorrectly()
    {
        //Arrange
        TimeEntryModel model = AnyTimeEntryModel();
        model.Proofs = null;

        //Act
        var result = _timeEntryMapper.ToDomainModel(model);

        //Assert
        Assert.That(result.Proofs, Is.Empty);
    }

    [Test]
    public void ToCosmosModel_WithoutProofs_DoesNotIncludeProofsProperty()
    {
        //Arrange
        TimeEntry timeEntry = AnyTimeEntry();

        //Act
        var result = _timeEntryMapper.ToCosmosModel(timeEntry);

        //Assert
        Assert.That(result.Proofs, Is.Null);
    }

    [Test]
    public void ToDomainModel_WithNullVerifyInfoId_MapsCorrectly()
    {
        //Arrange
        TimeEntryModel model = AnyTimeEntryModel();
        model.VerifyInfoId = null;

        //Act
        var result = _timeEntryMapper.ToDomainModel(model);

        //Assert
        Assert.That(result.VerifyInfoId, Is.Null);
    }

    private static TimeEntryModel AnyTimeEntryModel() => new TimeEntryModel()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        StageId = 1,
        CarId = 8,
        DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Time = 544500,
        UploadTime = "15/06/2024",
        VerifyInfoId = Guid.Parse("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
        Labels = "test label",
        Proofs = null
    };

    private static TimeEntry AnyTimeEntry() => new TimeEntry()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        StageId = 1,
        CarId = 8,
        DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Time = new TimeOnly(1, 30, 45),
        UploadTime = new DateTime(2024, 6, 15, 10, 30, 0),
        VerifyInfoId = Guid.Parse("5a5a5a5a-5a5a-5a5a-5a5a-5a5a5a5a5a5a"),
        Labels = "test label"
    };
}
