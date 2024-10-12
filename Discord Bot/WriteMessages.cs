using Discord;
using Discord.WebSocket;

namespace Discord_Bot;

[Command("writemessage", "Writes a Message")]
public class WriteMessages
{
    public void WriteMessageInChannel(Context context, string message)
    {
        var channel = context.GetCommand().Channel;
        channel.SendMessageAsync(message, embed: EmbedBuilder(context.GetCommand().User, Color.Blue));
    }

    private Embed EmbedBuilder(SocketUser user, Color color, string title = null, string text = null)
    {
        var builder = new EmbedBuilder()
        {
            Title = "Writing Messages",
            Author = new EmbedAuthorBuilder()
            {
                Name = "Leon"
            },
            Color = Color.Blue,
            Footer = new EmbedFooterBuilder()
            {
                Text = "footer Text"
            },
            Timestamp = DateTimeOffset.Now
        };
        return builder.Build();
    }
}