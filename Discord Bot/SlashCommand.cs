using System.Reflection.Metadata;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class SlashCommand()
{
    public string Name { get; init; }
    public string Description { get; init; }
    public DiscordSocketClient Client { get; init; }
    public List<SlashCommandOptionBuilder> Options { get; init; }
    
    public Parameter[] Parameters { get; init; }

    public Action<Context, string[]> Action { get; init; }

    public void AddOption(string optionName, string optionDescription, string[] choices, bool isRequired = false,
        ApplicationCommandOptionType optionType = ApplicationCommandOptionType.String)
    {
        var option = new SlashCommandOptionBuilder()
            .WithName(optionName)
            .WithDescription(optionDescription)
            .WithRequired(isRequired)
            .WithType(optionType);
        foreach (var t in choices)
        {
            option.AddChoice(t, t);
        }
        Options.Add(option);
        Console.WriteLine(Options.Count);
    }

    public async void Build()
    {
        var guild = Client.GetGuild(703363132126527569);
        var guildCommand = new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription(Description);
        if(Options != null) guildCommand.AddOptions(Options.ToArray());
        await guild.CreateApplicationCommandAsync(guildCommand.Build());
    }
}