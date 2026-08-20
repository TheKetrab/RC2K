using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.ViewModels;

public class MfmiEntryListItemViewModel
{
    public int Nr { get; init; }
    public Driver Driver { get; set; }
    public int Group { get; init; }
    public Car Car { get; init; }
    public string DriverProfile { get; init; }
}