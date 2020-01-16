using System.Collections.Generic;
using InstantGram.Core.Insterface;
using InstantGram.Data.DBmodels;
using Microsoft.Extensions.Logging;

namespace InstantGram.Core.Service
{
    public class PostService : IPostService
    {
        private ILogger<PostService> logger;

        public PostService(ILogger<PostService> logger)
        {
            this.logger = logger;
        }

        public List<Post> GetAllNewPostByUser(int userId)
        {
            this.logger.LogDebug("GetAllNewPostByUser Started");
            this.logger.LogDebug("GetAllNewPostByUser End");
            return null;
        }
    }
}