using Microsoft.AspNetCore;

namespace Aoyo.Taiga;

public class Program
{
    public static void Main(string[] args)
    {
        WebHost.
            CreateDefaultBuilder(args).
            UseStartup<Startup>().
            ConfigureAppConfiguration(k => 
                k.
                    AddJsonFile("appsettings.json").
                    AddJsonFile("appsettings.Development.json").
                    AddEnvironmentVariables()).
            UseKestrel(
                l => 
                    l.ListenAnyIP(8080)
                ).
            Build().Run();
    }
}