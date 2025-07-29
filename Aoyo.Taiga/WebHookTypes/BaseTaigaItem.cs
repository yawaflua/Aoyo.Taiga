using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public abstract class BaseTaigaItem : ITaigaItem
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("ref")]
    public int Ref { get; set; }
        
    [JsonPropertyName("subject")]
    public string Subject { get; set; }
        
    [JsonPropertyName("description")]
    public string Description { get; set; }
        
    [JsonPropertyName("created_date")]
    public DateTime CreatedDate { get; set; }
        
    [JsonPropertyName("modified_date")]
    public DateTime? ModifiedDate { get; set; }
        
    [JsonPropertyName("owner")]
    public TaigaUser Owner { get; set; }
        
    [JsonPropertyName("assigned_to")]
    public TaigaUser? AssignedTo { get; set; }
        
    [JsonPropertyName("project")]
    public TaigaProject Project { get; set; }
        
    [JsonPropertyName("status")]
    public TaigaStatus Status { get; set; }
        
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new List<string>();
        
    [JsonPropertyName("is_closed")]
    public bool IsClosed { get; set; }
        
    [JsonPropertyName("is_blocked")]
    public bool IsBlocked { get; set; }
        
    [JsonPropertyName("blocked_note")]
    public string BlockedNote { get; set; }
        
    [JsonPropertyName("version")]
    public int Version { get; set; }
        
    [JsonPropertyName("watchers")]
    public List<int> Watchers { get; set; } = new List<int>();
}