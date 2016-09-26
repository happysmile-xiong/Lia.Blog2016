using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lia.Blog.Domain.Model;

namespace Lia.Blog.Application.Interfaces
{
    public interface IBlogService
    {
        BlogInfo GetBlogById(string blogId);

        IQueryable<BlogInfo> GetBlogList(BlogParameter parameter);

        Task<bool> Save(BlogInfo blog, bool isAdd = false);
    }
}
