using System.Reflection;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class CreatingSlashCommands
{
    private readonly DiscordSocketClient _client;
    public Dictionary<string, SlashCommand> Commands { get; private set; }

    public CreatingSlashCommands(DiscordSocketClient client)
    {
        _client = client;
    }

    public SlashCommand RegisterCommands<T>()
    {
        Commands = new Dictionary<string, SlashCommand>();

        var type = typeof(T);
        var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
        if (commandAttribute == null)
            throw new NullReferenceException($"Command {type.Name} has no {nameof(CommandAttribute)}");

        var name = commandAttribute.Name;
        var description = commandAttribute.Description;

        var optionAttribute = type.GetCustomAttribute<OptionAttribute>();

        var action = new Action<Context>((context) =>
        {
            var argList = new List<object> { context };

            var classConstructor = type.GetConstructor(Type.EmptyTypes);
            if (classConstructor == null) throw new NullReferenceException();
            var classObject = classConstructor.Invoke([]);

            if (optionAttribute != null)
            {
                var option = context.GetOption();

                var method = type.GetMethod(option.ToString());
                if (method == null) throw new NullReferenceException();

                method.Invoke(classObject, argList.ToArray());
                return;
            }

            var methode = type.GetMethods().First();
            methode.Invoke(classObject, argList.ToArray());
        });

        List<SlashCommandOptionBuilder> options = null;
        if (optionAttribute != null) options = [];

        var slashCommand = AddSlashCommand(name, description, action, options);

        if (optionAttribute != null)
        {
            slashCommand.AddOption(optionAttribute.Name, optionAttribute.Description, optionAttribute.Choices,
                optionAttribute.IsRequired, optionAttribute.OptionType);
        }

        slashCommand.Build();
        Commands.Add(name, slashCommand);
        return slashCommand;
    }


    private SlashCommand AddSlashCommand(string name, string description, Action<Context> action,
        List<SlashCommandOptionBuilder> options)
    {
        return new SlashCommand()
        {
            Name = name,
            Description = description,
            Client = _client,
            Options = options,
            Action = action
        };
    }
}