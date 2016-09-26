using Lia.Blog.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.IRepository
{
    public interface IRepository<TEntity, TKey> where TEntity : class,IEntity<TKey>//, IAggregateRoot
    {
        TEntity GetById(TKey id);

        IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> strWhere = null, bool isAsNoTracking = true);

        Task<bool> Save(TEntity entity, bool isAdd = false);

        Task<bool> Insert(TEntity entity);

        Task<bool> Update(TEntity entity);

        IQueryable<TEntity> GetListByPage(PageParameter parameter);
    }
}
