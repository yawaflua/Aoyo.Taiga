using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaIssue : BaseTaigaItem
{
    [JsonPropertyName("type")]
    public TaigaType Type { get; set; }
        
    [JsonPropertyName("priority")]
    public TaigaPriority Priority { get; set; }
        
    [JsonPropertyName("severity")]
    public TaigaSeverity Severity { get; set; }
        
    [JsonPropertyName("milestone")]
    public TaigaMilestone Milestone { get; set; }
        
    [JsonPropertyName("generated_user_stories")]
    public List<TaigaUserStory> GeneratedUserStories { get; set; } = new List<TaigaUserStory>();
        
    [JsonPropertyName("external_reference")]
    public List<string> ExternalReference { get; set; } = new List<string>();
        
    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }
        
    [JsonPropertyName("due_date_reason")]
    public string DueDateReason { get; set; }
}