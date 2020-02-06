using InstantGram.Api.Controllers;
using InstantGram.Core.Insterface;
using InstantGram.Data.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace InstantGram.Api.Test.Controllers
{
    public class LoginControllerTest
    {
        private Mock<ILogger<LoginController>> mockLogger;
        private Mock<ILoginService> mockLoginService;
        private LoginController unitUnderTest;

        public LoginControllerTest()
        {
            IOptions<AppSettings> mockOptions = Options.Create<AppSettings>(new AppSettings()
            {
                Secret = "TEST",
                ApplicationDomainConfiguration = new AppSettings.ApplicationDomainConfigurations()
                {
                    InstantGramApiUrl = "http://test.Test.com",
                    InstantGramUIUrl = "http://test.Test.com",
                },
            });
            this.mockLogger = new Mock<ILogger<LoginController>>();
            this.mockLoginService = new Mock<ILoginService>();
            this.unitUnderTest = new LoginController(this.mockLogger.Object, mockOptions, mockLoginService.Object);
        }

        [Fact]
        public void Authenticate_Should_Return_AuthenticateModel_When_Authentication_Successful()
        {
            //Arrange
            this.mockLoginService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new LoggedInUserModel()
            {
                Username = "TEST",
                Token = "TEST",
                UserAvatarLink = "http://TEST.test.com/teasdsdst.png"
            });

            //Act
            var result = this.unitUnderTest.Authenticate(new Models.AuthenticateModel() { Username = "TEST", Password = "TET" });


            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var loggedInUserDetails = okObjectResult.Value as LoggedInUserModel;
            Assert.NotNull(loggedInUserDetails);
            Assert.Equal("TEST", loggedInUserDetails.Username);
            Assert.Equal("TEST", loggedInUserDetails.Token);
        }
    }
}