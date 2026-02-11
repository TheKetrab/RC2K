
namespace RC2K.Presentation.Shared.ViewModels;

public class BreadcrumbLink
{
    public int OrderIndex { get; set; }
    public string Address { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}