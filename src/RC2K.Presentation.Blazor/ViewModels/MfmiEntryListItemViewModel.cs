using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.ViewModels;

public class MfmiEntryListItemViewModel
{
    public int Nr { get; init; }
    public required Driver Driver { get; init; }
    public int Group { get; init; }
    public required Car Car { get; init; }
    public string? DriverProfile { get; set; }
}