using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InstantGram.Common.Domain.Interface;
using InstantGram.Core.Insterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InstantGram.Common.Helper;

namespace InstantGram.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private IPostService postService;
        private readonly ILogger<PostController> logger;
        private readonly IUserResolverService userResolverService;

        public PostController(IPostService postService, ILogger<PostController> logger, IUserResolverService userResolverService)
        {
            this.postService = postService;
            this.logger = logger;
            this.userResolverService = userResolverService;
        }

        [HttpGet]
        public IActionResult GetAllNewPosts([FromQuery] int pageNo = 1, [FromQuery]int pageSize = 10)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
            if (currentUserDetails.UserId > 0)
            {
                var response = this.postService.GetAllNewPostByUser(currentUserDetails.UserId, pageNo, pageSize);
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost("{postHashId}")]
        public IActionResult LikeDislikePost(string postHashId)
        {
            try
            {
                var postId = string.IsNullOrWhiteSpace(postHashId) ? 0 : Convert.ToInt32(postHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || postId == 0)
                {
                    return BadRequest();
                }

                var response = this.postService.LikeDislikePost(currentUserDetails.UserId, postId);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error occurred in Like Dislike post.");
                return BadRequest(new { message = "Something went wrong, Please Try again Later." });
            }
        }

        [HttpGet]
        public IActionResult GetAllOpenPosts([FromQuery]string userHashId = "", [FromQuery] int pageNo = 1, [FromQuery]int pageSize = 10)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
            var userId = string.IsNullOrWhiteSpace(userHashId) ? 0 : Convert.ToInt32(userHashId.ToDecrypt());
            if (currentUserDetails.UserId > 0)
            {
                var response = this.postService.GetAllOpenPosts(currentUserDetails.UserId, pageNo, pageSize, userId);
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetPostByHashId([FromQuery]string postHashId)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
            var postId = string.IsNullOrWhiteSpace(postHashId) ? 0 : Convert.ToInt32(postHashId.ToDecrypt());

            if (currentUserDetails.UserId <= 0 || postId <= 0)
            {
                return BadRequest();
            }

            var response = this.postService.GetPostById(currentUserDetails.UserId, postId);
            return Ok(response);
        }
    }
}
