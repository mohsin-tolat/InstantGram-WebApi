using System;
using System.Collections.Generic;
using System.Linq;
using InstantGram.Core.Insterface;
using InstantGram.Data.DbContexts;
using InstantGram.Data.DBmodels;
using InstantGram.Data.DTOModels;
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
    }
}