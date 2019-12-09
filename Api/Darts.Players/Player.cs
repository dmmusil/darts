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
        private string Password;

        protected override void Apply(Event e)
        {
            switch (e)
            {
                case UserRegistered registered:
                    Identifier = registered.Username;
                    Password = registered.PasswordHash;
                    break;
            }
        }

        public void Register(Username username, Password password, Email email)
        {
            ApplyEvent(new UserRegistered(username, password, email));
        }

        public void Authenticate(Password password)
        {
            if (password != Password)
            {
                throw new AuthenticationFailedException();
            }
        }
    }

    internal class AuthenticationFailedException : DomainException
    {
        public AuthenticationFailedException() : base(400, "Incorrect username or password")
        {
        }
    }

    public class UserRegistered : Event
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
        public static implicit operator Username(string username) => From(username);

    }

    public class SecureUsername : ValueOf<string, SecureUsername>
    {
        private static SecureUsername Hash(string plainText)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                var hash = Convert.ToBase64String(hashBytes);
                return From(hash);
            }
        }

        public static implicit operator string(SecureUsername username) => username.Value;
        public static implicit operator SecureUsername(string username) => Hash(username);

    }

    public class Password : ValueOf<string, Password>
    {
        private static Password Hash(string plainText)
        {
            if (plainText.Length < 6)
            {
                throw new PasswordComplexityException("Passwords must be at least 6 characters in length.");
            }

            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                return From(Convert.ToBase64String(hash));
            }
        }

        public static implicit operator Password(string password) => Hash(password);
        public static implicit operator string(Password password) => password.Value;

    }

    internal class PasswordComplexityException : DomainException
    {
        public PasswordComplexityException(string failureReason) : base(400, failureReason)
        {
        }
    }

    public class Email : ValueOf<string, Email>
    {
        protected override void Validate()
        {
            try
            {
                var _ = new MailAddress(Value);
            }
            catch (FormatException)
            {
                throw new InvalidEmailException();
            }
        }

        public static implicit operator string(Email email) => email.Value;
        public static implicit operator Email(string email) => From(email);
    }

    internal class InvalidEmailException : DomainException
    {
        public InvalidEmailException() : base(400, "Invalid email address provided.")
        {
        }
    }
}
