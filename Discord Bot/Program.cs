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

async Task GuildCommand() {
	var guild = client.GetGuild(703363132126527569);

	var guildCommand = new SlashCommandBuilder()
		.WithName("test")
		.WithDescription("Description")
		.AddOption(
			new SlashCommandOptionBuilder()
				.WithName("option1")
				.WithDescription("description of option1")
				.WithRequired(true)
				.AddChoice("top", 1)
				.AddChoice("flop", 2)
				.WithType(ApplicationCommandOptionType.Integer)
		);

	await guild.CreateApplicationCommandAsync(guildCommand.Build());
}

async Task SlashCommandHandler(SocketSlashCommand command) {
	var builder = new ComponentBuilder().WithButton("Hit", "id1");

	await command.DeferAsync();
	await command.ModifyOriginalResponseAsync(
		message => {
			message.Content = "Loool";
			message.Components = builder.Build();
		}
	);
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
