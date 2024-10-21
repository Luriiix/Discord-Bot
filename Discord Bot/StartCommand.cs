using Discord;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot;

[Command("start", "Start a Game")]
[Option("game", "Choose a Game", true)]
public class StartCommand
{
    [Choice("Rps")]
    public async Task Rps(Context context)
    {
        Console.WriteLine("Started Rps Game");
        var command = context.GetCommand();
        
        var component = new Component();
        var button = new CreatingButtons().RegisterButton("label");
        component.AddButton(button);
        var messageComponent = component.Build();
        await command.ModifyOriginalResponseAsync(message =>
        {
            message.Content = "Welcome to the RPS Game!";
            message.Components = messageComponent;
        });
    }
}