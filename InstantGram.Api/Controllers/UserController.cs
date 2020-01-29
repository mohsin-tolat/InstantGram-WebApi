using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantGram.Common.Domain.Interface;
using InstantGram.Core.Insterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InstantGram.Common.Helper;

namespace InstantGram.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserResolverService userResolverService;
        private readonly ILogger<UserController> logger;

        public UserController(ILogger<UserController> logger, IUserService userService, IUserResolverService userResolverService)
        {
            this.userService = userService;
            this.userResolverService = userResolverService;
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

        [HttpGet]
        public IActionResult GetUserDetailsBasedOnSearch([FromQuery]string searchText, [FromQuery] int pageNo = 1, [FromQuery]int pageSize = 10)
        {
            try
            {
                var currentLoggedInUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentLoggedInUserDetails.UserId > 0)
                {
                    var response = this.userService.GetUserDetailsBasedOnSearch(currentLoggedInUserDetails.UserId, searchText, pageNo, pageSize);
                    return Ok(response);
                }


                return BadRequest(new { Message = "Details Not Found" });
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred in AddNewUser");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult FollowUser(string followingUserHashId)
        {
            try
            {
                var followingUserId = string.IsNullOrWhiteSpace(followingUserHashId) ? 0 : Convert.ToInt32(followingUserHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || followingUserId == 0)
                {
                    return BadRequest(new { Message = "Details Not Found" });
                }

                var response = this.userService.FollowUnFollowUser(currentUserDetails.UserId, followingUserId, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred in FollowUser");
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult UnFollowUser(string followingUserHashId)
        {
            try
            {
                var followingUserId = string.IsNullOrWhiteSpace(followingUserHashId) ? 0 : Convert.ToInt32(followingUserHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || followingUserId == 0)
                {
                    return BadRequest(new { Message = "Details Not Found" });
                }


                var response = this.userService.FollowUnFollowUser(currentUserDetails.UserId, followingUserId, false);
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred in UnFollowUser");
                return BadRequest();
            }
        }


        [HttpGet]
        public IActionResult GetUserDetails(string userHashId)
        {
            try
            {
                var userId = string.IsNullOrWhiteSpace(userHashId) ? 0 : Convert.ToInt32(userHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || userId == 0)
                {
                    return BadRequest(new { Message = "Details Not Found" });
                }


                var response = this.userService.GetUserDetails(currentUserDetails.UserId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred in UnFollowUser");
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult GetCurrentUserProfile()
        {
            try
            {
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0)
                {
                    return BadRequest(new { Message = "Details Not Found" });
                }


                var response = this.userService.GetUserDetails(currentUserDetails.UserId, currentUserDetails.UserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred in UnFollowUser");
                return BadRequest();
            }
        }
    }
}
