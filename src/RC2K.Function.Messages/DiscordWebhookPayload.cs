using System.Text.Json.Serialization;

namespace RC2K.Function.Messages;

public class DiscordWebhookPayload
{
    [JsonPropertyName("username")]
    public string UserName { get; } = "RC2K Hub Bot";

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; } = "https://i.imgur.com/4M34hi2.png";

    [JsonRequired]
    [JsonPropertyName("content")]
    public string Content { get; set; } = default!;

    [JsonPropertyName("embeds")]
    public List<DiscordEmbed> Embeds { get; set; } = new();
}

public class DiscordEmbed
{
    [JsonPropertyName("color")]
    public int Color { get; } = 1413835;

    [JsonPropertyName("image")]
    public DiscordEmbedImage? Image { get; set; }
}

public class DiscordEmbedImage
{
    [JsonRequired]
    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;
}