using System;
using System.Collections.Generic;
using System.Linq;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DTOModels;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using InstantGram.Data.DBContexts;

namespace InstantGram.Core.Service
{
    public class LoginService : ILoginService
    {
        private ILogger<LoginService> logger;
        private ApplicationDbContext context;

        public LoginService(ILogger<LoginService> logger, ApplicationDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public LoggedInUserModel Authenticate(string userName, string password, string jwtSecretKey)
        {
            var userDetails = this.context.User.FirstOrDefault(x => x.Username == userName);

            if (userDetails == null)
            {
                return null;
            }

            var passwordHash = CommonUtilities.GenerateHashPassword(password, userDetails.PasswordSalt);

            if (userDetails.PasswordHash != passwordHash)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = Encoding.ASCII.GetBytes(jwtSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, userDetails.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricSecurityKey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new LoggedInUserModel()
            {
                // UserId = userDetails.Id,
                Username = userDetails.Username,
                Token = tokenHandler.WriteToken(token),
                UserAvatarLink = userDetails.UserAvatar
            };
        }
    }
}