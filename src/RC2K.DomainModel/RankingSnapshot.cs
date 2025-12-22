namespace RC2K.DomainModel;

public class RankingSnapshot
{
    public required Guid Id { get; init; }
    public required DateTime Date { get; init; }
    public List<RankingEntry> Entries { get; } = new();
}