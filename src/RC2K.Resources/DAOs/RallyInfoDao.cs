using System.Text.Json.Serialization;

namespace RC2K.Resources.DAOs;

public class RallyInfoDao
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("stages")]
    public List<StageInfoDao> Stages { get; set; } = [];
}