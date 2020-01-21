using System.Collections.Generic;
using InstantGram.Data.DBmodels;
using InstantGram.Data.DTOModels;

namespace InstantGram.Core.Insterface
{
    public interface IPostService
    {
        PagedResult<PostDto> GetAllNewPostByUser(int userId, int pageNo, int pageSize);
        bool LikePost(int currentUserId, int postId);
    }
}