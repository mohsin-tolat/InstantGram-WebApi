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

        [HttpPost("{postId}")]
        public IActionResult LikeDislikePost(int postId)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
            if (currentUserDetails.UserId > 0)
            {
                var response = this.postService.LikeDislikePost(currentUserDetails.UserId, postId);
                return Ok(response);
            }

            return BadRequest();
        }
    }
}
