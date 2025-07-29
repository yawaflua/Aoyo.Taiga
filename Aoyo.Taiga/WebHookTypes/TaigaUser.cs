using System.Text.Json.Serialization;

namespace Aoyo.Taiga.WebHookTypes;

public class TaigaUser
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
        
    [JsonPropertyName("username")]
    public string Username { get; set; }
        
    [JsonPropertyName("full_name")]
    public string FullName { get; set; }
        
    [JsonPropertyName("email")]
    public string Email { get; set; }
        
    [JsonPropertyName("photo")]
    public string Photo { get; set; }
    [JsonPropertyName("gravatar_id")]
    public string? GravatarId { get; set; }
}