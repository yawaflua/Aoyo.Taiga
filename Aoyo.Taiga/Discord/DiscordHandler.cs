using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Aoyo.Taiga.Discord;

public class DiscordHandler(
    DiscordSocketClient client,
    InteractionService interactionService,
    ILogger<DiscordHandler> logger,
    IServiceScope provider)
{
    
    public async Task InitializeAsync()
    {
        client.Ready += ClientOnReady;
        client.Log += ClientOnLog;
        await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), provider.ServiceProvider);
        client.InteractionCreated += ClientOnInteractionCreated;
    }

    private async Task ClientOnInteractionCreated(SocketInteraction arg)
    {
        try
        {
            var context = new SocketInteractionContext(client, arg);
            var result = await interactionService.ExecuteCommandAsync(context, provider.ServiceProvider);
            if (result.Error is not null)
            {
                throw new Exception($"{result.ErrorReason}: {result.Error}");
            }
        }
        catch (Exception ex)
        {
            await arg.RespondAsync(":x: " + ex.Message + "", ephemeral: true);
            logger.LogError(ex, ex.Message);
        }
    }

    private Task ClientOnLog(LogMessage arg)
    {
        logger.LogInformation(exception: arg.Exception, message:arg.Message);
        return Task.CompletedTask;
    }

    private async Task ClientOnReady()
    {
        await interactionService.RegisterCommandsGloballyAsync();
    }
}