using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class CommentLike
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int LikeByUserId { get; set; }
        public DateTime LikeOn { get; set; }

        public virtual PostComment Comment { get; set; }
        public virtual User LikeByUser { get; set; }
    }
}
