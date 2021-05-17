using Discord;
using DiscordBotFramework.Utilities;

namespace DiscordBotFramework.DefaultModules
{
    public class InfoBuilder
    {
        public string Author { get; set; }
        public string GitHub { get; set; }
        public string ProjectRepo { get; set; }

        public InfoBuilder() { }

        public Embed BuildInfoEmbed()
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Information",
                Color = new Color(ColorUtil.RandomColorGenerate())
            };

            embed.AddField("Author:", Author);
            embed.AddField("GitHub:", $"[Profile]({GitHub}) [Project]({ProjectRepo})");

            return embed.Build();
        }
    }
}
