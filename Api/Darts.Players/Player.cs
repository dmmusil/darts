﻿using Darts.Infrastructure;
using Darts.Players.Persistence;
using Darts.Players.Persistence.SQL;
using OneOf;
using System;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ValueOf;

namespace Darts.Players
{
    public class Player : Aggregate<PlayerState>
    {
        private Email Email { get; set; }
        private string PasswordHash { get; set; }
        private Username Username { get; set; }
        private AuthToken AuthToken { get; set; }

        public void Register(Username username, Password password, Email email)
        {
            Email = email;
            PasswordHash = password;
            Username = username;
            AuthToken = new AuthToken();
        }

        public OneOf<AuthToken, bool> Authenticate(Password password)
        {
            return password == PasswordHash ? AuthToken : (OneOf<AuthToken, bool>)false;
        }

        protected override void ApplyEvent(Event e)
        {
            throw new NotImplementedException();
        }

        protected override void LoadInternal(PlayerState state)
        {
            Id = state.Id;
            Email = state.Email;
            Username = state.Username;
            PasswordHash = state.Password;
            AuthToken = state.AuthToken;
        }
    }

    public class AuthToken : ValueOf<Guid, AuthToken>
    {
        public static implicit operator string(AuthToken token) => token.Value.ToString("N");
        public static implicit operator Guid(AuthToken token) => token.Value;
        public static implicit operator AuthToken(Guid token) => From(token);
    }

    public class Username : ValueOf<string, Username>
    {
        public static implicit operator string(Username username) => username.Value;
        public static implicit operator Username(string username) => From(username);

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
        public PasswordComplexityException(string failureReason) : base(failureReason)
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
        public InvalidEmailException() : base("Invalid email address provided.")
        {
        }
    }

    public class PlayerRepository : EntityFrameworkRepository<Player, PlayerState, PlayersContext>
    {
        private readonly PlayersContext DbContext;

        public PlayerRepository(PlayersContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        protected override Task<PlayerState> LoadWithIncludes(Expression<Func<PlayerState, bool>> selector)
        {
            return DbContext.Players.SingleOrDefaultAsync(selector);
        }
    }
}
