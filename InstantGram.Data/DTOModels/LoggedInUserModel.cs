using System;
using InstantGram.Common.Helper;
using InstantGram.Data.DBmodels;
using Newtonsoft.Json;

namespace InstantGram.Data.DTOModels
{
    public class LoggedInUserModel
    {
        [JsonIgnore]
        public int UserId { get; set; }
        private string userId { get; set; }
        public string UserName { get; set; }

        public string EmailAddress { get; set; }
        
        public string UserHashId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    return userId;
                }

                return this.UserId.ToEncrypt();
            }
            set
            {
                this.userId = this.UserId.ToEncrypt();
            }
        }

        public string Token { get; set; }
    }
}