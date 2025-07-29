using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaMilestone
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("name")]
    public string Name { get; set; }
        
    [JsonPropertyName("slug")]
    public string Slug { get; set; }
        
    [JsonPropertyName("estimated_start")]
    public DateTime? EstimatedStart { get; set; }
        
    [JsonPropertyName("estimated_finish")]
    public DateTime? EstimatedFinish { get; set; }
}