namespace RC2K.DomainModel;

public class Notification
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Message { get; init; }
    public DateTime Created { get; init; }
}
