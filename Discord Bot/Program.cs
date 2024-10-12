using Discord;
using Discord_Bot;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton(
    new DiscordSocketConfig()
        { GatewayIntents = GatewayIntents.MessageContent | GatewayIntents.GuildMessages | GatewayIntents.Guilds }
);
services.AddSingleton<DiscordSocketClient>();
services.AddSingleton<CommandService>();
services.AddSingleton<CommandHandler>();
services.AddSingleton<Rps>();
services.AddSingleton<CreatingSlashCommands>();
services.AddSingleton<WriteMessages>();

var app = services.BuildServiceProvider();
var client = app.GetRequiredService<DiscordSocketClient>();
client.Log += Log;

const string token = "MTI3OTgzODY3Mjk1MDM5OTA2OQ.GrVZnO.dr0PIqqe5qxCRlzG-xGQqVKAHyQ_B0kkfe3tIQ";

var handler = app.GetRequiredService<CommandHandler>();

await handler.InstallCommandsAsync();

await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

client.Ready += GuildCommand;
client.SlashCommandExecuted += SlashCommandHandler;
client.ButtonExecuted += MyButtonHandler;

await Task.Delay(-1);


Task Log(LogMessage msg)
{
    Console.WriteLine(msg);
    return Task.CompletedTask;
}

Task GuildCommand()
{
    Parser.AddAllParsers();
    var creatingSlashCommands = app.GetRequiredService<CreatingSlashCommands>();
    var startCommand = creatingSlashCommands.RegisterCommands<StartCommand>();
    var messageCommand = creatingSlashCommands.RegisterCommands<WriteMessages>();
    
    startCommand.Build();
    return Task.CompletedTask;
}

async Task SlashCommandHandler(SocketSlashCommand slashCommand) {
    var commandMessage = "!lol (HalLoooooooo)";
    
    await slashCommand.DeferAsync();
    var creatingSlashCommands = app.GetRequiredService<CreatingSlashCommands>();
    var commands = creatingSlashCommands.Commands;
    
    var command = commands[slashCommand.CommandName];
    command.Action(new Context(client, slashCommand, commandMessage));
    
    await slashCommand.ModifyOriginalResponseAsync(message => message.Content = "lmao");
}

async Task MyButtonHandler(SocketMessageComponent component)
{
    Console.WriteLine(component.Data.CustomId);
    // Buttonacctions[customid] = button;
    // button.action;
        
    switch (component.Data.CustomId)
    {
        case "id1":
            await component.DeferAsync();
            await component.Channel.SendMessageAsync("lol");
            await component.ModifyOriginalResponseAsync(
                message =>
                {
                    message.Content = "You won't seem to help here!";
                    message.Components = null;
                }
            );
            break;
    }
}

