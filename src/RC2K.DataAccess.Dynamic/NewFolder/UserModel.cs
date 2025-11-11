using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class UserModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "Users";

    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonRequired]
    [JsonPropertyName("driverId")]
    public required Guid DriverId { get; init; }

    [JsonPropertyName("hash")]
    public string? PasswordHash { get; set; }

    [JsonPropertyName("roles")]
    public List<string> Roles { get; } = [];

    [JsonPropertyName("email")]
    public string? Email { get; init; }
}
