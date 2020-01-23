using System;
using System.Collections.Generic;
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

        public PagedResult<PostDto> GetAllNewPostByUser(int userId, int pageNo, int pageSize)
        {
            this.logger.LogDebug("GetAllNewPostByUser Started");
            var allPosts = this.context.Post.Select(x => new PostDto()
            {
                Id = x.Id,
                ContentLink = x.ContentLink,
                TotalLikes = x.PostLike.Count(),
                UploadBy = x.UploadByUserId,
                UploadOn = x.UploadOn,
                UploadedByUserName = x.UploadByUser.Username,
                IsCurrentUserLikedPost = x.PostLike.Any(z => z.LikeByUserId == userId)
            }).GetPaged<PostDto>(pageNo, pageSize);

            this.logger.LogDebug("GetAllNewPostByUser End");
            return allPosts;
        }

        public PostDto LikeDislikePost(int currentUserId, int postId)
        {
            var postDetails = this.context.Post.Include(x => x.PostLike)
                                               .Include(x => x.UploadByUser)
                                               .Where(x => x.Id == postId)
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
                IsCurrentUserLikedPost = postDetails.PostLike.Any(z => z.LikeByUserId == currentUserId)
            };
        }
    }
}