namespace RC2K.Extensions;

public static class TimeExtensions
{

    public static int ToCentiseconds(this TimeOnly time) =>
        time.Hour * 3600 * 100 + time.Minute * 60 * 100 + time.Second * 100 + time.Millisecond / 10;

    public static TimeOnly ToTimeOnly(this int centiseconds)
    {
        int hours = centiseconds / 360000;
        centiseconds %= 360000;

        int minutes = centiseconds / 6000;
        centiseconds %= 6000;

        int seconds = centiseconds / 100;
        centiseconds %= 100;

        int milliseconds = centiseconds * 10;

        return new TimeOnly(hours, minutes, seconds, milliseconds);
    }

}
