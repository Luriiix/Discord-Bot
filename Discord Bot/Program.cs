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

var path = @"..\..\..\token.txt";

if (!File.Exists(path)) {
    string input;
    while (true) {
        Console.Write("Please enter the token of the Bot you want to run: ");
        input = Console.ReadLine();
        if (input == null) continue;
        break;
    }
    await using var writer = File.CreateText(path);
    await writer.WriteAsync(input);
}
var token = await File.ReadAllTextAsync(path);


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
    creatingSlashCommands.Commands = new Dictionary<string, SlashCommand>();
    var startCommand = creatingSlashCommands.RegisterCommands<StartCommand>();
    var messageCommand = creatingSlashCommands.RegisterCommands<WriteMessages>();
    
    startCommand.Build();
    return Task.CompletedTask;
}

async Task SlashCommandHandler(SocketSlashCommand slashCommand) {
    //var commandMessage = "!lol (HalLoooooooo)";
    
    await slashCommand.DeferAsync();
    var creatingSlashCommands = app.GetRequiredService<CreatingSlashCommands>();
    var commands = creatingSlashCommands.Commands;
    
    var command = commands[slashCommand.CommandName];
    command.Action(new Context(client, slashCommand, ""));
    
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

