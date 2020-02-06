using System;
using InstantGram.Common.Helper;
using InstantGram.Core.Service;
using InstantGram.Data.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace InstantGram.Core.Test.Service
{
    public class LoginServiceTest
    {
        public const string AUTHENTICATE_SECRET = "9b68a2abbdb51b902157ce7672761ab6d758c831a703ff39d3f0542090b6a2fe";
        private Mock<ILogger<LoginService>> mockLogger;
        private LoginService unitUnderTest;
        private ApplicationDbContext databaseContext;

        public LoginServiceTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            this.databaseContext = new ApplicationDbContext(options);
            this.databaseContext.Database.EnsureCreated();

            this.mockLogger = new Mock<ILogger<LoginService>>();

            this.unitUnderTest = new LoginService(this.mockLogger.Object, databaseContext);
        }

        [Fact]
        public void Authenticate_Should_Return_Null_When_UserNotFound_FromDatabase()
        {
            // Arrange
            this.databaseContext.User.Add(new Data.DBModels.User()
            {
                Username = "TEST1"
            });

            this.databaseContext.SaveChanges();

            // Act
            var response = this.unitUnderTest.Authenticate("TEST", "TEST", AUTHENTICATE_SECRET);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void Authenticate_Should_Return_Null_When_PasswordNotMatch()
        {
            var passwordHash = CommonUtilities.GenerateHashPassword("TEST", CommonUtilities.GenerateRandomString());

            // Arrange
            this.databaseContext.User.Add(new Data.DBModels.User()
            {
                Username = "TEST",
                PasswordHash = passwordHash,
                PasswordSalt = CommonUtilities.GenerateRandomString()
            });

            this.databaseContext.SaveChanges();

            // Act
            var response = this.unitUnderTest.Authenticate("TEST", "TEST", AUTHENTICATE_SECRET);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void Authenticate_Should_Return_AuthenticatedModel_When_UserSuccessfullyAuthenticated()
        {
            var passwordSalt = CommonUtilities.GenerateRandomString();
            var passwordHash = CommonUtilities.GenerateHashPassword("TEST", passwordSalt);

            // Arrange
            this.databaseContext.User.Add(new Data.DBModels.User()
            {
                Username = "TEST",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                UserAvatar = "http://www.test.com/123456.png"
            });

            this.databaseContext.SaveChanges();

            // Act
            var response = this.unitUnderTest.Authenticate("TEST", "TEST", AUTHENTICATE_SECRET);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response);
            Assert.Equal("TEST", response.Username);
            Assert.Equal("http://www.test.com/123456.png", response.UserAvatarLink);

        }
    }
}