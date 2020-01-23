using InstantGram.Data.DTOModels;

namespace InstantGram.Core.Insterface
{
    public interface ILoginService
    {
        LoggedInUserModel Authenticate(string userName, string password, string jwtSecretKey);
    }
}