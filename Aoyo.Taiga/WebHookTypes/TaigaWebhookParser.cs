namespace Aoyo.Taiga.WebHookTypes;

public class TaigaWebhookParser
{
    public static UserStoryWebhookPayload? ParseUserStoryWebhook(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<UserStoryWebhookPayload>(json);
    }
        
    public static IssueWebhookPayload? ParseIssueWebhook(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<IssueWebhookPayload>(json);
    }
        
    public static TaskWebhookPayload? ParseTaskWebhook(string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<TaskWebhookPayload>(json);
    }
}