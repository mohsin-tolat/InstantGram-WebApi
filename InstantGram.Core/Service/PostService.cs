using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InstantGram.Common.Exceptions;
using InstantGram.Common.Helper;
using InstantGram.Core.Insterface;
using InstantGram.Data.DBContexts;
using InstantGram.Data.DBModels;
using InstantGram.Data.DTOModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InstantGram.Core.Service
{
    public class PostService : IPostService
    {
        private readonly AppSettings appSettings;
        private ILogger<PostService> logger;
        private ApplicationDbContext context;

        public PostService(IOptions<AppSettings> appSettings, ILogger<PostService> logger, ApplicationDbContext context)
        {
            this.appSettings = appSettings.Value;
            this.logger = logger;
            this.context = context;
        }


        public PagedResult<PostDto> GetAllNewPostByUser(int currentLoggedInUserId, int pageNo, int pageSize)
        {
            this.logger.LogDebug("GetAllNewPostByUser Started");

            var allPosts = (from userFollower in this.context.UserFollower.Where(x => x.UserId == currentLoggedInUserId)
                            join userPosts in this.context.Post on userFollower.FollowingUserId equals userPosts.UploadByUserId
                            orderby userPosts.UploadOn descending
                            select new PostDto()
                            {
                                Id = userPosts.Id,
                                ContentLink = userPosts.ContentLink,
                                TotalComments = userPosts.PostComment.Count(),
                                TotalLikes = userPosts.PostLike.Count(),
                                UploadBy = userPosts.UploadByUserId,
                                UploadOn = userPosts.UploadOn,
                                UploadedByUserName = userPosts.UploadByUser.Username,
                                UploadedUserAvatar = userPosts.UploadByUser.UserAvatar,
                                IsCurrentUserLikedPost = userPosts.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId)
                            }).GetPaged<PostDto>(pageNo, pageSize);

            this.logger.LogDebug("GetAllNewPostByUser End");
            return allPosts;
        }

        public PagedResult<PostDto> GetAllOpenPosts(int currentLoggedInUserId, int pageNo, int pageSize, int userId = 0)
        {
            var allPosts = this.context.Post.Where(x => userId == 0 || x.UploadByUserId == userId).OrderByDescending(post => post.UploadOn).Select(x => new PostDto()
            {
                Id = x.Id,
                ContentLink = x.ContentLink,
                TotalComments = x.PostComment.Count(),
                TotalLikes = x.PostLike.Count(),
                UploadBy = x.UploadByUserId,
                UploadOn = x.UploadOn,
                UploadedByUserName = x.UploadByUser.Username,
                UploadedUserAvatar = x.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = x.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId)
            }).GetPaged<PostDto>(pageNo, pageSize);

            return allPosts;
        }

        public PostDto GetPostById(int currentLoggedInUserId, int postId)
        {
            var postDetails = this.context.Post.Where(x => x.Id == postId).OrderByDescending(post => post.UploadOn).Select(x => new PostDto()
            {
                Id = x.Id,
                ContentLink = x.ContentLink,
                TotalComments = x.PostComment.Count(),
                TotalLikes = x.PostLike.Count(),
                UploadBy = x.UploadByUserId,
                UploadOn = x.UploadOn,
                UploadedByUserName = x.UploadByUser.Username,
                UploadedUserAvatar = x.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = x.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId),
                IsCurrentUserUploadedPost = x.UploadByUserId == currentLoggedInUserId
            }).FirstOrDefault();

            return postDetails;
        }



        public PostDto LikeDislikePost(int currentUserId, int postId)
        {
            var postDetails = this.context.Post.Include(x => x.PostLike)
                                               .Include(x => x.UploadByUser)
                                               .Where(x => x.Id == postId)
                                               .OrderByDescending(post => post.UploadOn)
                                               .FirstOrDefault();

            if (postDetails == null)
            {
                return null;
            }

            if (!postDetails.PostLike.Any(x => x.LikeByUserId == currentUserId))
            {
                this.context.PostLike.Add(new PostLike()
                {
                    LikeByUserId = currentUserId,
                    PostId = postId,
                    LikeOn = CommonUtilities.GetCurrentDateTime()
                });
            }
            else
            {
                if (postDetails.PostLike.Any(x => x.LikeByUserId == currentUserId))
                {
                    this.context.PostLike.RemoveRange(postDetails.PostLike.Where(x => x.LikeByUserId == currentUserId).ToList());
                }
            }

            this.context.SaveChanges();

            return new PostDto()
            {
                Id = postDetails.Id,
                ContentLink = postDetails.ContentLink,
                TotalComments = postDetails.PostComment.Count(),
                TotalLikes = postDetails.PostLike.Count(),
                UploadBy = postDetails.UploadByUserId,
                UploadOn = postDetails.UploadOn,
                UploadedByUserName = postDetails.UploadByUser.Username,
                UploadedUserAvatar = postDetails.UploadByUser.UserAvatar,
                IsCurrentUserLikedPost = postDetails.PostLike.Any(z => z.LikeByUserId == currentUserId)
            };
        }

        public bool SavePostContentToFolderAndDatabase(int currentLoggedInUserId, string postContent)
        {
            var postContentLocal = postContent.Replace("data:image/jpeg;base64,", string.Empty);
            var folderName = Path.Combine("Resources", "UserPosts", currentLoggedInUserId.ToString());
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            string imageName = CommonUtilities.GenerateRandomString() + ".jpg";
            var isImageSaved = this.SaveImageToFolder(postContentLocal, imageName, pathToSave);
            if (!isImageSaved)
            {
                return false;
            }

            var dbPath = Path.Combine(this.appSettings.ApplicationDomainConfiguration.InstantGramApiUrl, folderName, imageName);
            return this.SaveImageDetailsIntoDatabase(currentLoggedInUserId, dbPath);
        }

        public bool DeletePostById(int currentLoggedInUserId, int postId)
        {
            var postDetails = this.context.Post.Include(x => x.PostLike).Include(x => x.PostComment).Include("PostComment.CommentLike").FirstOrDefault(x => x.Id == postId && x.UploadByUserId == currentLoggedInUserId);

            if (postDetails == null)
            {
                throw new DetailsNotFoundException("PostId", postId.ToEncrypt());
            }

            this.DeletePostFromStorage(postDetails, this.appSettings.ApplicationDomainConfiguration.InstantGramApiUrl);
            this.context.CommentLike.RemoveRange(postDetails.PostComment.SelectMany(x => x.CommentLike));
            this.context.PostComment.RemoveRange(postDetails.PostComment);
            this.context.PostLike.RemoveRange(postDetails.PostLike);
            this.context.Post.Remove(postDetails);
            return this.context.SaveChanges() > 0;
        }

        public PagedResult<ActivityDto> GetCurrentUserPostsActivities(int currentLoggedInUserId, int pageNo, int pageSize)
        {
            var activities = this.context.PostLike
                                                       .Include(x => x.Post)
                                                       .Where(x => x.Post.UploadByUserId == currentLoggedInUserId && x.LikeByUserId != currentLoggedInUserId)
                                                       .Select(activity => new ActivityDto()
                                                       {
                                                           ActivityDoneByUser = new UserDto()
                                                           {
                                                               Id = activity.LikeByUser.Id,
                                                               FirstName = activity.LikeByUser.FirstName,
                                                               LastName = activity.LikeByUser.LastName,
                                                               Username = activity.LikeByUser.Username,
                                                               EmailAddress = activity.LikeByUser.EmailAddress,
                                                               DateOfJoining = activity.LikeByUser.DateOfJoining,
                                                               UserAvatar = activity.LikeByUser.UserAvatar,
                                                               TotalPostCount = activity.LikeByUser.Post.Count(),
                                                               IsCurrentUser = activity.LikeByUser.Id == currentLoggedInUserId,
                                                               TotalFollowers = activity.LikeByUser.UserFollowerFollowingUser.Count(x => x.UserId == currentLoggedInUserId),
                                                               TotalFollowings = activity.LikeByUser.UserFollowerUser.Count(x => x.UserId == currentLoggedInUserId),
                                                               IsAlreadyFollowed = activity.LikeByUser.Id == currentLoggedInUserId || activity.LikeByUser.UserFollowerFollowingUser.Any(x => x.UserId == currentLoggedInUserId),
                                                           },
                                                           ActivityDoneOn = activity.LikeOn,
                                                           IsComment = false,
                                                           IsLike = true,
                                                           Post = new PostDto()
                                                           {
                                                               Id = activity.Post.Id,
                                                               ContentLink = activity.Post.ContentLink,
                                                               TotalComments = activity.Post.PostComment.Count(),
                                                               TotalLikes = activity.Post.PostLike.Count(),
                                                               UploadBy = activity.Post.UploadByUserId,
                                                               UploadOn = activity.Post.UploadOn,
                                                               UploadedByUserName = activity.Post.UploadByUser.Username,
                                                               UploadedUserAvatar = activity.Post.UploadByUser.UserAvatar,
                                                               IsCurrentUserLikedPost = activity.Post.PostLike.Any(z => z.LikeByUserId == currentLoggedInUserId)
                                                           }
                                                       })
                                                       .OrderByDescending(x => x.ActivityDoneOn)
                                                       .GetPaged(pageNo, pageSize);

            return activities;
        }

        public PagedResult<CommentDto> GetPostCommentsByPostId(int currentLoggedInUserId, int postId, int pageNo, int pageSize)
        {
            var postComments = this.context.PostComment
                                                       .Include(x => x.Post)
                                                       .Include(x => x.CommentLike)
                                                       .Where(x => x.PostId == postId)
                                                       .Select(comment => new CommentDto()
                                                       {
                                                           Id = comment.Id,
                                                           CommentedBy = new UserDto()
                                                           {
                                                               Id = comment.CommentedByUser.Id,
                                                               FirstName = comment.CommentedByUser.FirstName,
                                                               LastName = comment.CommentedByUser.LastName,
                                                               Username = comment.CommentedByUser.Username,
                                                               EmailAddress = comment.CommentedByUser.EmailAddress,
                                                               DateOfJoining = comment.CommentedByUser.DateOfJoining,
                                                               UserAvatar = comment.CommentedByUser.UserAvatar,
                                                           },
                                                           Content = comment.CommentContent,
                                                           TotalCommentLikes = comment.CommentLike.Count(),
                                                           IsCurrentUserComment = comment.CommentedByUserId == currentLoggedInUserId,
                                                           IsCurrentUserLikeComment = comment.CommentLike.Any(x => x.LikeByUserId == currentLoggedInUserId),
                                                           CommentedOn = comment.CommentedOn,
                                                           PostDetails = new PostDto()
                                                           {
                                                               Id = comment.Post.Id,
                                                               ContentLink = comment.Post.ContentLink,
                                                               TotalComments = comment.Post.PostComment.Count(),
                                                               TotalLikes = comment.Post.PostLike.Count(),
                                                               UploadBy = comment.Post.UploadByUserId,
                                                               UploadOn = comment.Post.UploadOn,
                                                               UploadedByUserName = comment.Post.UploadByUser.Username,
                                                               UploadedUserAvatar = comment.Post.UploadByUser.UserAvatar,
                                                           }
                                                       })
                                                       .OrderByDescending(x => x.CommentedOn)
                                                       .GetPaged(pageNo, pageSize);

            return postComments;
        }

        public bool AddNewCommentForPost(int currentUserId, UpdateCommentModel comment)
        {
            var isPostAvailable = this.context.Post.Any(x => x.Id == comment.PostId);
            if (!isPostAvailable)
            {
                return isPostAvailable;
            }

            this.context.PostComment.Add(new PostComment()
            {
                CommentContent = comment.Content,
                CommentedByUserId = currentUserId,
                CommentedOn = CommonUtilities.GetCurrentDateTime(),
                PostId = comment.PostId,
            });

            return this.context.SaveChanges() > 0;
        }

        public bool DeletePostCommentByPostId(int commentId, int currentUserId)
        {
            var postDetails = this.context.PostComment.Include(x => x.CommentLike).Where(x => x.Id == commentId && x.CommentedByUserId == currentUserId).FirstOrDefault();
            if (postDetails == null)
            {
                return true;
            }

            this.context.CommentLike.RemoveRange(postDetails.CommentLike);
            this.context.PostComment.Remove(postDetails);

            return this.context.SaveChanges() > 0;
        }

        public CommentDto LikeDislikeComment(int currentUserId, int postId, string commentIdentifier)
        {
            var commentDetails = this.context.PostComment.Include(x => x.CommentLike)
                                               .Include(x => x.CommentedByUser)
                                               .Include(x => x.Post)
                                               .Include(x => x.Post.UploadByUser)
                                               .Where(x => x.Id == postId)
                                               .OrderByDescending(comment => comment.CommentedOn)
                                               .FirstOrDefault();

            if (commentDetails == null)
            {
                return null;
            }

            if (!commentDetails.CommentLike.Any(x => x.LikeByUserId == currentUserId))
            {
                this.context.CommentLike.Add(new CommentLike()
                {
                    LikeByUserId = currentUserId,
                    CommentId = postId,
                    LikeOn = CommonUtilities.GetCurrentDateTime()
                });
            }
            else
            {
                if (commentDetails.CommentLike.Any(x => x.LikeByUserId == currentUserId))
                {
                    this.context.CommentLike.RemoveRange(commentDetails.CommentLike.Where(x => x.LikeByUserId == currentUserId).ToList());
                }
            }

            this.context.SaveChanges();

            return new CommentDto()
            {
                CommentIdentifier = commentIdentifier,
                Id = commentDetails.Id,
                CommentedBy = new UserDto()
                {
                    Id = commentDetails.CommentedByUser.Id,
                    FirstName = commentDetails.CommentedByUser.FirstName,
                    LastName = commentDetails.CommentedByUser.LastName,
                    Username = commentDetails.CommentedByUser.Username,
                    EmailAddress = commentDetails.CommentedByUser.EmailAddress,
                    DateOfJoining = commentDetails.CommentedByUser.DateOfJoining,
                    UserAvatar = commentDetails.CommentedByUser.UserAvatar,
                },
                Content = commentDetails.CommentContent,
                TotalCommentLikes = commentDetails.CommentLike.Count(),
                IsCurrentUserComment = commentDetails.CommentedByUserId == currentUserId,
                IsCurrentUserLikeComment = commentDetails.CommentLike.Any(x => x.LikeByUserId == currentUserId),
                CommentedOn = commentDetails.CommentedOn,
                PostDetails = new PostDto()
                {
                    Id = commentDetails.Post.Id,
                    ContentLink = commentDetails.Post.ContentLink,
                    TotalComments = commentDetails.Post.PostComment.Count(),
                    TotalLikes = commentDetails.Post.PostLike.Count(),
                    UploadBy = commentDetails.Post.UploadByUserId,
                    UploadOn = commentDetails.Post.UploadOn,
                    UploadedByUserName = commentDetails.Post.UploadByUser.Username,
                    UploadedUserAvatar = commentDetails.Post.UploadByUser.UserAvatar,
                }
            };
        }

        private bool DeletePostFromStorage(Post postDetails, string currentUrl)
        {
            try
            {
                string pathToImage = postDetails.ContentLink.Replace(currentUrl, string.Empty);
                //set the image path
                string imgPath = Path.Combine(Directory.GetCurrentDirectory(), pathToImage);

                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                    return true;
                }

                return true;
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex, "Error Occurred while deleting files from storage in DeletePostFromStorage.");
                return false;
            }
        }

        private bool SaveImageDetailsIntoDatabase(int currentLoggedInUserId, string dbPath)
        {
            this.context.Post.Add(new Post()
            {
                UploadByUserId = currentLoggedInUserId,
                ContentLink = $"{dbPath}",
                UploadOn = CommonUtilities.GetCurrentDateTime(),
            });

            return this.context.SaveChanges() > 0;
        }

        private bool SaveImageToFolder(string postContent, string imgName, string pathToFolder)
        {
            //Check if directory exist
            if (!System.IO.Directory.Exists(pathToFolder))
            {
                System.IO.Directory.CreateDirectory(pathToFolder); //Create directory if it doesn't exist
            }

            //set the image path
            string imgPath = Path.Combine(pathToFolder, imgName);

            byte[] imageBytes = Convert.FromBase64String(postContent);

            File.WriteAllBytes(imgPath, imageBytes);

            return true;
        }
    }
}