using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaFieldChange
{
    [JsonPropertyName("from")]
    public object From { get; set; }
        
    [JsonPropertyName("to")]
    public object To { get; set; }
}