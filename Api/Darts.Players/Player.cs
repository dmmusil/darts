﻿using Darts.Infrastructure;
using Darts.Players.Persistence;
using Darts.Players.Persistence.SQL;
using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using ValueOf;

namespace Darts.Players
{
    public class Player : Aggregate<PlayerState>
    {
        private Email Email { get; set; }
        private Password Password { get; set; }
        private string PasswordHash { get; set; }
        private Username Username { get; set; }

        public void Register(Username username, Password password, Email email)
        {
            Email = email;
            Password = password;
            Username = username;
        }

        public bool Authenticate(Password password)
        {
            return password == PasswordHash;
        }

        public override void Load(PlayerState state)
        {
            Id = state.Id;
            Email = state.Email;
            Username = state.Username;
            PasswordHash = state.Password;
        }

        public override PlayerState Memoize()
        {
            return new PlayerState(Id)
            {
                Username = Username,
                Email = Email,
                Password = Password
            };
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

    public class PlayerRepository : EntityFrameworkRepository<Player, PlayerState, PlayersContext>
    {
        public PlayerRepository(PlayersContext dbContext) : base(dbContext)
        {
        }
    }
}
