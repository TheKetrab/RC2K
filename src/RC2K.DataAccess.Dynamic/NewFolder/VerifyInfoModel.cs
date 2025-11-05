using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class VerifyInfoModel
{
    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("verifierId")]
    public required Guid VerifierId { get; init; }

    [JsonPropertyName("comment")]
    public string? Comment { get; init; }

    [JsonRequired]
    [JsonPropertyName("verifyDate")]
    public required string VerifyDate { get; init; }

}
