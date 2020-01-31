using System;
using System.Collections.Generic;
using System.Linq;
using InstantGram.Common.Exceptions;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DBContexts;
using InstantGram.Data.DBModels;
using InstantGram.Data.DTOModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InstantGram.Core.Service
{
    public class UserService : IUserService
    {
        private ILogger<PostService> logger;
        private ApplicationDbContext context;

        public AppSettings appSettings { get; }

        public UserService(IOptions<AppSettings> appSettings, ILogger<PostService> logger, ApplicationDbContext context)
        {
            this.appSettings = appSettings.Value;
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
                                                        .OrderByDescending(usr => usr.DateOfJoining)
                                                        .Select(usr => new UserDto()
                                                        {
                                                            Id = usr.Id,
                                                            FirstName = usr.FirstName,
                                                            LastName = usr.LastName,
                                                            Username = usr.Username,
                                                            EmailAddress = usr.EmailAddress,
                                                            DateOfJoining = usr.DateOfJoining,
                                                            UserAvatar = usr.UserAvatar,
                                                            TotalPostCount = usr.Post.Count(),
                                                            IsAlreadyFollowed = usr.Id == currentUserId || usr.UserFollowerFollowingUser.Any(x => x.UserId == currentUserId)
                                                        }).GetPaged<UserDto>(pageNo, pageSize);

            this.logger.LogDebug("GetAllNewPostByUser End");
            return allUsers;
        }



        public bool FollowUnFollowUser(int currentUserId, int followingUserId, bool isFollow)
        {
            var userToFollow = this.context.User.FirstOrDefault(x => x.Id == followingUserId);
            if (userToFollow == null)
            {
                throw new DetailsNotFoundException("FollowingUserId", followingUserId);
            }

            if (isFollow)
            {
                return FollowUser(currentUserId, followingUserId);
            }
            else
            {
                return UnFollowUser(currentUserId, followingUserId);


            }
        }

        public UserDto GetUserDetails(int currentUserId, int userId)
        {
            var dbUserDetails = this.context.User.Include(x => x.Post)
                                                 .Include(x => x.UserFollowerUser)
                                                 .Include(x => x.UserFollowerFollowingUser)
                                                 .OrderByDescending(usr => usr.DateOfJoining)
                                                 .FirstOrDefault(x => x.Id == userId);
            if (dbUserDetails == null)
            {
                throw new DetailsNotFoundException("UserId", userId);
            }

            var userDetails = new UserDto()
            {
                Id = dbUserDetails.Id,
                FirstName = dbUserDetails.FirstName,
                LastName = dbUserDetails.LastName,
                Username = dbUserDetails.Username,
                EmailAddress = dbUserDetails.EmailAddress,
                DateOfJoining = dbUserDetails.DateOfJoining,
                UserAvatar = dbUserDetails.UserAvatar,
                TotalPostCount = dbUserDetails.Post.Count(),
                IsCurrentUser = dbUserDetails.Id == currentUserId,
                TotalFollowers = dbUserDetails.UserFollowerFollowingUser.Count(x => x.UserId == currentUserId),
                TotalFollowings = dbUserDetails.UserFollowerUser.Count(x => x.UserId == currentUserId),
                IsAlreadyFollowed = dbUserDetails.Id == currentUserId || dbUserDetails.UserFollowerFollowingUser.Any(x => x.UserId == currentUserId),
            };

            var dbUserPosts = dbUserDetails.Post.OrderByDescending(post => post.UploadOn).Select(userPosts => new PostDto()
            {
                Id = userPosts.Id,
                ContentLink = userPosts.ContentLink,
                TotalLikes = userPosts.PostLike.Count(),
                UploadBy = userPosts.UploadByUserId,
                UploadOn = userPosts.UploadOn,
                UploadedByUserName = userPosts.UploadByUser.Username,
                UploadedUserAvatar = userPosts.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = userPosts.PostLike.Any(z => z.LikeByUserId == currentUserId)
            }).GetPaged(appSettings.UIRenderSetting.PostPageNoForUserProfile, appSettings.UIRenderSetting.PostPageSizeForUserProfile);

            if (dbUserPosts != null && !dbUserPosts.Results.IsNullOrEmpty())
            {
                userDetails.UserPostPageNo = dbUserPosts.CurrentPage;
                userDetails.UserPostPageSize = dbUserPosts.PageSize;
                userDetails.UserPostPageCount = dbUserPosts.PageCount;
                userDetails.AllUserPosts = dbUserPosts.Results.ToList();
            }

            return userDetails;
        }

        private bool UnFollowUser(int currentUserId, int followingUserId)
        {
            var userFollowerDetails = this.context.UserFollower.Where(x => x.UserId == currentUserId && x.FollowingUserId == followingUserId).ToList();

            if (userFollowerDetails.IsNullOrEmpty())
            {
                return true;
            }

            this.context.RemoveRange(userFollowerDetails);
            return this.context.SaveChanges() > 0;
        }

        private bool FollowUser(int currentUserId, int followingUserId)
        {
            var userFollowerDetails = this.context.UserFollower.Where(x => x.UserId == currentUserId && x.FollowingUserId == followingUserId).ToList();

            if (!userFollowerDetails.IsNullOrEmpty())
            {
                return true;
            }

            this.context.UserFollower.Add(new UserFollower()
            {
                UserId = currentUserId,
                FollowingUserId = followingUserId,
                DateOfFollowing = CommonUtilities.GetCurrentDateTime()
            });

            return this.context.SaveChanges() > 0;
        }

        private string GetRandomString()
        {
            return Guid.NewGuid().ToString("N");
        }


    }
}