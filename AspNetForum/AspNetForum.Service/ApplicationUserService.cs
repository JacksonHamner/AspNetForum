using AspNetForum.Data;
using AspNetForum.Data.Interfaces;
using AspNetForum.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetForum.Service
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return GetAll()
                .FirstOrDefault(user => user.Id == id);
        }

        public async Task UpdateUserRating(string id, Type type)
        {
            //id is userId
            var user = GetById(id);

            user.Rating = CalculateUserRating(type, user.Rating);

            await _context.SaveChangesAsync();

        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;
            if(type == typeof(Post))
            {
                inc = 1;
            }
            if (type == typeof(PostReply))
            {
                inc = 3;
            }

            return userRating + inc;
        }
    }
}
