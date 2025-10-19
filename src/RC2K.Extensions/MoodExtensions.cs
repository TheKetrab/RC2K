using RC2K.DomainModel;

namespace RC2K.Extensions;

public static class MoodExtensions
{
    public static Mood ParseMood(this string mood)
    {
        Mood m = new();
        var split = mood.Split(";");
        if (split.Contains("sunrise")) m |= Mood.Sunrise;
        if (split.Contains("day")) m |= Mood.Day;
        if (split.Contains("sunset")) m |= Mood.Sunset;
        if (split.Contains("night")) m |= Mood.Night;
        if (split.Contains("cloud")) m |= Mood.Cloud;
        if (split.Contains("rain")) m |= Mood.Rain;
        if (split.Contains("snow")) m |= Mood.Snow;
        if (split.Contains("fog")) m |= Mood.Fog;
        return m;
    }
}
