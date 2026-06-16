using RC2K.DomainModel;

namespace RC2K.Extensions.UnitTests;

public class MoodExtensionsTests
{
    public void ParseMood_ParsesStringToFlags()
    {
        const string input = "sunrise;day;sunset;night;cloud;rain;snow;fog";

        var mood = MoodExtensions.ParseMood(input);

        Assert.That(mood, Is.EqualTo(
            Mood.Sunrise &
            Mood.Day &
            Mood.Sunset &
            Mood.Night &
            Mood.Cloud &
            Mood.Rain &
            Mood.Snow &
            Mood.Fog
        ));
    }
}
