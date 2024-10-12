using Discord.WebSocket;

namespace Discord_Bot;

public class Context
{
    private DiscordSocketClient Client { get; }
    private SocketSlashCommand SlashCommand { get; }
    
    public string Message { get; set; }

    public Context(DiscordSocketClient client, SocketSlashCommand slashCommand, string message)
    {
        Client = client;
        SlashCommand = slashCommand;
        Message = message;
    }

    public object GetChoice() =>
        SlashCommand.Data.Options.First().Value;

    public SocketSlashCommand GetCommand() => SlashCommand;
}