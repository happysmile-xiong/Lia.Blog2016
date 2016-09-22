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
    public class CategoryRepository : BaseRepository<Category, string>, ICategoryRepository
    {
        public CategoryRepository(IDbContext dbContext):base(dbContext)
        {

        }
    }
}
