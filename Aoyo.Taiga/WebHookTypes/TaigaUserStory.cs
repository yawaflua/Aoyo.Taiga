using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaUserStory : BaseTaigaItem
{
    [JsonPropertyName("points")]
    public List<TaigaPoint> Points { get; set; } = new ();
        
    [JsonPropertyName("total_points")]
    public float? TotalPoints { get; set; }
        
    [JsonPropertyName("milestone")]
    public TaigaMilestone Milestone { get; set; }
        
    [JsonPropertyName("client_requirement")]
    public bool ClientRequirement { get; set; }
        
    [JsonPropertyName("team_requirement")]
    public bool TeamRequirement { get; set; }
        
    [JsonPropertyName("generated_from_issue")]
    public int? GeneratedFromIssue { get; set; }
        
    [JsonPropertyName("generated_from_task")]
    public int? GeneratedFromTask { get; set; }
        
    [JsonPropertyName("from_task_ref")]
    public int? FromTaskRef { get; set; }
        
    [JsonPropertyName("external_reference")]
    public List<string> ExternalReference { get; set; } = new List<string>();
        
    [JsonPropertyName("tribe_gig")]
    public string TribeGig { get; set; }
        
    [JsonPropertyName("kanban_order")]
    public int KanbanOrder { get; set; }
        
    [JsonPropertyName("sprint_order")]
    public int SprintOrder { get; set; }
        
    [JsonPropertyName("backlog_order")]
    public int BacklogOrder { get; set; }
}