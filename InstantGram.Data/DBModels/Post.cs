using System;

namespace InstantGram.Data.DBmodels
{
    public class Post
    {
        public int Id { get; set; }

        public int UploadBy { get; set; }

        public User User { get; set; }

        public string ContentLink { get; set; }

        public DateTime UploadOn { get; set; }
    }
}