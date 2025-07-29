using Aoyo.Taiga.Discord;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Aoyo.Taiga;

public class Startup(IConfiguration configuration)
{
    private readonly DiscordSocketConfig _socketConfig = new()
    {
        GatewayIntents = GatewayIntents.All,
        AlwaysDownloadUsers = true
    };

    public void ConfigureServices(IServiceCollection services)
    {
        var factory = LoggerFactory.Create(builder => builder.AddFilter(k => k >=
                                                                             (configuration.GetValue<LogLevel?>("Logging:LogLevel:Default") ??
                                                                              LogLevel.Information)).AddConsole());
        services.AddControllers();
        services.AddHttpLogging();
        services.AddSingleton(factory);
        services.AddRouting();
        services.AddSingleton(_socketConfig);
        services.AddSingleton(configuration);
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));
        services.AddSingleton<DiscordHandler>();
        services.AddSingleton(x => x.CreateScope());
        services.AddHostedService<DiscordService>();
        services.AddHealthChecks();
    }
 
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
 
        app.UsePathBase("/aoyo/");
        app.UseHealthChecks("/health");
        app.UseHttpLogging();
        
        app.UseRouting();
        
        app.UseEndpoints(endpoints => { 
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/heath");
        });
    }
}