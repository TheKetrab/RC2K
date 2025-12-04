using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class BonusPointsModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "BonusPoints";

    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("driverId")]
    public required Guid DriverId { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonRequired]
    [JsonPropertyName("points")]
    public required int Points { get; init; }

}
