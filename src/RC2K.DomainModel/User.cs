namespace RC2K.DomainModel;

public class User
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required Guid DriverId { get; init; }
    public required string[] Roles { get; init; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }

    public Driver? Driver { get; set; }

    public override string ToString() => $"{Name} ({string.Join(",", Roles)})";
}
