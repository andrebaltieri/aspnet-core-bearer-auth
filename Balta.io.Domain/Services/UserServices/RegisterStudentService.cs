using Balta.io.Domain.Commands.StudentCommands;
using Balta.io.Domain.Commands.UserCommands;
using Balta.io.Domain.Entities;
using Balta.io.Domain.Repositories;
using DomainNotificationHelperCore.Assertions;
using DomainNotificationHelperCore.Commands;

namespace Balta.io.Domain.Services.UserServices
{
    public class RegisterStudentService : ServerCommand
    {
        private RegisterStudentCommand _studentCommand;
        private RegisterUserCommand _userCommand;
        private IStudentRepository _repository;

        public RegisterStudentService(
            RegisterStudentCommand studentCommand,
            RegisterUserCommand userCommand,
            IStudentRepository repository) : base(studentCommand)
        {
            _studentCommand = studentCommand;
            _userCommand = userCommand;
            _repository = repository;
        }

        public void Run()
        {
            Validate();
            if (HasNotifications())
                return;

            var student = new Student(_studentCommand);
            student.AddUser(_userCommand);

            _repository.Save(student);
        }

        public void Validate()
        {
            AddNotification(Assert.Length(_studentCommand.FirstName, 3, 40, "FirstName", "O nome deve conter entre 3 e 40 caracteres"));
            AddNotification(Assert.Length(_studentCommand.LastName, 3, 40, "LastName", "O sobrenome deve conter entre 3 e 40 caracteres"));            
            AddNotification(Assert.Length(_studentCommand.Document, 11, 11, "Document", "CPF inválido"));
            AddNotification(Assert.EmailIsValid(_studentCommand.Email, "Email", "E-mail inválido"));
            AddNotification(Assert.Length(_userCommand.Username, 6, 20, "Username", "O usuário deve conter entre 6 e 20 caracteres"));
            AddNotification(Assert.Length(_userCommand.Password, 6, 20, "Password", "A senha deve conter entre 6 e 20 caracteres"));
            AddNotification(Assert.Length(_userCommand.ConfirmPassword, 6, 20, "ConfirmPassword", "A senha deve conter entre 6 e 20 caracteres"));
            AddNotification(Assert.AreEquals(_userCommand.Password, _userCommand.ConfirmPassword, "ConfirmPassword", "As senhas digitadas não coincidem"));
        }
    }
}
