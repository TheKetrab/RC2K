using System.ComponentModel.DataAnnotations;

namespace RC2K.Presentation.Blazor.ViewModels;

public class LoginDto
{
    [Required(AllowEmptyStrings = false)]
    public string? UserName { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string? Password { get; set; }
}
