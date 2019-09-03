using AspNetForum.Data.Interfaces;
using AspNetForum.Data.Models;
using AspNetForum.ViewModels.Forum;
using AspNetForum.ViewModels.Post;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AspNetForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;
        private readonly IApplicationUser _userService;

        public ForumController(IForum forumService, IPost postService, IUpload uploadService, IConfiguration configuration, IApplicationUser userService)
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = uploadService;
            _configuration = configuration;
            _userService = userService;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll()
                .Select(forum => new ForumListingModel {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description
            });

            var model = new ForumIndexModel
            {
                ForumList = forums
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();
        
            var postListing = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            var model = new ForumTopicModel {
                Posts = postListing,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        
        public IActionResult Create(int id)
        {
            var model = new AddForumModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "images/users/default.jpg";

            if (model.ImageUpload != null)
            {
                var blockBlob = UploadForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;
            }

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };
            await _forumService.Create(forum);

            return RedirectToAction("Index", "Forum");

        }

        private CloudBlockBlob UploadForumImage(IFormFile file)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            var container = _uploadService.GetBlobContainer(connectionString, "forum-images");
            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = contentDisposition.FileName.Trim('"');
            var blockBlob = container.GetBlockBlobReference(filename);
            blockBlob.UploadFromStreamAsync(file.OpenReadStream()).Wait();

            return blockBlob;
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);
        }
    }
}