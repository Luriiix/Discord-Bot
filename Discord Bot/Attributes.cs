using Discord;

namespace Discord_Bot;

public class CommandAttribute : Attribute {
	public string Name { get; }
	public string Description { get; }

	public CommandAttribute(string name, string description) {
		Name = name;
		Description = description;
	}
}

public class OptionAttribute : Attribute {
	public string Name { get; }
	public string Description { get; }
	public bool IsRequired { get; }
	public ApplicationCommandOptionType OptionType { get; }

	public OptionAttribute(
		string name, string description, bool isRequired = false,
		ApplicationCommandOptionType optionType = ApplicationCommandOptionType.String
	) {
		Name = name;
		Description = description;
		IsRequired = isRequired;
		OptionType = optionType;
	}
}

public class ChoiceAttribute : Attribute {
	public string Name { get; init; }

	public ChoiceAttribute(string name) {
		Name = name;
	}
}
