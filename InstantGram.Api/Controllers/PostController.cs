using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet()]
        public IActionResult GetAllNewPosts([FromQuery] int pageNo = 1, [FromQuery]int pageSize = 10)
        {
            var userIdClaimValue = this.User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).Select(x => x.Value).FirstOrDefault();
            if (int.TryParse(userIdClaimValue, out int currentUserId) && currentUserId > 0)
            {
                var response = this.postService.GetAllNewPostByUser(currentUserId, pageNo, pageSize);
                return Ok(response);
            }

            return BadRequest();
        }

        [HttpPost("{postId}")]
        public IActionResult LikePost(int postId)
        {
            var userIdClaimValue = this.User.Claims.Where(x => x.Type == ClaimTypes.SerialNumber).Select(x => x.Value).FirstOrDefault();
            if (int.TryParse(userIdClaimValue, out int currentUserId) && currentUserId > 0)
            {
                var response = this.postService.LikePost(currentUserId, postId);
                return Ok(response);
            }

            return BadRequest();
        }
    }
}
