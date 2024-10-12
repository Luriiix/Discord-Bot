using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot;

[Command("start", "Start a Game")]
[Option("game", "Choose a Game", true)]
public class StartCommand
{
    [Choice("RockPaperSiccors")]
    public async Task Rps(Context context)
    {
        Console.WriteLine("Started Rps Game");
        var command = context.GetCommand();
        
        var component = new Component();
        component.AddButton("Against Player");
        component.AddButton("Against KI");
        var messageComponent = component.Build();
        await command.ModifyOriginalResponseAsync(message =>
        {
            message.Content = "Welcome to the RPS Game!";
            message.Components = messageComponent;
        });
    }
}