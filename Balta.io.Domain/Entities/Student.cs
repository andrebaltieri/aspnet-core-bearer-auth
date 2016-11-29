using Balta.io.Domain.Commands.StudentCommands;
using Balta.io.Domain.Commands.UserCommands;
using System;

namespace Balta.io.Domain.Entities
{
    public class Student
    {
        protected Student() { }
        public Student(RegisterStudentCommand command)
        {
            if (command.HasNotifications())
                return;

            Id = Guid.NewGuid();
            FirstName = command.FirstName;
            LastName = command.LastName;
            Email = command.Email;
            Document = command.Document;
        }

        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Document { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public void AddUser(RegisterUserCommand command)
        {
            if (command.HasNotifications())
                return;

            User = new User(command);
        }
    }
}
