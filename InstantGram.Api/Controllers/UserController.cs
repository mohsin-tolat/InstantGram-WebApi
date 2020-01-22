using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantGram.Core.Insterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InstantGram.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private IUserService userService;
        private ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult AddNewUser([FromBody] UserRegistration userDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = this.userService.AddNewUser(userDetails);
                    return Ok(response);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred in AddNewUser");
                return BadRequest();
            }
        }
    }
}
