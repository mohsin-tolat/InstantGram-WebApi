using System;
using InstantGram.Data.DBModels;

namespace InstantGram.Data.DTOModels
{
    public class PostDto
    {
        public int Id { get; set; }

        public int UploadBy { get; set; }

        public User User { get; set; }

        public string ContentLink { get; set; }

        public DateTime UploadOn { get; set; }

        public int TotalLikes { get; set; }

        public bool IsCurrentUserLikedPost { get; set; }

        public string UploadedByUserName { get; set; }

        public string UploadedUserAvatar { get; set; }
    }
}