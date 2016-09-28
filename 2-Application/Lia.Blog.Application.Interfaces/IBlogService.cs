using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lia.Blog.Domain.Model;
using System.Linq.Expressions;

namespace Lia.Blog.Application.Interfaces
{
    public interface IBlogService
    {
        BlogInfo GetBlogById(string blogId);

        IQueryable<BlogInfo> GetList(Expression<Func<BlogInfo, bool>> strWhere = null, bool isAsNoTracking = true);

        IQueryable<BlogInfo> GetBlogList(BlogParameter parameter);

        Task<bool> Save(BlogInfo blog, bool isAdd = false);
    }
}
