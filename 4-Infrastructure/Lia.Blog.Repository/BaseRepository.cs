using Lia.Blog.Domain;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity,TKey> where TEntity : class,IEntity<TKey>
    {
        public readonly IQueryable<TEntity> _entities;

        public BaseRepository(IDbContext dbContext)
        {
            _entities = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(string id)
        {
            return _entities.Where(t => t.Id.Equals(id));
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entities;
        }
    }
}
