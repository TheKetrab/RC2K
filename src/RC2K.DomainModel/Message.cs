namespace RC2K.DomainModel;

public class DateTimeMessage
{
    public required Guid Id { get; init; }
    public required bool Published { get; set; }
    public required string Value { get; init; }
    public required DateTime DateTime { get; init; }
    public string? Name { get; set; }
}
