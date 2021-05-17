using Discord;
using DiscordBotFramework.Utilities;
using System.Collections.Generic;

namespace DiscordBotFramework.DefaultModules
{
    public class HelpBuilder
    {
        public Dictionary<string, string> HelpValues { get; set; } = new Dictionary<string, string>();

        public HelpBuilder() { }

        public Embed BuildHelpEmbed()
        {
            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Valid commands:",
                Color = new Color(ColorUtil.RandomColorGenerate())
            };

            foreach(KeyValuePair<string, string> kvp in HelpValues)
            {
                embed.AddField(kvp.Key, kvp.Value);
            }

            return embed.Build();
        }
    }
}
