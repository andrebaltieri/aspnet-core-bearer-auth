using DomainNotificationHelperCore.Commands;

namespace Balta.io.Domain.Commands.UserCommands
{
    public class RegisterUserCommand : Command
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
