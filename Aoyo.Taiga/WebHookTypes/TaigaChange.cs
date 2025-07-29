using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaChange
{
    [JsonPropertyName("comment")]
    public string Comment { get; set; }
        
    [JsonPropertyName("comment_html")]
    public string CommentHtml { get; set; }
        
    [JsonPropertyName("diff")]
    public Dictionary<string, TaigaFieldChange> Diff { get; set; } = new Dictionary<string, TaigaFieldChange>();
}