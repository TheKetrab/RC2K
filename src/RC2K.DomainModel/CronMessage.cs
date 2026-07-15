namespace RC2K.DomainModel;

public class CronMessage
{
    public required Guid Id { get; init; }
    public required bool Published { get; set; }
    public required string Message { get; init; }
    public required string Cron { get; init; }
}
