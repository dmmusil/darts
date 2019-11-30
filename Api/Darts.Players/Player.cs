using Darts.Infrastructure;
using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using ValueOf;

namespace Darts.Players
{
    public class Player : Aggregate
    {
        protected override void Apply(Event e)
        {
            switch (e)
            {
                case UserRegistered _:
                    Identifier = new Id();
                    break;
            }
        }

        public void Create(Username username, Password password, Email email)
        {
            ApplyEvent(new UserRegistered(username, password, email));
        }
    }

    internal class UserRegistered : Event
    {
        public string Username { get; }
        public string PasswordHash { get; }
        public string Email { get; }

        public UserRegistered(string username, string passwordHash, string email)
        {
            Username = username;
            PasswordHash = passwordHash;
            Email = email;
        }
    }

    public class Username : ValueOf<string, Username>
    {
        public static implicit operator string(Username username) => username.Value;

    }

    public class Password : ValueOf<string, Password>
    {
        private static Password Hash(string plainText)
        {
            if (plainText.Length < 6)
            {
                throw new PasswordComplexityException("Passwords must be at least 6 characters in length.");
            }

            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
            return From(Encoding.UTF8.GetString(hash));
        }

        public static implicit operator Password(string password) => Hash(password);
        public static implicit operator string(Password password) => password.Value;

    }

    internal class PasswordComplexityException : Exception
    {
        public PasswordComplexityException(string message) : base(message)
        {
        }
    }

    public class Email : ValueOf<string, Email>
    {
        protected override void Validate()
        {
            new MailAddress(Value);
        }

        public static implicit operator string(Email email) => email.Value;
    }
}
