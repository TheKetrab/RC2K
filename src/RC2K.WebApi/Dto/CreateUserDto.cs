namespace RC2K.WebApi.Dto;

public class CreateUserDto
{
    public string? Nationality { get; set; }
    public required string Name { get; init; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
