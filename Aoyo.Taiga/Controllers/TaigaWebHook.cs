using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aoyo.Taiga.WebHookTypes;
using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace Aoyo.Taiga.Controllers;


[Route("api/v1/TaigaWebHook")]
[ApiController]
[SuppressMessage("ReSharper", "UnusedVariable")]
[SuppressMessage("ReSharper", "UnusedMember.Local")]
public class TaigaWebHook : Controller
{
    private const string ChannelIdConfigurationPath = "Discord:Id";
    private const string TaigaKeyPath = "Taiga:Key";
    private readonly ulong _id;
    private ITextChannel? _logChannel;
    private readonly DiscordSocketClient _client;
    private readonly ILogger<TaigaWebHook> _logger;
    private readonly string _key;
    private IConfiguration _config;

    public TaigaWebHook(DiscordSocketClient client, ILogger<TaigaWebHook> logger, IConfiguration config)
    {
        _config = config;
        _client = client;
        _logger = logger;
        _id = config.GetValue<ulong?>(ChannelIdConfigurationPath) ?? throw new NullReferenceException("Channel id should to be provided");
        _key = config.GetValue<string>(TaigaKeyPath) ?? throw new NullReferenceException("Taiga KEY should to be provided!") ;
        _logChannel = client.GetChannel(_id) as ITextChannel ?? throw new NullReferenceException("Channel not found.");
    }

    [HttpPost("post")]
    public async Task<IActionResult> PostAsync()
    {
        try
        {
            var signature = Request.Headers["X-TAIGA-WEBHOOK-SIGNATURE"].First()!;

            using var reader = new StreamReader(Request.Body);
            var data = await reader.ReadToEndAsync();
            _logger.LogInformation(data);
                
            if (_config.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Production")
                if (!VerifySignature(_key, data, signature))
                {
                    return BadRequest("Invalid signature");
                }


            var webhookType = DetermineWebhookType(data);
            
            switch (webhookType)
            {
                case "userstory":
                    await HandleUserStoryWebhook(data);
                    break;
                case "issue":
                    await HandleIssueWebhook(data);
                    break;
                case "task":
                    await HandleTaskWebhook(data);
                    break;
                default:
                    _logger.LogWarning("Unsupported type of webhook: {WebhookType}", webhookType);
                    return BadRequest($"Unsupported type of webhook: {webhookType}");
            }
            
            return Ok();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return BadRequest(exception.Message);
        }
    }

    private bool VerifySignature(string key, string data, string signature)
    {
        using HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key));
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        var computedHash = Convert.ToHexString(hashBytes).ToLower();
        
