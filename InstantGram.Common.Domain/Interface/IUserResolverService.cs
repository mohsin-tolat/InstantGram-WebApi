using InstantGram.Data.DTOModels;

namespace InstantGram.Common.Domain.Interface
{
    public interface IUserResolverService
    {
        string GetCurrentLoggedInUserName();
        
        LoggedInUserModel GetLoggedInUserDetails();
    }
}