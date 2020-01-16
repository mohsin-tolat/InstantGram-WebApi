using System;

namespace InstantGram.Data.DBmodels
{
    public class PostLike
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        
        public DateTime LikeOn { get; set; }

        public int LikeBy { get; set; }

        public User User { get; set; }

        public Post Post { get; set; }
    }
}