        return computedHash.Length == signature.Length;
    }
    
    private async Task HandleUserStoryWebhook(string json)
        {
            try
            {
                var webhook = TaigaWebhookParser.ParseUserStoryWebhook(json);
                var userStory = webhook.Data;
                var action = webhook.GetAction();

                _logger.LogInformation("Handling User Story: {Subject} (ID: {Id}), Action: {Action}", 
                    userStory.Subject, userStory.Id, action);

                switch (action)
                {
                    case WebhookAction.Create:
                        await OnUserStoryCreated(userStory, webhook);
                        break;
                    case WebhookAction.Change:
                        await OnUserStoryChanged(userStory, webhook);
                        break;
                    case WebhookAction.Delete:
                        await OnUserStoryDeleted(userStory, webhook);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when handled User Story webhook");
                throw;
            }
        }

        private async Task HandleIssueWebhook(string json)
        {
            try
            {
                var webhook = TaigaWebhookParser.ParseIssueWebhook(json);
                var issue = webhook.Data;
                var action = webhook.GetAction();

                _logger.LogInformation("Processing Issue: {Subject} (ID: {Id}), Action: {Action}", 
                    issue.Subject, issue.Id, action);

                switch (action)
                {
                    case WebhookAction.Create:
                        await OnIssueCreated(issue, webhook);
                        break;
                    case WebhookAction.Change:
                        await OnIssueChanged(issue, webhook);
                        break;
                    case WebhookAction.Delete:
                        await OnIssueDeleted(issue, webhook);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when processing Issue webhook");
                throw;
            }
        }

        private async Task HandleTaskWebhook(string json)
        {
            try
            {
                var webhook = TaigaWebhookParser.ParseTaskWebhook(json);
                var task = webhook.Data;
                var action = webhook.GetAction();

                _logger.LogInformation("Processing Task: {Subject} (ID: {Id}), Action: {Action}", 
                    task.Subject, task.Id, action);

                switch (action)
                {
                    case WebhookAction.Create:
                        await OnTaskCreated(task, webhook);
                        break;
                    case WebhookAction.Change:
                        await OnTaskChanged(task, webhook);
                        break;
                    case WebhookAction.Delete:
                        await OnTaskDeleted(task, webhook);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when processing Task webhook");
                throw;
            }
        }

        private async Task OnUserStoryCreated(TaigaUserStory userStory, UserStoryWebhookPayload? webhook)
        {
            _logger.LogInformation("Created new User Story: {Subject} in project {ProjectName}", 
                userStory.Subject, userStory.Project.Name);
            
            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Created",
                Type = "User Story",
                Date = webhook.Date,
                Description = $"**{userStory.Subject}**\n" +
                             $"> Project: {userStory.Project.Name}\n" +
                             $"> Status: {userStory.Status.Name}\n" +
                             $"> Points: {userStory.TotalPoints}",
                Color = Color.Green
            });
        }

        private async Task OnUserStoryChanged(TaigaUserStory userStory, UserStoryWebhookPayload? webhook)
        {
            _logger.LogInformation("Changed User Story: {Subject}", userStory.Subject);
            
            var changes = "";
            if (webhook.Change?.Diff != null)
            {
                foreach (var change in webhook.Change.Diff)
                {
                    changes += $"• {change.Key}: {change.Value.From} → {change.Value.To}\n";
                }
            }

            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Changed",
                Type = "User Story",
                Date = webhook.Date,
                Description = $"**{userStory.Subject}**\n" +
                             $"> Project: {userStory.Project.Name}\n" +
                             $"> Changes:\n{changes}",
                Color = Color.LightOrange
            });
        }

        private async Task OnUserStoryDeleted(TaigaUserStory userStory, UserStoryWebhookPayload? webhook)
        {
            _logger.LogInformation("Deleted User Story: {Subject} (ID: {Id})", 
                userStory.Subject, userStory.Id);
            
            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Deleted",
                Type = "User Story",
                Date = webhook.Date,
                Description = $"**{userStory.Subject}**\n" +
                             $"> Project: {userStory.Project.Name}\n" +
                             $"> ID: {userStory.Id}",
                Color = Color.DarkRed
            });
        }

        // Методы для обработки Issue событий
        private async Task OnIssueCreated(TaigaIssue issue, IssueWebhookPayload? webhook)
        {
            _logger.LogInformation("Created new Issue: {Subject} in project {ProjectName}", 
                issue.Subject, issue.Project.Name);
            
            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Created",
                Type = "Issue",
                Date = webhook.Date,
                Description = $"**{issue.Subject}**\n" +
                             $"> Project: {issue.Project.Name}\n" +
                             $"> Type: {issue.Type.Name}\n" +
                             $"> Prority: {issue.Priority.Name}",
                Color = Color.Blue
            });
        }

        private async Task OnIssueChanged(TaigaIssue issue, IssueWebhookPayload? webhook)
        {
            _logger.LogInformation("Changed Issue: {Subject}", issue.Subject);
            
            var changes = "";
            if (webhook.Change?.Diff != null)
            {
                foreach (var change in webhook.Change.Diff)
                {
                    changes += $"> • {change.Key}: {change.Value.From} → {change.Value.To}\n";
                }
            }

            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Changed",
                Type = "Issue",
                Date = webhook.Date,
                Description = $"**{issue.Subject}**\n" +
                             $"> Project: {issue.Project.Name}\n" +
                             $"> Changes:\n{changes}",
                Color = Color.LightOrange
            });
        }

        private async Task OnIssueDeleted(TaigaIssue issue, IssueWebhookPayload? webhook)
        {
            _logger.LogInformation("Deleted Issue: {Subject} (ID: {Id})", 
                issue.Subject, issue.Id);
            
            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Deleted",
                Type = "Issue",
                Date = webhook.Date,
                Description = $"**{issue.Subject}**\n" +
                             $"> Project: {issue.Project.Name}\n" +
                             $"> ID: {issue.Id}",
                Color = Color.Red
            });
        }

        private async Task OnTaskCreated(TaigaTask task, TaskWebhookPayload? webhook)
        {
            _logger.LogInformation("Created new Task: {Subject} in project {ProjectName}", 
                task.Subject, task.Project.Name);
            
            var description = $"**{task.Subject}**\n" +
                     $"> Project: {task.Project.Name}\n" +
                     $"> Исполнитель: {task.AssignedTo?.FullName ?? "Не назначен"}";
            
            if (task.UserStory != null)
            {
                description += $"\n> User Story: {task.UserStory.Subject} (#{task.UserStory.Ref})";
            }

            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Created",
                Type = "Task",
                Date = webhook.Date,
                Description = description,
                Color = Color.Green
            });
        }

        private async Task OnTaskChanged(TaigaTask task, TaskWebhookPayload? webhook)
        {
            _logger.LogInformation("Changed Task: {Subject}", task.Subject);
            
            var changes = "";
            if (webhook.Change?.Diff != null)
            {
                foreach (var change in webhook.Change.Diff)
                {
                    changes += $"• {change.Key}: {change.Value.From} → {change.Value.To}\n";
                }
            }

            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Changed",
                Type = "Task",
                Date = webhook.Date,
                Description = $"**{task.Subject}**\n" +
                     $"> Project: {task.Project.Name}\n" +
                     $"> Changes:\n{changes}",
                Color = Color.Orange
            });
        }

        private async Task OnTaskDeleted(TaigaTask task, TaskWebhookPayload? webhook)
        {
            _logger.LogInformation("Deleted Task: {Subject} (ID: {Id})", 
                task.Subject, task.Id);
            
            await SendDiscordNotification(new DiscordDTO
            {
                By = webhook.By,
                Action = "Deleted",
                Type = "Task",
                Date = webhook.Date,
                Description = $"**{task.Subject}**\n" +
                     $"> Project: {task.Project.Name}\n" +
                     $"> ID: {task.Id}",
                Color = Color.Red
            });
        }

        private async Task SendDiscordNotification(DiscordDTO payload)
        {
            try
            {
                _logChannel ??= _client.GetChannel(_id) as ITextChannel;
                
                if (_logChannel == null)
                {
                    _logger.LogError("Не удалось получить Discord канал с ID: {ChannelId}", _id);
                    return;
                }

                await _logChannel.SendMessageAsync(embed:
                    new EmbedBuilder()
                        .WithAuthor(payload.By.Username, payload.By.Photo)
                        .WithTitle($"{payload.Action} {payload.Type.ToLower()}")
                        .WithDescription(payload.Description)
                        .WithColor(payload.Color)
                        .WithTimestamp(payload.Date)
                        .Build());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when sending message into Discord");
            }
        }


        private string DetermineWebhookType(string json)
        {
            try
            {
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;
                
                if (root.TryGetProperty("type", out var typeElement))
                {
                    return typeElement.GetString()?.ToLower()!;
                }
                
                if (root.TryGetProperty("data", out var dataElement))
                {
                    if (dataElement.TryGetProperty("points", out _))
                    {
                        return "userstory";
                    }
                    else if (dataElement.TryGetProperty("type", out _) && dataElement.TryGetProperty("priority", out _))
                    {
                        return "issue";
                    }
                    else if (dataElement.TryGetProperty("user_story", out _))
                    {
                        return "task";
                    }
                }
                
                return "unknown";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot parse this type of webhook");
                return "unknown";
            }
        }
    
}