using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class Component
{
    private List<Button> Buttons = [];

    public void AddButton(Button button)
    {
        Buttons.Add(button);
    }

    public MessageComponent Build()
    {
        var builder = new ComponentBuilder();
        foreach (var button in Buttons)
        {
            builder.WithButton(label: button.Label, style: button.Style, disabled: button.Disabled, emote: button.Emote,
                customId: button.CustomId);
        }

        return builder.Build();
    }
}

public class CreatingButtons
{
    public Dictionary<string, Button> ButtonActions { get; set; } = new();

    public Button RegisterButton(string label, ButtonStyle style = ButtonStyle.Premium, bool disabled = false,
        IEmote emote = null)
    {
        var task = Task.Run(() =>
        {
            Console.WriteLine($"Registering button {label}");
            Task.Delay(1000).Wait();
        });

        var customId = Guid.NewGuid().ToString();
        var button = new Button
        {
            Label = label, Style = style, Disabled = disabled, Emote = emote, CustomId = customId, CustomTask = task
        };
        ButtonActions.Add(customId, button);
        return button;
    }
}

public class Button
{
    public string Label { get; init; }
    public ButtonStyle Style { get; init; }
    public bool Disabled { get; init; }
    public IEmote Emote { get; init; }
    public string CustomId { get; init; }

    public Task CustomTask { get; init; }
}