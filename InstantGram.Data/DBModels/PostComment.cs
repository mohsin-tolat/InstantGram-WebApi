using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class PostComment
    {
        public PostComment()
        {
            CommentLike = new HashSet<CommentLike>();
        }

        public int Id { get; set; }
        public int PostId { get; set; }
        public int CommentedByUserId { get; set; }
        public DateTime CommentedOn { get; set; }
        public string CommentContent { get; set; }

        public virtual User CommentedByUser { get; set; }
        public virtual Post Post { get; set; }
        public virtual ICollection<CommentLike> CommentLike { get; set; }
    }
}
