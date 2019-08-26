using AspNetForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetForum.Data.Interfaces
{
    public interface IPost
    {
        Post GetById(int id);
        IEnumerable<Post> GetAll();
        IEnumerable<Post> GetFilteredPosts(string searchQuery);
        IEnumerable<Post> GetPostsByForum(int id);
        IEnumerable<Post> GetLatestPost(int numberOfPosts);

        Task Add(Post post);
        Task Delete(int id);
        Task EditPostContent(int id, string content);
        Task AddReply(PostReply reply);
        
    }
}
