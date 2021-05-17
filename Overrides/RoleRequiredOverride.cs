using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBotFramework.Overrides
{
    public class RequireRoleAttribute : PreconditionAttribute
    {
        private readonly string Name;

        public RequireRoleAttribute(string name) => Name = name;

        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider svc)
        {
            if (context.User is SocketGuildUser gUser)
            {
                if (gUser.Roles.Any(r => r.Name == Name))
                {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
                else
                {
                    return Task.FromResult(PreconditionResult.FromError($"You must have the role '{Name}' to run this command."));
                }
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError("You must be in a server to run this command."));
            }
        }
    }
}
