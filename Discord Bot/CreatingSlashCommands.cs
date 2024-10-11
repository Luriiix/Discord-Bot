using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Channels;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class CreatingSlashCommands
{
    private readonly DiscordSocketClient _client;
    private readonly Parser _parser;
    public Dictionary<string, SlashCommand> Commands { get; private set; }

    public CreatingSlashCommands(DiscordSocketClient client, Parser parser)
    {
        _client = client;
        _parser = parser;
    }

    public SlashCommand RegisterCommands<T>()
    {
        Commands = new Dictionary<string, SlashCommand>();
        //_parser.AddAllParsers();

        var type = typeof(T);
        var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
        if (commandAttribute == null)
            throw new NullReferenceException($"Command {type.Name} has no {nameof(CommandAttribute)}");

        var name = commandAttribute.Name;
        var description = commandAttribute.Description;

        var optionAttribute = type.GetCustomAttribute<OptionAttribute>();

        var action = new Action<Context, string[]>((context, args) =>
        {
            var argList = new List<object> { context };
            var argsArray  = argList.ToArray();

            var classConstructor = type.GetConstructor(Type.EmptyTypes);
            if (classConstructor == null) throw new NullReferenceException();
            var classObject = classConstructor.Invoke([]);

            if (optionAttribute != null)
            {
                var option = context.GetOption();

                var method = type.GetMethod(option.ToString());
                if (method == null) throw new NullReferenceException();
                
                /*for (int i = 0; i < parameters.Length; i++)
                {
                    _parser.Parse(parameters[i], argsArray[i], out var arg);
                    argList.Add(arg);
                }*/
                
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

    public class Parser
    {
        private Dictionary<Type, Func<string, object>> _parser = [];
        public void Add<T>(string key, Context context, Func<string, object> parser)
        {
            _parser.Add(typeof(T), arg => parser(arg));
        }
        
        public void Parse(ParameterInfo parameter, string arg,  out object o)
        {
            _parser.TryGetValue(parameter.ParameterType, out var parser);
            
            if (parser == null) throw new NullReferenceException($"Parser for type {parameter.ParameterType} not found");
            
            o = parser(arg);
            
            if(o == null) throw new NullReferenceException($"Could not parse {arg} to type {parameter.ParameterType}");
        }

        public void AddAllParsers()
        {
            var type = typeof(IMessageChannel);
            type.GetField("channelname");


        }


    }


    private SlashCommand AddSlashCommand(string name, string description, Action<Context, string[]> action,
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