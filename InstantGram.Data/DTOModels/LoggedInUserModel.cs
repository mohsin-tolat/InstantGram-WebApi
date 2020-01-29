using System;
using InstantGram.Common.Helper;

using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class LoggedInUserModel
    {
        public string Username { get; set; }

        public string EmailAddress { get; set; }

        [JsonIgnore]
        public int UserId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.userId))
                {
                    return Convert.ToInt32(this.userId);
                }

                return default;
            }
            set
            {
                this.userId = value.ToString();
            }
        }

        private string userId { get; set; }

        public string UserHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.userId))
                {
                    return this.userId.ToEncrypt();
                }

                return default;
            }
            set
            {
                this.userId = value;
            }
        }
        
        public string Token { get; set; }
    }
}