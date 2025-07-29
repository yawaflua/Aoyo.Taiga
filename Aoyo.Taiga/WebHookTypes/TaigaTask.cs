using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaTask : BaseTaigaItem
{
    [JsonPropertyName("user_story")]
    public TaigaUserStory? UserStory { get; set; }
        
    [JsonPropertyName("milestone")]
    public TaigaMilestone Milestone { get; set; }
        
        
    [JsonPropertyName("external_reference")]
    public List<string> ExternalReference { get; set; } = new List<string>();
        
    [JsonPropertyName("due_date")]
    public DateTime? DueDate { get; set; }
        
    [JsonPropertyName("due_date_reason")]
    public string DueDateReason { get; set; }
}