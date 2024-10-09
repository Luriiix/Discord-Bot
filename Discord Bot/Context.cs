using Discord.WebSocket;

namespace Discord_Bot;

public class Context
{
    private DiscordSocketClient Client { get; }
    private SocketSlashCommand SlashCommand { get; }

    public Context(DiscordSocketClient client, SocketSlashCommand slashCommand)
    {
        Client = client;
        SlashCommand = slashCommand;
    }

    public object GetOption() =>
        SlashCommand.Data.Options.First().Value;

    public SocketSlashCommand GetCommand() => SlashCommand;
}