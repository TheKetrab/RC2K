
using System.ComponentModel.DataAnnotations;

namespace RC2K.Presentation.Blazor.ViewModels;

public class RegisterAccountDto
{
    [Required(AllowEmptyStrings = false)]
    [StringLength(8, ErrorMessage = "Name length can't be more than 8.")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public (string, string)? Nationality { get; set; }

    [Required(AllowEmptyStrings = false)]
    [StringLength(64, ErrorMessage = "Password must be at least 8 characters long.", MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    [Compare(nameof(Password))]
    public string Password2 { get; set; } = string.Empty;

}