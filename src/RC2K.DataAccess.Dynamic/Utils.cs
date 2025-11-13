using RC2K.DomainModel;
using System.Globalization;

namespace RC2K.DataAccess.Dynamic;

internal static class Utils
{
    public static int TimeOnlyToCentiseconds(TimeOnly time) =>
        time.Hour * 3600 + time.Minute * 60 + time.Millisecond / 10;

    public static TimeOnly CentisecondsToTimeOnly(int centiseconds)
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

    public static string DateTimeToString(DateTime dt) =>
        dt.ToString("dd/MM/yyyy");

    public static DateTime StringToDateTime(string str) =>
        DateTime.ParseExact(str, "dd/MM/yyyy", CultureInfo.InvariantCulture);

    public static char DirectionToChar(Direction direction) => direction switch
    {
        Direction.Simulation => 's',
        Direction.Arcade => 'a',
        _ => throw new ArgumentException()
    };


    public static Direction CharToDirection(char c) => c switch
    {
        's' => Direction.Simulation,
        'a' => Direction.Arcade,
        _ => throw new ArgumentException()
    };

    public static Proof DeserializeProof(string str)
    {
        int idx = str.IndexOf('|');
        if (idx != 1)
        {
            return new Proof() { Url = str };
        }

        ProofType type = str[0] switch
        {
            'i' => ProofType.Image,
            't' => ProofType.Twitch,
            'y' => ProofType.Youtube,
            'r' => ProofType.Replay,
            _ => ProofType.Unknown
        };

        return new Proof() { Url = str.Substring(2), Type = type };
    }

    public static string SerializeProof(Proof proof)
    {
        return $"{(char)proof.Type}|{proof.Url}";
    }

}
