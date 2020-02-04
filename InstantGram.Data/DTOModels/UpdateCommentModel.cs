using System;
using InstantGram.Common.Helper;
using InstantGram.Data.DBModels;
using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class UpdateCommentModel
    {
        public string Content { get; set; }

        [JsonIgnore]
        public int PostId
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
                this.postId = value.ToDecrypt().ToString();
            }
        }


        [JsonIgnore]
        public int CommentedByUserId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.commentedByUserId))
                {
                    return Convert.ToInt32(this.commentedByUserId);
                }

                return default;
            }
            set
            {
                this.commentedByUserId = value.ToString();
            }
        }

        private string commentedByUserId { get; set; }

        public string CommentedByUserHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.commentedByUserId))
                {
                    return this.commentedByUserId.ToEncrypt();
                }

                return default;
            }
            set
            {
                this.commentedByUserId = value.ToDecrypt().ToString();
            }
        }
    }
}