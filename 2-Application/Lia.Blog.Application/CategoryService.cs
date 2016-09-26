using Lia.Blog.Application.Interfaces;
using Lia.Blog.Domain.Entity;
using Lia.Blog.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Application
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _caregoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _caregoryRepository = categoryRepository;
        }

        public IList<Category> GetCategories()
        {
            return _caregoryRepository.GetList().ToList();
        }

        public Category GetCateById(string id)
        {
            return _caregoryRepository.GetById(id);
        }
    }
}
