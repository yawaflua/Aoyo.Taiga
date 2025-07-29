using Microsoft.AspNetCore;

namespace Aoyo.Taiga;

public class Program
{
    public static void Main(string[] args)
    {
        WebHost.
            CreateDefaultBuilder(args).
            UseStartup<Startup>().
            UseKestrel(
                l => 
                    l.ListenAnyIP(8080)
                ).
            Build().Run();
    }
}