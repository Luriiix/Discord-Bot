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
        var message = messageParam as SocketUserMessage;
        if (message == null) return;
        
        var argPos = 0;
        
        if (!message.HasCharPrefix('!', ref argPos)) return;
        if (message.Author.IsBot) return;
        if (message.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

        var context = new SocketCommandContext(_client, message);
        await _commands.ExecuteAsync(
            context: context, 
            argPos: argPos,
            services: null);

    }
}

public class InfoModule : ModuleBase<SocketCommandContext>
{
    
}