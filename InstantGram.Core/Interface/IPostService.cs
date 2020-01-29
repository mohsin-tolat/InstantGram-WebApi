using System.Collections.Generic;
using InstantGram.Common.Helper;
using InstantGram.Data.DTOModels;

namespace InstantGram.Core.Insterface
{
    public interface IPostService
    {
        PagedResult<PostDto> GetAllNewPostByUser(int userId, int pageNo, int pageSize);

        PostDto LikeDislikePost(int currentUserId, int postId);

        PagedResult<PostDto> GetAllOpenPosts(int currentLoggedInUserId, int pageNo, int pageSize, int userId = 0);

        PostDto GetPostById(int currentLoggedInUserId, int postId);

        bool SavePostContentToFolderAndDatabase(int currentLoggedInUserId, string instantGramApiUrl, string postContent);
    }
}