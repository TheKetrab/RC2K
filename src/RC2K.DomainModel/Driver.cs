namespace RC2K.DomainModel;

public class Driver
{
    public required Guid Id { get; init; }
    public required bool Known { get; init; }
    public Guid? UserId { get; init; }
    public string? Name { get; init; }
    public string? Key { get; init; }
    public User? User { get; set; }
    public string? Nationality { get; init; }
}
