using System.ComponentModel;
using System.Reflection;
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

    void RegisterCommands<T>()
    {
        var commands = new Dictionary<string, Command>();
        var t = typeof(T);

        foreach (var methode in t.GetMethods())
        {
            var name = methode.GetCustomAttribute<CommandAttribute>();
            if (name == null) continue;
            var description = methode.GetCustomAttribute<DescriptionAttribute>();
            if (description == null) continue;
            
            
        }
    }
}

public class SlashCommand(){
    public string Name { get; init; }
    public string Description { get; init; }
    public DiscordSocketClient Client { get; init; }
    public List<SlashCommandOptionBuilder> Options { get; init; }
    
    public Action<CommandContext, string> Action { get; init; }
    
    public void AddOption(string optionName, string optionDescription, string[] choices, bool isRequired = false, ApplicationCommandOptionType optionType = ApplicationCommandOptionType.String) {
        var option = new SlashCommandOptionBuilder()
            .WithName(optionName)
            .WithDescription(optionDescription)
            .WithRequired(isRequired)
            .WithType(optionType);
        foreach (var t in choices) {
            option.AddChoice(t, t);
        }

        Options.Add(option);
        Console.WriteLine(Options.Count);
    }

    public async void Build() {
        var guild = Client.GetGuild(703363132126527569);
        var guildCommand = new SlashCommandBuilder()
            .WithName(Name)
            .WithDescription(Description)
            .AddOptions(Options.ToArray());
        await guild.CreateApplicationCommandAsync(guildCommand.Build());
    }
}

class RpsCommand()
{
    [Command("start")]
    [Description("start a Game")]
    public void startGame(CommandContext context)
    {
        
    }
}