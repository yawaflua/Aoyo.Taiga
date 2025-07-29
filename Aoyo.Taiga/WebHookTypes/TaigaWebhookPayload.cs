using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaWebhookPayload<T> where T : ITaigaItem
{
    [JsonPropertyName("action")]
    public string Action { get; set; }
        
    [JsonPropertyName("type")]
    public string Type { get; set; }
        
    [JsonPropertyName("by")]
    public TaigaUser By { get; set; }
        
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
        
    [JsonPropertyName("data")]
    public T Data { get; set; }
        
    [JsonPropertyName("change")]
    public TaigaChange? Change { get; set; }
        
    public WebhookAction GetAction()
    {
        return Action?.ToLower() switch
        {
            "create" => WebhookAction.Create,
            "change" => WebhookAction.Change,
            "delete" => WebhookAction.Delete,
            _ => WebhookAction.Change
        };
    }
}