using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests;

public class VerifyInfoMapperTests
{
    private VerifyInfoMapper _verifyInfoMapper;

    [SetUp]
    public void Setup()
    {
        _verifyInfoMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        VerifyInfoModel model = AnyVerifyInfoModel();

        //Act
        var result = _verifyInfoMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.VerifierId, Is.EqualTo(model.VerifierId));
            Assert.That(result.Comment, Is.EqualTo(model.Comment));
            Assert.That(result.VerifyDate.Year, Is.EqualTo(2024));
            Assert.That(result.VerifyDate.Month, Is.EqualTo(6));
            Assert.That(result.VerifyDate.Day, Is.EqualTo(15));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        VerifyInfo verifyInfo = AnyVerifyInfo();

        //Act
        var result = _verifyInfoMapper.ToCosmosModel(verifyInfo);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(verifyInfo.Id));
            Assert.That(result.VerifierId, Is.EqualTo(verifyInfo.VerifierId));
            Assert.That(result.Comment, Is.EqualTo(verifyInfo.Comment));
            Assert.That(result.VerifyDate, Is.Not.Empty);
        }
    }

    [Test]
    public void ToDomainModel_WithNullComment_MapsProperly()
    {
        //Arrange
        VerifyInfoModel model = new VerifyInfoModel()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            VerifierId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            Comment = null,
            VerifyDate = "15/06/2024"
        };

        //Act
        var result = _verifyInfoMapper.ToDomainModel(model);

        //Assert
        Assert.That(result.Comment, Is.Null);
    }

    [Test]
    public void RoundTrip_FromDomainToCosmosAndBack_PreservesData()
    {
        //Arrange
        VerifyInfo original = AnyVerifyInfo();

        //Act
        VerifyInfoModel model = _verifyInfoMapper.ToCosmosModel(original);
        VerifyInfo restored = _verifyInfoMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(restored.Id, Is.EqualTo(original.Id));
            Assert.That(restored.VerifierId, Is.EqualTo(original.VerifierId));
            Assert.That(restored.Comment, Is.EqualTo(original.Comment));
            Assert.That(restored.VerifyDate.Date, Is.EqualTo(original.VerifyDate.Date));
        }
    }

    [Test]
    public void ToCosmosModel_WithDifferentComment_MapsProperly()
    {
        //Arrange
        VerifyInfo verifyInfo = new VerifyInfo()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            VerifierId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            Comment = "Verified successfully with clear proof",
            VerifyDate = new DateTime(2024, 6, 15, 10, 30, 0)
        };

        //Act
        var result = _verifyInfoMapper.ToCosmosModel(verifyInfo);

        //Assert
        Assert.That(result.Comment, Is.EqualTo("Verified successfully with clear proof"));
    }

    private static VerifyInfoModel AnyVerifyInfoModel() => new VerifyInfoModel()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        VerifierId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Comment = "Good time submission",
        VerifyDate = "15/06/2024"
    };

    private static VerifyInfo AnyVerifyInfo() => new VerifyInfo()
    {
        Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
        VerifierId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
        Comment = "Good time submission",
        VerifyDate = new DateTime(2024, 6, 15, 10, 30, 0)
    };
}
