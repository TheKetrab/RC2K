using RC2K.DomainModel;
using RC2K.Utils;

namespace RC2K.Extensions.UnitTests;

public class LevelHelperTests
{
    [TestCase(false, 11)]
    [TestCase(true, 21)]
    public void GetStageCode_UsesProperDictionary(bool isArcade, int expectedResult) =>
        Assert.That(LevelHelper.GetStageCode(1, 1, isArcade), Is.EqualTo(expectedResult));

    [Test]
    public void GetStageCode_UsesProperDictionary() =>
        Assert.That(LevelHelper.GetStageCode(RallyCode.Pirelli, 2), Is.EqualTo(62));

    [Test]
    public void GetRallyCodeByStageCode_UsesProperDictionary() =>
        Assert.That(LevelHelper.GetRallyCodeByStageCode(23), Is.EqualTo(RallyCode.Sony));

    [Test]
    public void RallyCodeToRallyName_UsesProperDictionary() =>
        Assert.That(LevelHelper.RallyCodeToRallyName(RallyCode.Seat), Is.EqualTo("SEAT Jim Clark Memorial Rally"));

    [Test]
    public void RallyCodeToRallyImageName_UsesProperDictionary() =>
        Assert.That(LevelHelper.RallyCodeToRallyImageName(RallyCode.Seat), Is.EqualTo("WELJC"));

    [Test]
    public void RallyCodeToRallyShortName_UsesProperDictionary() =>
        Assert.That(LevelHelper.RallyCodeToRallyShortName(RallyCode.Stena), Is.EqualTo("stena"));

    [Test]
    public void StageCodeToRallyName_UsesProperDictionary() =>
        Assert.That(LevelHelper.StageCodeToRallyName(42), Is.EqualTo("Vauxhall Rally of Wales"));

    [Test]
    public void StageCodeToRallyImageName_UsesProperDictionary() =>
        Assert.That(LevelHelper.StageCodeToRallyImageName(62), Is.EqualTo("WELPIR"));


}
