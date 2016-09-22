using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.Model;

namespace Lia.Blog.Domain.IRepository
{
    public interface IBlogRepository : IRepository<BlogInfo, string>
    {
        IQueryable<BlogInfo> GetBlogList(BlogParameter parameter);
    }
}
