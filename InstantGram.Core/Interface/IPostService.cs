using System.Collections.Generic;
using InstantGram.Data.DBmodels;

namespace InstantGram.Core.Insterface
{
    public interface IPostService
    {
        List<PostDto> GetAllNewPostByUser(int userId);
    }
}