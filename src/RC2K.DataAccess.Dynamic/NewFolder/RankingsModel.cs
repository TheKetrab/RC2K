using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class RankingSnapshotModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "Statistics";

    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("type")]
    public string Type { get; set; } = "ranking";

    [JsonRequired]
    [JsonPropertyName("date")]
    public required string Date { get; init; }

    [JsonRequired]
    [JsonPropertyName("entries")]
    public List<RankingEntryModel> Entries { get; set; } = new();
}

public class RankingEntryModel
{
    [JsonRequired]
    [JsonPropertyName("place")]
    public required int Place { get; init; }

    [JsonRequired]
    [JsonPropertyName("driverId")]
    public required Guid DriverId { get; init; }

    [JsonRequired]
    [JsonPropertyName("data")]
    public required string Data { get; init; }
}