
namespace RC2K.Resources.UnitTests;

public class StagesTests
{
    private readonly HashSet<string> _codes =
        [.. Enumerable.Range(21, 6)
            .Concat(Enumerable.Range(31, 6))
            .Concat(Enumerable.Range(41, 6))
            .Concat(Enumerable.Range(51, 6))
            .Concat(Enumerable.Range(61, 6))
            .Concat(Enumerable.Range(71, 6))
        .Select(x => x.ToString())];

    [SetUp]
    public void Setup()
    {
        // invoke static ctor
        _ = Stages.GetStages();
    }

    [Test]
    public void GetStages_ReturnsProperCount()
    {
        //Arrange-Act
        var stages = Stages.GetStages();

        //Assert
        Assert.That(stages, Has.Count.EqualTo(36));
    }

    [Test]
    public void GetWaypoints_ReturnsWaypointsForEachStage()
    {
        //Arrange-Act
        var stages = Stages.GetWaypoints();

        //Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(stages, Has.Count.EqualTo(36));
            Assert.That(stages.Select(x => x.Code).Intersect(_codes).ToList(), Has.Count.EqualTo(36));
        }
    }

    [Test]
    public void GetRallyInfos_ReturnsProperRalliesCount()
    {
        //Arrange-Act
        var rallies = Stages.GetRallyInfos();

        //Assert
        Assert.That(rallies, Has.Count.EqualTo(6));
    }
}