using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class PostLike
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int LikeByUserId { get; set; }
        public DateTime LikeOn { get; set; }

        public virtual User LikeByUser { get; set; }
        public virtual Post Post { get; set; }
    }
}
