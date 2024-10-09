using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

public class Component
{
	private List<Tuple<string, ButtonStyle, bool, IEmote>> Buttons { get; set; } = [];
	public Dictionary<string, Action<Context>> ButtonActions { get; set; } = new Dictionary<string, Action<Context>>();

    public void AddButton(string label, ButtonStyle buttonStyle = ButtonStyle.Primary, bool disabled = false, IEmote emote = null)
    {
	    var action = new Action<SocketMessageComponent>((component) =>
	    {
		    
	    });
	    
	    var button = Tuple.Create(label, buttonStyle, disabled, emote);
	    Buttons.Add(button);
	    //label, action);
    }

    public MessageComponent Build()
    {
	    var builder = new ComponentBuilder();
	    foreach (var button in Buttons)
	    {
		    builder.WithButton(label: button.Item1, style: button.Item2, disabled: button.Item3, emote: button.Item4, customId: button.Item1);
	    }
	    return builder.Build();
    }
}