using System;
using InstantGram.Data.DBmodels;

namespace InstantGram.Data.DTOModels
{
    public class LoggedInUserModel
    {
        public string UserName { get; set; }

        public string Token { get; set; }
    }
}