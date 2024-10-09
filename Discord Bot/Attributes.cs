using Discord;

namespace Discord_Bot;

public class CommandAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }

    public CommandAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }
}

public class OptionAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }
    public string[] Choices { get; }
    public bool IsRequired { get; }
    public ApplicationCommandOptionType OptionType {get;}

    public OptionAttribute(string name, string description, string[] choices = null, bool isRequired = false, ApplicationCommandOptionType optionType = ApplicationCommandOptionType.String)
    {
        Name = name;
        Description = description;
        Choices = choices;
        IsRequired = isRequired;
        OptionType = optionType;
    }
}