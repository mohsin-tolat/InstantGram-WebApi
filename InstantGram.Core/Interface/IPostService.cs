using System.Collections.Generic;
using InstantGram.Data.DBmodels;

namespace InstantGram.Core.Insterface
{
    public interface IPostService
    {
        PagedResult<PostDto> GetAllNewPostByUser(int userId, int pageNo, int pageSize);
    }
}