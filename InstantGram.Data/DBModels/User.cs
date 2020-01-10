using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        public ICollection<Post> Posts { get; set; }

    }
}