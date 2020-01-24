using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class UserFollower
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FollowingUserId { get; set; }
        public DateTime DateOfFollowing { get; set; }

        public virtual User FollowingUser { get; set; }
        public virtual User User { get; set; }
    }
}
