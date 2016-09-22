using Lia.Blog.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.IRepository
{
    public interface ICategoryRepository : IRepository<Category, string>
    {
    }
}
