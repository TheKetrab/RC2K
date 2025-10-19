using System.Text.Json.Serialization;

namespace RC2K.Resources.DAOs;

public class StageInfoDao
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("img-name")]
    public string ImgName { get; set; } = string.Empty;

    [JsonPropertyName("description")] 
    public string Description { get; set; } = string.Empty;
}