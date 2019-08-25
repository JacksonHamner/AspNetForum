using AspNetForum.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetForum.Data.Interfaces
{
    public interface IForum
    {
        Forum GetById(int id);
        IEnumerable<Forum> GetAll();
        IEnumerable<ApplicationUser> GetAllActiveUsers();

        Task Create(Forum forum);
        Task Delete(int id);
        Task UpdateForumTitle(int id, string title);
        Task UpdateForumDescription(int id, string description);
    }
}
