using Discord;
using Discord.Commands;
using DiscordBotFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotFramework.DefaultModules
{
    public class ModeratorTools
    {
        public ModeratorTools() { }

        public async Task BanUsersAsync(SocketCommandContext Context, IGuildUser user, string reason)
        {
            var id = user.Id;

            await Context.Channel.SendMessageAsync($"<@{id}> has been banned.");

            if(reason != null)
            {
                await Context.Channel.SendMessageAsync($"Reason: {reason}");
            }
            else
            {
                await Context.Channel.SendMessageAsync("No reason was provided.");
            }

            await user.Guild.AddBanAsync(user, reason: reason);
            await user.SendMessageAsync($"You have been banned from {Context.Guild.Name}. Reason: {reason}");
        }

        // Kick a user
        public async Task KickUsersAsync(SocketCommandContext Context, IGuildUser user, string reason)
        {
            var id = user.Id;

            await Context.Channel.SendMessageAsync($"<@{id}> has been kicked.");

            if(reason != null)
            {
                await Context.Channel.SendMessageAsync($"Reason: {reason}");
            }

            else
            {
                await Context.Channel.SendMessageAsync("No reason was provided.");
            }

            await user.KickAsync();
            await user.SendMessageAsync($"You have been kicked from {Context.Guild.Name}. Reason: {reason}");
        }

        public async Task ShutdownAsync(SocketCommandContext Context)
        {
            await Context.Channel.SendMessageAsync("Shutting down. Bye!");
            Environment.Exit(0);
        }

        public async Task ChangeStatusAsync(SocketCommandContext Context, string status)
        {
            await Context.Client.SetGameAsync(status);
            await Context.Channel.SendMessageAsync($"Aye, captain. Changing my status to: {status}");
        }

        public async Task BotStatus(SocketCommandContext Context, bool verbose)
        {
            List<string> GuildNames = new List<string>();
            List<string> GuildOwners = new List<string>();
            List<int> GuildTotalUsers = new List<int>();

            var watch = Stopwatch.StartNew();

            foreach (Discord.WebSocket.SocketGuild guild in Context.Client.Guilds)
            {
                GuildNames.Add(guild.Name);
                GuildOwners.Add(guild.Owner.Username);
                GuildTotalUsers.Add(guild.Users.Count());
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            var resultFooter = new EmbedFooterBuilder().WithText($"Time elapsed: {elapsedMs}ms");

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Bot Status",
                Color = new Color(ColorUtil.RandomColorGenerate()),
                Footer = resultFooter
            }.WithCurrentTimestamp();

            if (!verbose)
            {
                embed.AddField($"**Guild Count**", $"**{GuildNames.Count()}**");
                embed.AddField($"**Total Users**", $"**{GuildTotalUsers.Sum(x => Convert.ToInt32(x))}**");
            }
            else
            {
                embed.AddField($"Guild Count:", $"{GuildNames.Count()}");
                foreach (string s in GuildNames)
                {
                    int i = GuildNames.IndexOf(s);
                    embed.AddField($"**{s}**", $"**Owned by:** {GuildOwners[i]} \n **Total users:** {GuildTotalUsers[i]}");
                }
            }
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        public async Task BotUptime(SocketCommandContext Context)
        {
            DateTime Startup = Process.GetCurrentProcess().StartTime;
            TimeSpan uptime = DateTime.Now - Startup;

            string Days = $"{uptime.Days} day{(uptime.Days != 1 ? "s" : "")}, ";
            string Hours = $"{uptime.Hours} hour{(uptime.Hours != 1 ? "s" : "")}, ";
            string Mins = $"{uptime.Minutes} min{(uptime.Minutes != 1 ? "s" : "")}, and ";
            string Secs = $"{uptime.Seconds} second{(uptime.Seconds != 1 ? "s" : "")}";

            string UpString = Days + Hours + Mins + Secs;

            EmbedBuilder embed = new EmbedBuilder
            {
                Title = "Uptime",
                Color = new Color(ColorUtil.RandomColorGenerate())
            };

            embed.AddField("Current:", $"**{UpString}**");

            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

    }
}
