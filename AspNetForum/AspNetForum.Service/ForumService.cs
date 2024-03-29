﻿using System.Collections.Generic;
using System.Linq;
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

        public async Task Create(Forum forum)
        {
            _context.Add(forum);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var forum = GetById(id);

            _context.Remove(forum);
            await _context.SaveChangesAsync();
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
            return _context.Forums
                .Where(forum => forum.Id == id)
                .Include(forum => forum.Posts)
                    .ThenInclude(post => post.User)
                .Include(forum => forum.Posts)
                    .ThenInclude(post => post.Replies)
                        .ThenInclude(reply => reply.User)
                .FirstOrDefault();
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
