using Discord;
using Discord.WebSocket;

namespace Aoyo.Taiga.Discord;

public class DiscordService(
    DiscordSocketClient client,
    IConfiguration configuration,
    ILogger<DiscordService> logger,
    DiscordHandler interactionHandler)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        client.Log += ClientOnLog;
        await client.LoginAsync(TokenType.Bot, configuration.GetValue<string>("Discord:Token"));
        await client.StartAsync();
        await interactionHandler.InitializeAsync();
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private Task ClientOnLog(LogMessage arg)
    {
        logger.LogInformation(exception: arg.Exception, message:arg.Message);
        return Task.CompletedTask;
    }
}