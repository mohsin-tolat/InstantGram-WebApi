using System;
using System.Linq;
using InstantGram.Common.Domain.Interface;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DTOModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public AppSettings appSettings { get; }

        public PostController(IPostService postService, ILogger<PostController> logger, IUserResolverService userResolverService, IOptions<AppSettings> appSettings)
        {
            this.postService = postService;
            this.logger = logger;
            this.userResolverService = userResolverService;
            this.appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult GetAllNewPosts([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
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
        public IActionResult GetAllOpenPosts([FromQuery] string userHashId = "", [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 10)
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
        public IActionResult GetPostByHashId([FromQuery] string postHashId)
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

        [HttpPost, DisableRequestSizeLimit]
        public IActionResult UploadPost([FromBody] UploadPostDto uploadPostDetails)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();

            if (currentUserDetails.UserId <= 0 || string.IsNullOrWhiteSpace(uploadPostDetails.ImageContent))
            {
                return BadRequest();
            }

            var isImageUploaded = this.postService.SavePostContentToFolderAndDatabase(currentUserDetails.UserId, uploadPostDetails.ImageContent);
            return Ok(isImageUploaded);
        }

        [HttpDelete]
        public IActionResult DeletePostByHashId([FromQuery] string postHashId)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
            var postId = string.IsNullOrWhiteSpace(postHashId) ? 0 : Convert.ToInt32(postHashId.ToDecrypt());

            if (currentUserDetails.UserId <= 0 || postId <= 0)
            {
                return BadRequest();
            }

            var response = this.postService.DeletePostById(currentUserDetails.UserId, postId);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetCurrentUserPostsActivities([FromQuery] int pageNo = 1, [FromQuery] int pageSize = 20)
        {
            var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();

            if (currentUserDetails.UserId == 0)
            {
                return BadRequest();
            }

            var response = this.postService.GetCurrentUserPostsActivities(currentUserDetails.UserId, pageNo, pageSize);
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetPostCommentsByPostHashId([FromQuery] string postHashId, [FromQuery] int pageNo = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var postId = string.IsNullOrWhiteSpace(postHashId) ? 0 : Convert.ToInt32(postHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || postId == 0)
                {
                    return BadRequest();
                }

                var response = this.postService.GetPostCommentsByPostId(currentUserDetails.UserId, postId, pageNo, pageSize);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error occurred in Get PostComments By Post Hash Id.");
                return BadRequest(new { message = "Something went wrong, Please Try again Later." });
            }
        }

        [HttpPost]
        public IActionResult AddNewCommentForPost([FromBody] UpdateCommentModel comment)
        {
            try
            {
                var postId = string.IsNullOrWhiteSpace(comment.PostHashId) ? 0 : Convert.ToInt32(comment.PostHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || postId == 0 || string.IsNullOrWhiteSpace(comment.Content))
                {
                    return BadRequest();
                }

                var response = this.postService.AddNewCommentForPost(currentUserDetails.UserId, comment);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error occurred in Get PostComments By Post Hash Id.");
                return BadRequest(new { message = "Something went wrong, Please Try again Later." });
            }
        }

        [HttpDelete]
        public IActionResult DeletePostCommentByPostHashId([FromQuery] string commentHashId)
        {
            try
            {
                var commentId = string.IsNullOrWhiteSpace(commentHashId) ? 0 : Convert.ToInt32(commentHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || commentId == 0)
                {
                    return BadRequest();
                }

                var response = this.postService.DeletePostCommentByPostId(commentId, currentUserDetails.UserId);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error occurred in Get PostComments By Post Hash Id.");
                return BadRequest(new { message = "Something went wrong, Please Try again Later." });
            }
        }

        [HttpPost("{commentHashId}/{commentIdentifier}")]
        public IActionResult LikeDislikeComment(string commentHashId, string commentIdentifier)
        {
            try
            {
                var commentId = string.IsNullOrWhiteSpace(commentHashId) ? 0 : Convert.ToInt32(commentHashId.ToDecrypt());
                var currentUserDetails = this.userResolverService.GetLoggedInUserDetails();
                if (currentUserDetails.UserId == 0 || commentId == 0 || string.IsNullOrWhiteSpace(commentIdentifier))
                {
                    return BadRequest();
                }

                var response = this.postService.LikeDislikeComment(currentUserDetails.UserId, commentId, commentIdentifier);

                return Ok(response);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error occurred in Like Dislike post.");
                return BadRequest(new { message = "Something went wrong, Please Try again Later." });
            }
        }

    }
}