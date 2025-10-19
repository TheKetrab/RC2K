namespace RC2K.DomainModel;

public class User : IEntity
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required int DriverId { get; init; }
    public required string[] Roles { get; init; }
    public required string PasswordHash { get; init; }

    public Driver? Driver { get; set; }

    public override string ToString() => $"{Name} ({string.Join(",", Roles)})";
}
