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

        public PagedResult<UserDto> GetUserDetailsBasedOnSearch(int currentUserId, string searchText, int pageNo, int pageSize)
        {
            this.logger.LogDebug("GetAllNewPostByUser Started");

            var allUsers = this.context.User.Where(x => string.IsNullOrWhiteSpace(searchText)
                                                        || (x.FirstName.Contains(searchText)
                                                        || x.LastName.Contains(searchText)
                                                        || x.Username.Contains(searchText)))
                                                        .Select(usr => new UserDto()
                                                        {
                                                            FirstName = usr.FirstName,
                                                            LastName = usr.LastName,
                                                            Username = usr.Username,
                                                            EmailAddress = usr.EmailAddress,
                                                            DateOfJoining = usr.DateOfJoining,
                                                            UserAvatar = usr.UserAvatar
                                                            // TODO: Need to check this.
                                                            // IsAlreadyFollowed=usr.UserFollowerFollowingUser.Any(x=>x.)
                                                        }).GetPaged<UserDto>(pageNo, pageSize);

            this.logger.LogDebug("GetAllNewPostByUser End");
            return allUsers;
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}