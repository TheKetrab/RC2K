
namespace RC2K.Presentation.Shared.ViewModels;

public class MenuItem
{
    public Icon Icon { get; set; }
    public string Item { get; set; } = string.Empty;
    public string ItemLink { get; set; } = string.Empty;
    public List<(string label, string link)> Subitems { get; set; } = new();
}
