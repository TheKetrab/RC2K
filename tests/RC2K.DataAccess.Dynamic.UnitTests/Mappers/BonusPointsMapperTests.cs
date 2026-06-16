using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests.Mappers;

public class BonusPointsMapperTests
{
    private BonusPointsMapper _bonusPointsMapper;

    [SetUp]
    public void Setup()
    {
        _bonusPointsMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        BonusPointsModel model = AnyBonusPointsModel();

        //Act
        var result = _bonusPointsMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.DriverId, Is.EqualTo(model.DriverId));
            Assert.That(result.Comment, Is.EqualTo(model.Comment));
            Assert.That(result.Points, Is.EqualTo(model.Points));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        BonusPoints bonusPoints = AnyBonusPoints();

        //Act
        var result = _bonusPointsMapper.ToCosmosModel(bonusPoints);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(bonusPoints.Id));
            Assert.That(result.DriverId, Is.EqualTo(bonusPoints.DriverId));
            Assert.That(result.Comment, Is.EqualTo(bonusPoints.Comment));
            Assert.That(result.Points, Is.EqualTo(bonusPoints.Points));
        }
    }

    [Test]
    public void ToDomainModel_WithNullComment_MapsProperly()
    {
        //Arrange
        BonusPointsModel model = new()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            Comment = null,
            Points = 100
        };

        //Act
        var result = _bonusPointsMapper.ToDomainModel(model);

        //Assert
        Assert.That(result.Comment, Is.Null);
    }

    private static BonusPointsModel AnyBonusPointsModel() => new()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Comment = "Good performance",
        Points = 50
    };

    private static BonusPoints AnyBonusPoints() => new()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Comment = "Good performance",
        Points = 50
    };
}
