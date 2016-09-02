using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.IRepository
{
    public interface IRepository<TEntity, Tkey> where TEntity : class,IEntity<Tkey>//, IAggregateRoot
    {
        IQueryable<TEntity> Get(string id);

        IQueryable<TEntity> GetAll();
    }
}
