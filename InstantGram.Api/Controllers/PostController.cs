using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InstantGram.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {

        private readonly ILogger<PostController> _logger;

        public PostController(ILogger<PostController> logger)
        {
            this._logger = logger;
        }

        public ActionResult<List<string>> Get()
        {
            return Ok(new List<string>() { "Value 1", "Value 2" });
        }

        public ActionResult<List<string>> GetAllNewPost()
        {
            this
        }
    }
}
