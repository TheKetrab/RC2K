namespace RC2K.DomainModel;

public class BonusPoints
{
    public required Guid Id { get; init; }
    public required Guid DriverId { get; init; }
    public string? Comment { get; set; }
    public int Points { get; init; }

    public Driver? Driver { get; set; }
}
