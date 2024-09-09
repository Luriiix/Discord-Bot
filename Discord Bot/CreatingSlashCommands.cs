using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Serialization;

namespace Discord_Bot;

public class CreatingSlashCommands
{
    private DiscordSocketClient _client;

    public CreatingSlashCommands(DiscordSocketClient client)
    {
        _client = client;
    }

    public SlashCommand AddSlashCommand(string name, string description) {
        return new SlashCommand(name, description, _client, []);
        
    }
    
}

public class SlashCommand(string name, string description, DiscordSocketClient client, List<SlashCommandOptionBuilder> options){
    public void AddOption(string optionName, string optionDescription, string[] choices, bool isRequired = false, ApplicationCommandOptionType optionType = ApplicationCommandOptionType.String) {
        var option = new SlashCommandOptionBuilder()
            .WithName(optionName)
            .WithDescription(optionDescription)
            .WithRequired(isRequired)
            .WithType(optionType);
        foreach (var t in choices) {
            option.AddChoice(t, t);
        }

        options.Add(option);
        Console.WriteLine(options.Count);
    }

    public async void Build() {
        var guild = client.GetGuild(703363132126527569);
        var guildCommand = new SlashCommandBuilder()
            .WithName(name)
            .WithDescription(description)
            .AddOptions(options.ToArray());
        await guild.CreateApplicationCommandAsync(guildCommand.Build());
    }
}
