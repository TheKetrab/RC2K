using RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

public static class Mfmi26Helper
{
    private static readonly Dictionary<string, int> _name2nr = 
        ContestsDefinitions.Instance.Mfmi26.Participants.ToDictionary(x => x.Name, x => x.Nr);

    private static readonly Dictionary<string, int> _name2group =
        ContestsDefinitions.Instance.Mfmi26.Participants.ToDictionary(x => x.Name, x => x.Group);

    private static readonly Dictionary<string, string> _name2car =
        ContestsDefinitions.Instance.Mfmi26.Participants.ToDictionary(x => x.Name, x => x.Car);

    public static int GetNr(string driver) =>
        _name2nr.TryGetValue(driver, out int res) ? res : -1;

    public static string GetGroup(string driver) => 
        _name2group.TryGetValue(driver, out int res) ? res.ToString() : "?";

    public static string GetCar(string driver) =>
        _name2car.TryGetValue(driver, out string? res) ? (res ?? "?") : "?";

    public static int GetCompetitionDay()
    {
        TimeZoneInfo polishZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime plNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, polishZone);

        if (plNow <= UtcFromPolishTime(2026, 11, 21, 20)) return -1; // not started

        if (plNow <= UtcFromPolishTime(2026, 11, 22, 20)) return 1;
        if (plNow <= UtcFromPolishTime(2026, 11, 23, 20)) return 2;
        if (plNow <= UtcFromPolishTime(2026, 11, 24, 20)) return 3;

        if (plNow <= UtcFromPolishTime(2026, 11, 26, 20)) return 4;
        if (plNow <= UtcFromPolishTime(2026, 11, 27, 20)) return 5;
        if (plNow <= UtcFromPolishTime(2026, 11, 28, 20)) return 6;
        if (plNow <= UtcFromPolishTime(2026, 11, 29, 20)) return 7;
        if (plNow <= UtcFromPolishTime(2026, 11, 30, 20)) return 8;

        if (plNow <= UtcFromPolishTime(2026, 12, 2, 20)) return 9;
        if (plNow <= UtcFromPolishTime(2026, 12, 3, 20)) return 10;
        if (plNow <= UtcFromPolishTime(2026, 12, 4, 20)) return 11;

        if (plNow <= UtcFromPolishTime(2026, 12, 6, 20)) return 12;
        if (plNow <= UtcFromPolishTime(2026, 12, 7, 20)) return 13;

        if (plNow <= UtcFromPolishTime(2026, 12, 9, 20)) return 14;
        if (plNow <= UtcFromPolishTime(2026, 12, 10, 20)) return 15;
        if (plNow <= UtcFromPolishTime(2026, 12, 11, 20)) return 16;

        if (plNow <= UtcFromPolishTime(2026, 12, 13, 20)) return 17;
        if (plNow <= UtcFromPolishTime(2026, 12, 14, 20)) return 18;

        return 20; // competition ended
    }

    private static DateTime UtcFromPolishTime(int year, int month, int day, int hour)
    {
        TimeZoneInfo polandZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime targetPolishLocal = new(year, month, day, hour, 0, 0, DateTimeKind.Unspecified);
        DateTime targetUtc = TimeZoneInfo.ConvertTimeToUtc(targetPolishLocal, polandZone); // handles CET and CEST
        return targetUtc;
    }

}
