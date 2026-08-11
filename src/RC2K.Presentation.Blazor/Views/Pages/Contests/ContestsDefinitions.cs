using RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi24;
using RC2K.Presentation.Blazor.Views.Pages.Contests.Mfmi26;

namespace RC2K.Presentation.Blazor.Views.Pages.Contests.Definitions;

public interface IContest
{
    string MainLink { get; }
    Dictionary<string, object> CustomProperties { get; }
}

public interface IContest<TParticipant> : IContest where TParticipant : IParticipant
{
    List<TParticipant> Participants { get; }
}

public interface IParticipant
{
    string Name { get; }
    string Car { get; }
    Dictionary<string, object> CustomProperties { get; }
}

public class Constants
{
    public const string PdfManualLinkProperty = nameof(PdfManualLinkProperty);
    public const string NrProperty = nameof(NrProperty);
    public const string GroupProperty = nameof(GroupProperty);
    public const string DriverProfileProperty = nameof(DriverProfileProperty);
}

public class MfmiParticipant : IParticipant
{
    public int Nr { get; }
    public string Name { get; }
    public string Car { get; }
    public Dictionary<string, object> CustomProperties { get; } = [];

    public MfmiParticipant(string name, string car, int nr)
    {
        Name = name;
        Car = car;
        Nr = nr;
        CustomProperties.Add(Constants.NrProperty, Nr);
    }
}

public class ContestsDefinitions
{
    private static readonly Lock _lock = new();
    public static ContestsDefinitions Instance 
    {
        get
        {
            if (field is null)
            {
                lock (_lock)
                {
                    return field ??= new();
                }
            }
            return field;
        } 
    }
    public Mfmi24Contest Mfmi24 { get; }
    public Mfmi26Contest Mfmi26 { get; }
    private ContestsDefinitions()
    {
        Mfmi24 = new();
        Mfmi26 = new();
    }
}
