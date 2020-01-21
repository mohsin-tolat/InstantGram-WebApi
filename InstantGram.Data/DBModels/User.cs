using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBmodels
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string EmailAddress { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime DateOfJoining { get; set; }

        public ICollection<Post> Posts { get; set; }

        public ICollection<PostLike> PostLikes { get; set; }
    }
}