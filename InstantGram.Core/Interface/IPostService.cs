using System.Collections.Generic;

namespace InstantGram.Core.Insterface
{
    public interface IPostService
    {
        List<Post> GetAllNewPost(int userId);
    }
}