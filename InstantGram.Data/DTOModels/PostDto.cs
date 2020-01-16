using System;

namespace InstantGram.Data.DBmodels
{
    public class PostDto
    {
        public int Id { get; set; }

        public int UploadBy { get; set; }

        public User User { get; set; }

        public string ContentLink { get; set; }

        public DateTime UploadOn { get; set; }

        public int TotalLikes { get; set; }

        public string UploadedByUserName { get; set; }

        public string UploadedUserAvatar
        {
            get
            {
                return string.Format("https://gravatar.com/avatar/{0}?s=400&d=robohash&r=x", this.GetRandomString());
            }
            set
            {
                value = string.Empty;
            }
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}