namespace RC2K.DomainModel;

public class Driver : IEntity
{
    public required int Id { get; init; }
    public required bool Known { get; init; }
    public int? UserId { get; init; }
    public string? Name { get; init; }
    public string? Key { get; init; }
    public User? User { get; set; }
}
