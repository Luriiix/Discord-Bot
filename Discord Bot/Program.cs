using Discord;
using Discord_Bot;
using Discord.Commands;
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


Task Log(LogMessage msg) {
	Console.WriteLine(msg);
	return Task.CompletedTask;
}

Task GuildCommand() {
	var creatingSlashCommands = app.GetRequiredService<CreatingSlashCommands>();
	var slashCommand = creatingSlashCommands.AddSlashCommand("start", "start a Game");
	slashCommand.AddOption("rps", "start rock-paper-scissors", ["against KI", "against other player"], true);
	slashCommand.Build();
	return Task.CompletedTask;
}

async Task SlashCommandHandler(SocketSlashCommand command) {
	await command.DeferAsync();
	var option = "";
	var choice = "";
	
	if(command.Data.Options.First() != null) option = command.Data.Options.First().Name;
	if (command.Data.Options.First().Value != null) choice = command.Data.Options.First().Value.ToString();
	
	
	switch (command.CommandName) {
		
		case "start":
			switch (option) {
				
				case "rps":
					switch (choice) {
						
					 	case "against KI":
							await command.ModifyOriginalResponseAsync(
								message => message.Content = " You are about to start a game against KI."
							);
							app.GetRequiredService<Rps>().RpsGame(command, true);
							break;
					}

					break;
			}

			break;
	}
}

async Task MyButtonHandler(SocketMessageComponent command) {
	switch (command.Data.CustomId) {
		case "id1":
			await command.DeferAsync();
			await command.Channel.SendMessageAsync("lol");
			await command.ModifyOriginalResponseAsync(
				message => {
					message.Content = "You won't seem to help here!";
					message.Components = null;
				}
			);
			break;
	}
}

void WriteMessage(string message)
 {
	 
 }
