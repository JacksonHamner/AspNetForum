using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetForum.Data;
using AspNetForum.Data.Interfaces;
using AspNetForum.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetForum.Service
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext _context;

        public ForumService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task Create(Forum forum)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Forum> GetAll()
        {
            return _context.Forums
                .Include(forum => forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new System.NotImplementedException();
        }

        public Forum GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateForumDescription(int id, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateForumTitle(int id, string title)
        {
            throw new System.NotImplementedException();
        }
    }
}
