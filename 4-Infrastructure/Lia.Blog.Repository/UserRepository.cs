using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Repository
{
    public class UserRepository : BaseRepository<User,string>, IUserRepository
    {
        public UserRepository(IDbContext dbContext) : base(dbContext)
        {
        }

        public User GetByName(string name)
        {
            var result = new User();
            if (string.IsNullOrEmpty(name))
                return result;

            return _entities.Where(u => u.UserName.Equals(name, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
        }
    }
}
