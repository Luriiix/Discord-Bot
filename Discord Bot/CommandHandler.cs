using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

namespace Discord_Bot;

public class CommandHandler
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commands;

    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        _commands = commands;
        _client = client;
    }

    public async Task InstallCommandsAsync()
    {
        _client.MessageReceived += HandleCommandAsync;
        await _commands.AddModulesAsync(assembly: Assembly.GetEntryAssembly(),
            services: null);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        if (messageParam is not SocketUserMessage message) return;
        var argPos = 0;
        
        if (!message.HasCharPrefix('!', ref argPos)) return;
        if (message.Author.IsBot) return;

        var context = new SocketCommandContext(_client, message);
        await _commands.ExecuteAsync(context, argPos, null);

    }
}

public class InfoModule : ModuleBase<SocketCommandContext>
{
    [Command("say")]
    [Summary("Echoes a message.")]
    public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
        => ReplyAsync(echo);
}

