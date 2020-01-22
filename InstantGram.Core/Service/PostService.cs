using System;
using System.Collections.Generic;
using System.Linq;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DbContexts;
using InstantGram.Data.DBmodels;
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

        public PagedResult<PostDto> GetAllNewPostByUser(int userId, int pageNo, int pageSize)
        {
            this.logger.LogDebug("GetAllNewPostByUser Started");
            var allPosts = this.context.Post.Select(x => new PostDto()
            {
                Id = x.Id,
                ContentLink = x.ContentLink,
                TotalLikes = x.PostLikes.Count(),
                UploadBy = x.UploadBy,
                UploadOn = x.UploadOn,
                UploadedByUserName = x.User.Username,
            }).GetPaged<PostDto>(pageNo, pageSize);

            this.logger.LogDebug("GetAllNewPostByUser End");
            return allPosts;
        }

        public PostDto LikeDislikePost(int currentUserId, int postId)
        {
            var postDetails = this.context.Post.Include(x => x.PostLikes)
                                               .Include(x => x.User)
                                               .Where(x => x.Id == postId)
                                               .FirstOrDefault();

            if (postDetails == null)
            {
                return null;
            }

            if (!postDetails.PostLikes.Any(x => x.LikeBy == currentUserId))
            {
                this.context.PostLike.Add(new PostLike()
                {
                    LikeBy = currentUserId,
                    PostId = postId,
                    LikeOn = CommonUtilities.GetCurrentDateTime()
                });
            }
            else
            {
                if (postDetails.PostLikes.Any(x => x.LikeBy == currentUserId))
                {
                    this.context.PostLike.RemoveRange(postDetails.PostLikes.Where(x => x.LikeBy == currentUserId).ToList());
                }
            }

            this.context.SaveChanges();

            return new PostDto()
            {
                Id = postDetails.Id,
                ContentLink = postDetails.ContentLink,
                TotalLikes = postDetails.PostLikes.Count(),
                UploadBy = postDetails.UploadBy,
                UploadOn = postDetails.UploadOn,
                UploadedByUserName = postDetails.User.Username,
            };
        }
    }
}