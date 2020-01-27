using System;
using System.Collections.Generic;
using InstantGram.Common.Helper;
using InstantGram.Data.DBModels;
using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class UserDto
    {
        [JsonIgnore]
        public int Id
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.userId))
                {
                    return Convert.ToInt32(this.userId);
                }

                return default;
            }
            set
            {
                this.userId = value.ToString();
            }
        }

        private string userId { get; set; }

        public string UserHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.userId))
                {
                    return this.userId.ToEncrypt();
                }

                return default;
            }
            set
            {
                this.userId = value;
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Username { get; set; }

        public bool IsAlreadyFollowed { get; set; }

        public int TotalPostCount { get; set; }

        public DateTime DateOfJoining { get; set; }
        public string UserAvatar { get; set; }

        public List<UserFollower> UserFollowerFollowingUser { get; set; }

        public List<UserFollower> UserFollower { get; set; }

        public List<PostDto> AllUserPosts { get; set; }
        public bool IsCurrentUser { get; set; }
        public int TotalFollowers { get; set; }
        public int TotalFollowings { get; set; }

        public int UserPostPageNo { get; set; }

        public int UserPostPageSize { get; set; }
    }
}