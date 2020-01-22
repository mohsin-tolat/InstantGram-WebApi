using System;
using InstantGram.Common.Helper;
using InstantGram.Data.DBmodels;

namespace InstantGram.Data.DTOModels
{
    public class LoggedInUserModel
    {
        public int UserId { get; set; }
        private string _userId { get; set; }
        public string UserName { get; set; }

        public string UserHashId
        {
            get { return _userId; }
            set
            {
                this._userId = this.UserId.ToEncrypt();
            }
        }

        public string Token { get; set; }
    }
}