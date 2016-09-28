using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Domain.Model;
using Lia.Blog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Application
{
    public class BlogService:IBlogService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlogRepository _blogRepository;

        public BlogService(IUnitOfWork unitOfWork, IBlogRepository blogRepository)
        {
            _unitOfWork = unitOfWork;
            _blogRepository = blogRepository;
        }

        public BlogInfo GetBlogById(string blogId)
        {
            return _blogRepository.GetById(blogId);
        }

        public IQueryable<BlogInfo> GetList(Expression<Func<BlogInfo, bool>> strWhere = null, bool isAsNoTracking = true)
        {
            return _blogRepository.GetList(strWhere, isAsNoTracking);
        }

        public IQueryable<BlogInfo> GetBlogList(BlogParameter parameter)
        {
            var result = _blogRepository.GetBlogList(parameter);
            return result;
        }

        public async Task<bool> Save(BlogInfo blog, bool isAdd = false)
        {
            return await _blogRepository.Save(blog, isAdd);
        }
    }
}
