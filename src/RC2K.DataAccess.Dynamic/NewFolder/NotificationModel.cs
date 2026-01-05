using System.Text.Json.Serialization;

namespace RC2K.DataAccess.Dynamic.Models;

public class NotificationModel
{
    [JsonRequired]
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = "Notifications";

    [JsonRequired]
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }

    [JsonRequired]
    [JsonPropertyName("userId")]
    public required Guid UserId { get; init; }

    [JsonRequired]
    [JsonPropertyName("created")]
    public required string Created { get; init; }

    [JsonRequired]
    [JsonPropertyName("message")]
    public required string Message { get; init; }

}
