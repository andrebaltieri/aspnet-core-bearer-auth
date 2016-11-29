using Balta.io.Domain.Commands.UserCommands;
using Balta.io.Domain.Enums;
using System;

namespace Balta.io.Domain.Entities
{
    public class User
    {
        protected User() { }
        public User(RegisterUserCommand command)
        {
            if (command.HasNotifications())
                return;

            Id = Guid.NewGuid();
            Username = command.Username;
            Password = command.Password;
            CreateDate = DateTime.Now;
            Active = false;
            Role = ERole.Free;

            EncryptPassword();
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public DateTime CreateDate { get; private set; }
        public bool Active { get; private set; }
        public ERole Role { get; private set; }

        private void EncryptPassword()
        {
            if (string.IsNullOrEmpty(Password))
                return ;

            Password += "|9d18ec10-81b3-4979-9c16-84afd37e599b.balta.io";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] data = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(Password));
            System.Text.StringBuilder sbString = new System.Text.StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sbString.Append(data[i].ToString("x2"));
            Password = sbString.ToString();
        }
    }
}