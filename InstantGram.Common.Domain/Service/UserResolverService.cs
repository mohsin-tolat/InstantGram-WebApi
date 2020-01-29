using System.Linq;
using System.Security.Claims;
using InstantGram.Common.Domain.Interface;
using InstantGram.Data.DBContexts;
using InstantGram.Data.DTOModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InstantGram.Common.Domain.Service
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly ILogger<UserResolverService> logger;
        private readonly ApplicationDbContext context;

        public UserResolverService(IHttpContextAccessor httpContext, ILogger<UserResolverService> logger, ApplicationDbContext context)
        {
            this.httpContext = httpContext;
            this.logger = logger;
            this.context = context;
        }

        public string GetCurrentLoggedInUserName()
        {
            LoggedInUserModel loggedInUser = new LoggedInUserModel();
            return this.httpContext.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
        }

        public LoggedInUserModel GetLoggedInUserDetails()
        {
            LoggedInUserModel loggedInUser = new LoggedInUserModel();
            var userNameClaimValue = this.httpContext.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(userNameClaimValue))
            {
                loggedInUser = this.context.User.Where(x => x.Username == userNameClaimValue).Select(x => new LoggedInUserModel()
                {
                    UserId = x.Id,
                    Username = x.Username,
                    EmailAddress = x.EmailAddress
                }).FirstOrDefault();
            }

            return loggedInUser;
        }
    }
}