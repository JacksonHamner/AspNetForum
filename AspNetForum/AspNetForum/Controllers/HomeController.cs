using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AspNetForum.Models;
using System;
using AspNetForum.ViewModels.Home;
using AspNetForum.Data.Interfaces;
using System.Linq;
using AspNetForum.ViewModels.Post;
using AspNetForum.ViewModels.Forum;
using AspNetForum.Data.Models;

namespace AspNetForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;

        public HomeController(IPost postService, IForum forumService)
        {
            _postService = postService;
            _forumService = forumService;
        }

        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();
            return View(model);
        }

  

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPost(10);

            var posts = latestPosts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created.ToString(),
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
            });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };

        }
    }
}
