namespace RC2K.Extensions.UnitTests;

public class TimeExtensionsTests
{
    [Test]
    public void TimeOnlyToCentiseconds_GivenTimeOnly_ReturnsCentiseconds()
    {
        //Arrange
        TimeOnly timeOnly = new(1, 2, 3, 70);

        //Act
        var result = timeOnly.ToCentiseconds();

        //Assert
        Assert.That(result, Is.EqualTo(372_307));
    }

    [Test]
    public void CentisecondsToTimeOnyl_GivenCentiseconds_ReturnsTimeOnly()
    {
        //Arrange
        const int centiseconds = 372_307;

        //Act
        var result = centiseconds.ToTimeOnly();

        //Assert
        Assert.That(result, Is.EqualTo(new TimeOnly(1,2,3,70)));
    }
}
