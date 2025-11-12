using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class TimeEntryModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "TimeEntries";

    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("stageId")]
    public required int StageId { get; init; }

    [JsonRequired]
    [JsonPropertyName("carId")]
    public required int CarId { get; init; }

    [JsonRequired]
    [JsonPropertyName("driverId")]
    public required Guid DriverId { get; init; }

    [JsonRequired]
    [JsonPropertyName("time")]
    public required int Time { get; init; } // centiseconds

    [JsonRequired]
    [JsonPropertyName("uploadTime")]
    public required string UploadTime { get; init; }

    [JsonPropertyName("proofs")]
    public List<string>? Proofs { get; set; }

    [JsonPropertyName("verifyInfoId")]
    public Guid? VerifyInfoId { get; set; }

    [JsonPropertyName("labels")]
    public string? Labels { get; set; }

}
