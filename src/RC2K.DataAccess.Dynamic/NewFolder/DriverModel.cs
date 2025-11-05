using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class DriverModel
{
    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("known")]
    public required bool Known { get; init; }

    [JsonPropertyName("userId")]
    public Guid? UserId { get; init; }

    [JsonPropertyName("name")]
    public string? Name { get; init; }

    [JsonPropertyName("key")]
    public string? Key { get; init; }

    [JsonPropertyName("nat")]
    public string? Nat { get; set; }
}
