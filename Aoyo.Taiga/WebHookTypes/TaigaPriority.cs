using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaPriority
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("name")]
    public string Name { get; set; }
        
    [JsonPropertyName("color")]
    public string Color { get; set; }
        
    [JsonPropertyName("order")]
    public int Order { get; set; }
}