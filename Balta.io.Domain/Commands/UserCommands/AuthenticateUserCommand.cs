using DomainNotificationHelperCore.Commands;

namespace Balta.io.Domain.Commands.UserCommands
{
    public class AuthenticateUserCommand : Command
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
