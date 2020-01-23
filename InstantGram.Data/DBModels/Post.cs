using System;
using System.Collections.Generic;

namespace InstantGram.Data.DBModels
{
    public partial class Post
    {
        public Post()
        {
            PostLike = new HashSet<PostLike>();
        }

        public int Id { get; set; }
        public int UploadBy { get; set; }
        public string ContentLink { get; set; }
        public DateTime UploadOn { get; set; }

        public virtual ICollection<PostLike> PostLike { get; set; }
    }
}
