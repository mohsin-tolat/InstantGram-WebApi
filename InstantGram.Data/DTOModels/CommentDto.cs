using System;
using InstantGram.Common.Helper;
using InstantGram.Data.DBModels;
using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class CommentDto
    {
        [JsonIgnore]
        public int Id
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.commentId))
                {
                    return Convert.ToInt32(this.commentId);
                }

                return default;
            }
            set
            {
                this.commentId = value.ToString();
            }
        }

        private string commentId { get; set; }

        public string CommentHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.commentId))
                {
                    return this.commentId.ToEncrypt();
                }

                return default;
            }
            set
            {
                this.commentId = value.ToDecrypt().ToString();
            }
        }

        public PostDto PostDetails { get; set; }

        public UserDto CommentedBy { get; set; }

        public DateTime CommentedOn { get; set; }

        public string Content { get; set; }

        public int TotalCommentLikes { get; set; }

        public bool IsCurrentUserLikeComment { get; set; }

        public bool IsCurrentUserComment { get; set; }
    }
}