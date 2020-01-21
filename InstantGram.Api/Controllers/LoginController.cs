using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantGram.Api.Models;
using InstantGram.Core.Insterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InstantGram.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LoginController : ControllerBase
    {
        private ILogger<LoginController> logger;
        private ILoginService loginService;
        private AppSettings config;

        public LoginController(ILogger<LoginController> logger, IOptions<AppSettings> appSettings, ILoginService loginService)
        {
            this.logger = logger;
            this.loginService = loginService;
            this.config = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            try
            {
                var response = this.loginService.Authenticate(model.Username, model.Password, config.Secret);
                if (response != null)
                {
                    return Ok(response);
                }

                return BadRequest(new { message = "Username or password is incorrect" });
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error occurred in Authentication");
                throw;
            }
        }

        [HttpGet]
        public IActionResult CheckAuthorization()
        {
            return Ok(true);
        }
    }
}
