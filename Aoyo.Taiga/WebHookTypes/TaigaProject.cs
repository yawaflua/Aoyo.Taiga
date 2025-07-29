using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaProject
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("name")]
    public string Name { get; set; }
        
    [JsonPropertyName("slug")]
    public string Slug { get; set; }
        
    [JsonPropertyName("description")]
    public string Description { get; set; }
}