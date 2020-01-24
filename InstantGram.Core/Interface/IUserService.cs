using System.Collections.Generic;
using InstantGram.Common.Helper;
using InstantGram.Data.DTOModels;

namespace InstantGram.Core.Insterface
{
    public interface IUserService
    {
        bool AddNewUser(UserRegistration registrationDetails);
        PagedResult<UserDto> GetUserDetailsBasedOnSearch(int currentUserId, string searchText, int pageNo, int pageSize);
    }
}