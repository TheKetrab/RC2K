using RC2K.DataAccess.Dynamic.Mappers;
using RC2K.DataAccess.Dynamic.Models;
using RC2K.DomainModel;

namespace RC2K.DataAccess.Dynamic.UnitTests.Mappers;

public class RankingsMapperTests
{
    private RankingsMapper _rankingsMapper;

    [SetUp]
    public void Setup()
    {
        _rankingsMapper = new();
    }

    [Test]
    public void ToDomainModel_MapsProperly()
    {
        //Arrange
        RankingSnapshotModel model = AnyRankingSnapshotModel();

        //Act
        var result = _rankingsMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(model.Id));
            Assert.That(result.Date.Year, Is.EqualTo(2024));
            Assert.That(result.Date.Month, Is.EqualTo(6));
            Assert.That(result.Date.Day, Is.EqualTo(15));
            Assert.That(result.Entries, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public void ToDomainModel_MapsEntryPropertiesCorrectly()
    {
        //Arrange
        RankingSnapshotModel model = AnyRankingSnapshotModel();

        //Act
        var result = _rankingsMapper.ToDomainModel(model);

        //Assert
        var firstEntry = result.Entries[0];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstEntry.Place, Is.EqualTo(1));
            Assert.That(firstEntry.DriverId, Is.EqualTo(Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3")));
            Assert.That(firstEntry.GeneralPoints, Is.EqualTo(100));
            Assert.That(firstEntry.GeneralTop30Count, Is.EqualTo(5));
            Assert.That(firstEntry.GeneralTop10Count, Is.EqualTo(3));
            Assert.That(firstEntry.GeneralTop3Count, Is.EqualTo(2));
            Assert.That(firstEntry.GeneralTop1Count, Is.EqualTo(1));
            Assert.That(firstEntry.CarA8Points, Is.EqualTo(50));
            Assert.That(firstEntry.CarA8PointsTop5Count, Is.EqualTo(2));
            Assert.That(firstEntry.CarA8PointsTop1Count, Is.Zero);
            Assert.That(firstEntry.CarA7Points, Is.EqualTo(40));
            Assert.That(firstEntry.CarA7PointsTop5Count, Is.EqualTo(1));
            Assert.That(firstEntry.CarA7PointsTop1Count, Is.Zero);
            Assert.That(firstEntry.CarA6Points, Is.EqualTo(30));
            Assert.That(firstEntry.CarA6PointsTop5Count, Is.EqualTo(1));
            Assert.That(firstEntry.CarA6PointsTop1Count, Is.Zero);
            Assert.That(firstEntry.CarA5Points, Is.EqualTo(20));
            Assert.That(firstEntry.CarA5PointsTop5Count, Is.EqualTo(1));
            Assert.That(firstEntry.CarA5PointsTop1Count, Is.Zero);
            Assert.That(firstEntry.CarBonusPoints, Is.EqualTo(10));
            Assert.That(firstEntry.CarBonusPointsTop5Count, Is.Zero);
            Assert.That(firstEntry.CarBonusPointsTop1Count, Is.Zero);
            Assert.That(firstEntry.BonusPoints, Is.EqualTo(5));
        }
    }

    [Test]
    public void ToCosmosModel_MapsProperly()
    {
        //Arrange
        RankingSnapshot rankingSnapshot = AnyRankingSnapshot();

        //Act
        var result = _rankingsMapper.ToCosmosModel(rankingSnapshot);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(rankingSnapshot.Id));
            Assert.That(result.Type, Is.EqualTo("ranking"));
            Assert.That(result.Entries, Has.Count.EqualTo(2));
        }
    }

    [Test]
    public void ToCosmosModel_SerializesEntryDataCorrectly()
    {
        //Arrange
        RankingSnapshot rankingSnapshot = AnyRankingSnapshot();

        //Act
        var result = _rankingsMapper.ToCosmosModel(rankingSnapshot);

        //Assert
        var firstEntry = result.Entries[0];
        using (Assert.EnterMultipleScope())
        {
            Assert.That(firstEntry.Place, Is.EqualTo(1));
            Assert.That(firstEntry.DriverId, Is.EqualTo(Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3")));
            string expectedData = "100,5,3,2,1,50,2,0,40,1,0,30,1,0,20,1,0,10,0,0,5";
            Assert.That(firstEntry.Data, Is.EqualTo(expectedData));
        }
    }

    [Test]
    public void RoundTrip_FromDomainToCosmosAndBack_PreservesData()
    {
        //Arrange
        RankingSnapshot original = AnyRankingSnapshot();

        //Act
        RankingSnapshotModel model = _rankingsMapper.ToCosmosModel(original);
        RankingSnapshot restored = _rankingsMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(restored.Id, Is.EqualTo(original.Id));
            Assert.That(restored.Date.Date, Is.EqualTo(original.Date.Date));
            Assert.That(restored.Entries, Has.Count.EqualTo(original.Entries.Count));

            var originalFirstEntry = original.Entries[0];
            var restoredFirstEntry = restored.Entries[0];

            Assert.That(restoredFirstEntry.Place, Is.EqualTo(originalFirstEntry.Place));
            Assert.That(restoredFirstEntry.DriverId, Is.EqualTo(originalFirstEntry.DriverId));
            Assert.That(restoredFirstEntry.GeneralPoints, Is.EqualTo(originalFirstEntry.GeneralPoints));
            Assert.That(restoredFirstEntry.CarA8Points, Is.EqualTo(originalFirstEntry.CarA8Points));
            Assert.That(restoredFirstEntry.BonusPoints, Is.EqualTo(originalFirstEntry.BonusPoints));
        }
    }

    [Test]
    public void ToDomainModel_WithMultipleEntries_MapsAllCorrectly()
    {
        //Arrange
        RankingSnapshotModel model = AnyRankingSnapshotModel();

        //Act
        var result = _rankingsMapper.ToDomainModel(model);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Entries, Has.Count.EqualTo(2));

            // Second entry
            var secondEntry = result.Entries[1];
            Assert.That(secondEntry.Place, Is.EqualTo(2));
            Assert.That(secondEntry.DriverId, Is.EqualTo(Guid.Parse("2b91f15a-62ea-539c-9c16-1b948c6dd5d4")));
            Assert.That(secondEntry.GeneralPoints, Is.EqualTo(90));
            Assert.That(secondEntry.BonusPoints, Is.EqualTo(3));
        }
    }

    [Test]
    public void ToCosmosModel_WithEmptyEntries_MapsCorrectly()
    {
        //Arrange
        RankingSnapshot rankingSnapshot = new()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            Date = new DateTime(2024, 6, 15, 10, 30, 0)
        };

        //Act
        var result = _rankingsMapper.ToCosmosModel(rankingSnapshot);

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(result.Id, Is.EqualTo(rankingSnapshot.Id));
            Assert.That(result.Entries, Is.Empty);
        }
    }

    private static RankingSnapshotModel AnyRankingSnapshotModel()
    {
        string entryData1 = "100,5,3,2,1,50,2,0,40,1,0,30,1,0,20,1,0,10,0,0,5";
        string entryData2 = "90,4,2,1,0,45,1,0,35,0,0,25,0,0,15,0,0,8,0,0,3";

        return new RankingSnapshotModel()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            Date = "15/06/2024",
            Entries = new List<RankingEntryModel>
            {
                new()
                {
                    Place = 1,
                    DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
                    Data = entryData1
                },
                new()
                {
                    Place = 2,
                    DriverId = Guid.Parse("2b91f15a-62ea-539c-9c16-1b948c6dd5d4"),
                    Data = entryData2
                }
            }
        };
    }

    private static RankingSnapshot AnyRankingSnapshot()
    {
        var snapshot = new RankingSnapshot()
        {
            Id = Guid.Parse("3258b9d9-43f9-4e00-8605-0d739b5cc791"),
            Date = new DateTime(2024, 6, 15, 10, 30, 0)
        };

        snapshot.Entries.Add(new RankingEntry()
        {
            Place = 1,
            DriverId = Guid.Parse("1a80e049-51d9-428b-ad32-08a037ecc4c3"),
            GeneralPoints = 100,
            GeneralTop30Count = 5,
            GeneralTop10Count = 3,
            GeneralTop3Count = 2,
            GeneralTop1Count = 1,
            CarA8Points = 50,
            CarA8PointsTop5Count = 2,
            CarA8PointsTop1Count = 0,
            CarA7Points = 40,
            CarA7PointsTop5Count = 1,
            CarA7PointsTop1Count = 0,
            CarA6Points = 30,
            CarA6PointsTop5Count = 1,
            CarA6PointsTop1Count = 0,
            CarA5Points = 20,
            CarA5PointsTop5Count = 1,
            CarA5PointsTop1Count = 0,
            CarBonusPoints = 10,
            CarBonusPointsTop5Count = 0,
            CarBonusPointsTop1Count = 0,
            BonusPoints = 5
        });

        snapshot.Entries.Add(new RankingEntry()
        {
            Place = 2,
            DriverId = Guid.Parse("2b91f15a-62ea-539c-9c16-1b948c6dd5d4"),
            GeneralPoints = 90,
            GeneralTop30Count = 4,
            GeneralTop10Count = 2,
            GeneralTop3Count = 1,
            GeneralTop1Count = 0,
            CarA8Points = 45,
            CarA8PointsTop5Count = 1,
            CarA8PointsTop1Count = 0,
            CarA7Points = 35,
            CarA7PointsTop5Count = 0,
            CarA7PointsTop1Count = 0,
            CarA6Points = 25,
            CarA6PointsTop5Count = 0,
            CarA6PointsTop1Count = 0,
            CarA5Points = 15,
            CarA5PointsTop5Count = 0,
            CarA5PointsTop1Count = 0,
            CarBonusPoints = 8,
            CarBonusPointsTop5Count = 0,
            CarBonusPointsTop1Count = 0,
            BonusPoints = 3
        });

        return snapshot;
    }
}
