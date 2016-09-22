using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Application.Interfaces
{
    public interface ICategoryService
    {
        IList<Category> GetCategories();
    }
}
