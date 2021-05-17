using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBotFramework.Utilities;
using DiscordBotFramework.Enums;


namespace DiscordBotFramework.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commandService;
        private readonly DiscordSocketClient _discordSocketClient;
        private readonly IServiceProvider _serviceProvider;

        public CommandHandlingService(IServiceProvider provider)
        {
            _commandService = provider.GetRequiredService<CommandService>();
            _discordSocketClient = provider.GetRequiredService<DiscordSocketClient>();
            _serviceProvider = provider;

            // Hook CommandsExecuted to handle post-command execution logic
            _commandService.CommandExecuted += CommandExecutedAsync;

            // Hook MessageReceived so we can process each message to see if it qualifies as a command
            _discordSocketClient.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync() => await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            int argPos = 0;

            // Ignore system and bot messages
            if(!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            if (!message.HasCharPrefix(char.Parse(Properties.Settings.Default.BotPrefix), ref argPos)) return;

            SocketCommandContext context = new SocketCommandContext(_discordSocketClient, message);

            // Execute
            await _commandService.ExecuteAsync(context, argPos, _serviceProvider);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // Command is unspecified when there is a search failure; we normally don't care about these errors.
            if(!command.IsSpecified)
            {
                await context.Channel.SendMessageAsync("Sorry, I don't understand.");
            }

            // Otherwise, if the command was successful, we don't care about the result
            if (result.IsSuccess) return;

            // Otherwise, we've encountered a catastrophic failure and we want to log that error.
            string logDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + $"\\LogDump_{DateTime.Now.ToString($"dd_MM_yy")}.txt";
            List<LogUtility> CommandErrors = new List<LogUtility>();
            LogUtility.AddToLog(context.User.Username, context.Guild.Name, $"A catastrophic failure has occurred! Error: {result.ErrorReason}", CommandErrors, LogTypeEnum.Error);
            LogUtility.WriteLogFile(logDir, CommandErrors);

            await Task.CompletedTask;
        }
    }
}
