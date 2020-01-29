using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DBContexts;
using InstantGram.Data.DBModels;
using InstantGram.Data.DTOModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InstantGram.Core.Service
{
    public class PostService : IPostService
    {
        private ILogger<PostService> logger;
        private ApplicationDbContext context;

        public PostService(ILogger<PostService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public PagedResult<PostDto> GetAllNewPostByUser(int currentLoggedInUserId, int pageNo, int pageSize)
        {
            this.logger.LogDebug("GetAllNewPostByUser Started");

            var allPosts = (from userFollower in this.context.UserFollower.Where(x => x.UserId == currentLoggedInUserId)
                            join userPosts in this.context.Post on userFollower.FollowingUserId equals userPosts.UploadByUserId
                            orderby userPosts.UploadOn descending
                            select new PostDto()
                            {
                                Id = userPosts.Id,
                                ContentLink = userPosts.ContentLink,
                                TotalLikes = userPosts.PostLike.Count(),
                                UploadBy = userPosts.UploadByUserId,
                                UploadOn = userPosts.UploadOn,
                                UploadedByUserName = userPosts.UploadByUser.Username,
                                UploadedUserAvatar = userPosts.UploadByUser.UserAvatar,
                                IsCurrentUserLikedPost = userPosts.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId)
                            }).GetPaged<PostDto>(pageNo, pageSize);

            this.logger.LogDebug("GetAllNewPostByUser End");
            return allPosts;
        }

        public PagedResult<PostDto> GetAllOpenPosts(int currentLoggedInUserId, int pageNo, int pageSize, int userId = 0)
        {
            var allPosts = this.context.Post.Where(x => userId == 0 || x.UploadByUserId == userId).OrderByDescending(post => post.UploadOn).Select(x => new PostDto()
            {
                Id = x.Id,
                ContentLink = x.ContentLink,
                TotalLikes = x.PostLike.Count(),
                UploadBy = x.UploadByUserId,
                UploadOn = x.UploadOn,
                UploadedByUserName = x.UploadByUser.Username,
                UploadedUserAvatar = x.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = x.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId)
            }).GetPaged<PostDto>(pageNo, pageSize);

            return allPosts;
        }

        public PostDto GetPostById(int currentLoggedInUserId, int postId)
        {
            var postDetails = this.context.Post.Where(x => x.Id == postId).OrderByDescending(post => post.UploadOn).Select(x => new PostDto()
            {
                Id = x.Id,
                ContentLink = x.ContentLink,
                TotalLikes = x.PostLike.Count(),
                UploadBy = x.UploadByUserId,
                UploadOn = x.UploadOn,
                UploadedByUserName = x.UploadByUser.Username,
                UploadedUserAvatar = x.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = x.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId)
            }).FirstOrDefault();

            return postDetails;
        }



        public PostDto LikeDislikePost(int currentUserId, int postId)
        {
            var postDetails = this.context.Post.Include(x => x.PostLike)
                                               .Include(x => x.UploadByUser)
                                               .Where(x => x.Id == postId)
                                               .OrderByDescending(post => post.UploadOn)
                                               .FirstOrDefault();

            if (postDetails == null)
            {
                return null;
            }

            if (!postDetails.PostLike.Any(x => x.LikeByUserId == currentUserId))
            {
                this.context.PostLike.Add(new PostLike()
                {
                    LikeByUserId = currentUserId,
                    PostId = postId,
                    LikeOn = CommonUtilities.GetCurrentDateTime()
                });
            }
            else
            {
                if (postDetails.PostLike.Any(x => x.LikeByUserId == currentUserId))
                {
                    this.context.PostLike.RemoveRange(postDetails.PostLike.Where(x => x.LikeByUserId == currentUserId).ToList());
                }
            }

            this.context.SaveChanges();

            return new PostDto()
            {
                Id = postDetails.Id,
                ContentLink = postDetails.ContentLink,
                TotalLikes = postDetails.PostLike.Count(),
                UploadBy = postDetails.UploadByUserId,
                UploadOn = postDetails.UploadOn,
                UploadedByUserName = postDetails.UploadByUser.Username,
                UploadedUserAvatar = postDetails.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = postDetails.PostLike.Any(z => z.LikeByUserId == currentUserId)
            };
        }

        public bool SavePostContentToFolderAndDatabase(int currentLoggedInUserId, string instantGramApiUrl, string postContent)
        {
            var postContentLocal = postContent.Replace("data:image/jpeg;base64,", string.Empty);
            var folderName = Path.Combine("Resources", "UserPosts", currentLoggedInUserId.ToString());
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string imageName = CommonUtilities.GenerateRandomString() + ".jpg";
            var isImageSaved = this.SaveImageToFolder(postContentLocal, imageName, pathToSave);
            if (!isImageSaved)
            {
                return false;
            }

            var dbPath = Path.Combine(instantGramApiUrl, folderName, imageName);
            return this.SaveImageDetailsIntoDatabase(currentLoggedInUserId, dbPath);
        }

        private bool SaveImageDetailsIntoDatabase(int currentLoggedInUserId, string dbPath)
        {
            this.context.Post.Add(new Post()
            {
                UploadByUserId = currentLoggedInUserId,
                ContentLink = $"{dbPath}",
                UploadOn = CommonUtilities.GetCurrentDateTime(),
            });

            return this.context.SaveChanges() > 0;
        }

        private bool SaveImageToFolder(string postContent, string imgName, string pathToFolder)
        {
            //Check if directory exist
            if (!System.IO.Directory.Exists(pathToFolder))
            {
                System.IO.Directory.CreateDirectory(pathToFolder); //Create directory if it doesn't exist
            }

            //set the image path
            string imgPath = Path.Combine(pathToFolder, imgName);

            byte[] imageBytes = Convert.FromBase64String(postContent);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }
    }
}