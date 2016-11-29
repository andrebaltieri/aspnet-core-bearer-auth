using DomainNotificationHelperCore.Commands;

namespace Balta.io.Domain.Commands.StudentCommands
{
    public class RegisterStudentCommand : Command
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Document { get; set; }
    }
}
