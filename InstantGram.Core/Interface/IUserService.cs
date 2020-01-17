using System.Collections.Generic;
using InstantGram.Data.DBmodels;
using InstantGram.Data.DTOModels;

namespace InstantGram.Core.Insterface
{
    public interface IUserService
    {
        bool AddNewUser(UserRegistration registrationDetails);
    }
}