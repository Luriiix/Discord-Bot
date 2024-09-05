using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class Rps {
	private readonly DiscordSocketClient _client;
	
	public Rps(DiscordSocketClient client) {
		_client = client;
	}

	public void RpsGame(SocketSlashCommand command, bool isKiGame) {
		var builder = new ComponentBuilder().WithButton("Hit", "id1").WithButton("Smash", "id2").WithButton("Burst", "id3");
		command.ModifyOriginalResponseAsync(message => message.Components = builder.Build());
	}
}
