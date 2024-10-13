using System.Reflection;
using System.Reflection.Metadata;
using System.Threading.Channels;
using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class CreatingSlashCommands {
	private readonly DiscordSocketClient _client;
	public Dictionary<string, SlashCommand> Commands { get; set; }

	public CreatingSlashCommands(DiscordSocketClient client) {
		_client = client;
	}

	public SlashCommand RegisterCommands<T>() {
		var type = typeof(T);
		var commandAttribute = type.GetCustomAttribute<CommandAttribute>();
		if (commandAttribute == null)
			throw new NullReferenceException($"Command {type.Name} has no {nameof(CommandAttribute)}");

		var name = commandAttribute.Name;
		var description = commandAttribute.Description;

		var optionAttribute = type.GetCustomAttribute<OptionAttribute>();

		var action = (Context context) => {
			var argList = new List<object> { context };

			var classConstructor = type.GetConstructor(Type.EmptyTypes);
			if (classConstructor == null) throw new NullReferenceException();
			var classObject = classConstructor.Invoke([]);

			if (optionAttribute != null) {
				var choice = context.GetChoice();

				//Get Choice Methode
				MethodInfo methode = null;
				foreach (var m in type.GetMethods()) {
					var choiceAttribute = m.GetCustomAttribute<ChoiceAttribute>();
					if (choiceAttribute == null) {
						methode = m;
						continue;
					}

					if (choiceAttribute.Name == choice.ToString()) methode = m;
				}

				if (methode == null) throw new NullReferenceException();

				argList.Add(Parser.ParseMessageToParameters(context.Message, methode.GetParameters()));
				foreach (var s in argList) {
					Console.WriteLine(s);
				}
				methode.Invoke(classObject, argList.ToArray());
				return;
			}

			var method = type.GetMethods().First();
			argList.Add(Parser.ParseMessageToParameters(context.Message, method.GetParameters()));
			method.Invoke(classObject, argList.ToArray());
		};

		List<SlashCommandOptionBuilder> options = null;
		if (optionAttribute != null) options = [];

		var slashCommand = AddSlashCommand(name, description, action, options);
		var choices = GetChoices(type);

		if (optionAttribute != null) {
			slashCommand.AddOption(
				optionAttribute.Name, optionAttribute.Description, choices,
				optionAttribute.IsRequired, optionAttribute.OptionType
			);
		}

		slashCommand.Build();
		Commands.Add(name, slashCommand);
		return slashCommand;
	}

	private static string[] GetChoices(Type type) {
		var methods = type.GetMethods();

		return methods.Select(method => method.GetCustomAttribute<ChoiceAttribute>()).OfType<ChoiceAttribute>().Select(choiceAttribute => choiceAttribute.Name).ToArray();
	}

	private SlashCommand AddSlashCommand(
		string name, string description, Action<Context> action,
		List<SlashCommandOptionBuilder> options
	) {
		return new SlashCommand() {
			Name = name,
			Description = description,
			Client = _client,
			Options = options,
			Action = action
		};
	}
}
