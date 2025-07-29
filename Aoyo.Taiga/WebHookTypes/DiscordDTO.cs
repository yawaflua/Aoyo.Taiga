using Discord;

namespace Aoyo.Taiga.WebHookTypes;

public class DiscordDTO
{
    public TaigaUser By { get; set; }
    public string Action { get; set; }
    public string Type { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public Color Color { get; set; }
}