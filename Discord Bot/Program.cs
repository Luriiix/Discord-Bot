﻿using Discord;
using Discord_Bot;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton(new DiscordConfig());
services.AddSingleton<DiscordSocketClient>();

var app = services.BuildServiceProvider();
var client = app.GetRequiredService<DiscordSocketClient>();
client.Log += Log;

const string token = "MTI3OTgzODY3Mjk1MDM5OTA2OQ.GrVZnO.dr0PIqqe5qxCRlzG-xGQqVKAHyQ_B0kkfe3tIQ";

var handler = new CommandHandler(client, new CommandService());
await handler.InstallCommandsAsync();

await client.LoginAsync(TokenType.Bot, token);
await client.StartAsync();

await Task.Delay(-1);


Task Log(LogMessage msg) {
	Console.WriteLine(msg);
	return Task.CompletedTask;
}
