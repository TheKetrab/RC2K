using RC2K.DomainModel;

namespace RC2K.Presentation.Blazor.ViewModels;

public class UploadTimeDto
{
    public int Min { get; set; }
    public int Sec { get; set; }
    public int Cc { get; set; }
    public Car? Car { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public string Label { get; set; } = "TA";
}
