namespace Aoyo.Taiga.WebHookTypes;

public class UserStoryWebhookPayload : TaigaWebhookPayload<TaigaUserStory> { }
public class IssueWebhookPayload : TaigaWebhookPayload<TaigaIssue> { }
public class TaskWebhookPayload : TaigaWebhookPayload<TaigaTask> { }