using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class CronMessageModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "Statistics";

    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("published")]
    public required bool Published { get; init; }

    [JsonRequired]
    [JsonPropertyName("cron")]
    public required string Cron { get; init; }
    [JsonRequired]
    [JsonPropertyName("message")]
    public required string Message { get; init; }

}
