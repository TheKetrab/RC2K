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
    public void CentisecondsToTimeOnly_GivenCentiseconds_ReturnsTimeOnly()
    {
        //Arrange
        const int centiseconds = 372_307;

        //Act
        var result = centiseconds.ToTimeOnly();

        //Assert
        Assert.That(result, Is.EqualTo(new TimeOnly(1,2,3,70)));
    }

    [Test]
    public void TimeSpanToCentiseconds_GivenTimeOnly_ReturnsCentiseconds()
    {
        //Arrange
        TimeSpan timeSpan = new(1, 2, 3, 70);

        //Act
        var result = timeSpan.ToCentiseconds();

        //Assert
        Assert.That(result, Is.EqualTo(372_307));
    }

    [Test]
    public void CentisecondsToTimeSpan_GivenCentiseconds_ReturnsTimeSpan()
    {
        //Arrange
        const int centiseconds = 372_307;

        //Act
        var result = centiseconds.ToTimeSpan();

        //Assert
        Assert.That(result, Is.EqualTo(new TimeSpan(1, 2, 3, 70)));
    }
}
