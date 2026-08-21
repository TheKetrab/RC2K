using System.Text.Json.Serialization;

namespace RC2K.Presentation.Blazor.Database;

public class MfmiSummariesModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "Statistics";

    [JsonRequired]
    [JsonPropertyName("type")]
    public string Type { get; set; } = "mfmi-summary";

    [JsonRequired]
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonRequired]
    [JsonPropertyName("edition")]
    public string? Edition { get; set; }

    [JsonPropertyName("day")]
    public int? Day { get; set; }

    [JsonRequired]
    [JsonPropertyName("rallies")]
    public List<MfmiSummaryRallyModel> Rallies { get; set; } = [];
}

public class MfmiSummaryRallyModel
{
    [JsonRequired]
    [JsonPropertyName("rallyCode")]
    public required int RallyCode { get; init; }

    [JsonRequired]
    [JsonPropertyName("entries")]
    public List<MfmiRankingEntryModel> Entries { get; set; } = [];
}

public class MfmiRankingEntryModel
{
    [JsonRequired]
    [JsonPropertyName("rank")]
    public required int Rank { get; init; }

    [JsonRequired]
    [JsonPropertyName("nr")]
    public required int Nr { get; init; }

    [JsonRequired]
    [JsonPropertyName("driverFriendlyName")]
    public required string DriverFriendlyName { get; init; }

    [JsonPropertyName("driverNationality")]
    public string? DriverNationality { get; init; }

    [JsonPropertyName("group")]
    public string? Group { get; set; }

    [JsonRequired]
    [JsonPropertyName("carId")]
    public required int CarId { get; init; }

    [JsonRequired]
    [JsonPropertyName("time")]
    public required long TimeCentiseconds { get; init; }

    [JsonRequired]
    [JsonPropertyName("timeWcb")]
    public required long TimeWcbCentiseconds { get; init; }

    [JsonRequired]
    [JsonPropertyName("points")]
    public required int Points { get; init; }
}
