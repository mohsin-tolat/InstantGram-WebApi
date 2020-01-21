using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet("{userId}")]
        public IActionResult GetAllNewPosts(int userId, [FromQuery] int pageNo = 1, [FromQuery]int pageSize = 10)
        {
            var response = this.postService.GetAllNewPostByUser(userId, pageNo, pageSize);
            return Ok(response);
        }
    }
}
