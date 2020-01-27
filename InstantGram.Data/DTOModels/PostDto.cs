using System;
using InstantGram.Common.Helper;
using InstantGram.Data.DBModels;
using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class PostDto
    {
        [JsonIgnore]
        public int Id
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.postId))
                {
                    return Convert.ToInt32(this.postId);
                }

                return default;
            }
            set
            {
                this.postId = value.ToString();
            }
        }

        private string postId { get; set; }

        public string PostHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.postId))
                {
                    return this.postId.ToEncrypt();
                }

                return default;
            }
            set
            {
                this.postId = value;
            }
        }

        [JsonIgnore]
        public int UploadBy
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.uploadByUserId))
                {
                    return Convert.ToInt32(this.uploadByUserId);
                }

                return default;
            }
            set
            {
                this.uploadByUserId = value.ToString();
            }
        }

        public string uploadByUserId { get; set; }

        public string UploadedByUserHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.uploadByUserId))
                {
                    return this.uploadByUserId.ToEncrypt();
                }

                return default;
            }
            set
            {
                this.uploadByUserId = value;
            }
        }



        public string ContentLink { get; set; }

        public DateTime UploadOn { get; set; }

        public int TotalLikes { get; set; }

        public bool IsCurrentUserLikedPost { get; set; }

        public string UploadedByUserName { get; set; }

        public string UploadedUserAvatar { get; set; }
    }
}