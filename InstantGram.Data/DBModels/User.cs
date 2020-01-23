using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class User
    {
        public User()
        {
            PostLike = new HashSet<PostLike>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime DateOfJoining { get; set; }

        public virtual ICollection<PostLike> PostLike { get; set; }
    }
}
