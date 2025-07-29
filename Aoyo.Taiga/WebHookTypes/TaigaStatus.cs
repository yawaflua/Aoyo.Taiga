using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaStatus
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("name")]
    public string Name { get; set; }
        
    [JsonPropertyName("color")]
    public string Color { get; set; }
        
    [JsonPropertyName("is_closed")]
    public bool IsClosed { get; set; }
}