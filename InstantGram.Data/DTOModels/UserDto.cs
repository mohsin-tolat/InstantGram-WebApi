using System;

namespace InstantGram.Data.DTOModels
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }

        public bool IsAlreadyFollowed { get; set; }

        public int TotalPostCount { get; set; }

        public DateTime DateOfJoining { get; set; }
        public string UserAvatar { get; set; }
    }
}