using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaPoint
{
    [JsonPropertyName("role")]
    public string Role { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("value")]
    public float? Value { get; set; }
}