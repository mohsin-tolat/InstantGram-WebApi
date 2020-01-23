using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DBContexts;
using InstantGram.Data.DBModels;
using InstantGram.Data.DTOModels;
using Microsoft.Extensions.Logging;

namespace InstantGram.Core.Service
{
    public class UserService : IUserService
    {
        private ILogger<PostService> logger;
        private ApplicationDbContext context;

        public UserService(ILogger<PostService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public bool AddNewUser(UserRegistration registrationDetails)
        {
            User newUser = new User()
            {
                FirstName = registrationDetails.FirstName,
                LastName = registrationDetails.LastName,
                Username = registrationDetails.Username,
                EmailAddress = registrationDetails.EmailAddress,
                DateOfJoining = CommonUtilities.GetCurrentDateTime(),
                UserAvatar = string.Format("https://gravatar.com/avatar/{0}?s=400&d=robohash&r=x", this.GetRandomString())
            };

            var passwordSalt = CommonUtilities.GeneratePasswordSalt();
            newUser.PasswordSalt = passwordSalt;
            newUser.PasswordHash = CommonUtilities.GenerateHashPassword(registrationDetails.Password, passwordSalt);


            this.context.User.Add(newUser);
            return context.SaveChanges() > 0;
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}