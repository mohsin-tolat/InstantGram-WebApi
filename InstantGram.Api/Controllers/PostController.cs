using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantGram.Core.Insterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InstantGram.Api.Controllers
{
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
        public IActionResult GetAllNewPosts(int userId)
        {
            var response = this.postService.GetAllNewPostByUser(userId);
            return Ok(response);
        }
    }
}
