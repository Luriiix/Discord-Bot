using Discord;
using Discord.WebSocket;

public class Program
{
    private static DiscordSocketClient _client;
    
    public static async Task Main()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;
        
        const string token = "MTI3OTgzODY3Mjk1MDM5OTA2OQ.GrVZnO.dr0PIqqe5qxCRlzG-xGQqVKAHyQ_B0kkfe3tIQ";
        
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        
        await Task.Delay(-1);
    }
    
    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}