using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Darts.ReadModels
{
    public class UserProfile
    {
        public UserProfile(string email, string username)
        {
            Email = email;
            Username = username;
        }

        public int UserProfileId { get; }
        public string Email { get; }
        public string Username { get; }
    }
}
