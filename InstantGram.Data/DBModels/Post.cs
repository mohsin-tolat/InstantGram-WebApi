using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class Post
    {
        public Post()
        {
            PostComment = new HashSet<PostComment>();
            PostLike = new HashSet<PostLike>();
        }

        public int Id { get; set; }
        public int UploadByUserId { get; set; }
        public string ContentLink { get; set; }
        public DateTime UploadOn { get; set; }

        public virtual User UploadByUser { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<PostLike> PostLike { get; set; }
    }
}
