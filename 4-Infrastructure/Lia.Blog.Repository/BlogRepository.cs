using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Domain.Model;
using Lia.Blog.Infrastructure;
using Lia.Blog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Repository
{
    public class BlogRepository : BaseRepository<BlogInfo, string>, IBlogRepository
    {
        public BlogRepository(IDbContext dbContext) : base(dbContext)
        {
            ((BlogDbContext)dbContext).Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<BlogInfo> GetBlogList(BlogParameter parameter)
        {
            var list = base.GetListByPage(parameter);
            if (!string.IsNullOrEmpty(parameter.AuthorId))// && Guid.TryParse(parameter.AuthorId, out userId))
            {
                list = list.Where(b => b.AuthorId == parameter.AuthorId);
            }
            return list;
        }
    }
}
