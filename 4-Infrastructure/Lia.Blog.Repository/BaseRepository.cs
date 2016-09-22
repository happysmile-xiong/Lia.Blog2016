using Lia.Blog.Domain;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Domain.Model;
using Lia.Blog.Infrastructure;
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
        public readonly IDbContext _context;

        public BaseRepository(IDbContext dbContext)
        {
            _context = dbContext;//(BlogDbContext)dbContext;
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

        public IQueryable<TEntity> GetListByPage(PageParameter parameter)
        {
            if (string.IsNullOrEmpty(parameter.OrderBy))
            {
                parameter.OrderBy = "Id";
                parameter.IsAsc = false;
            }
            if (parameter.PageSize == 0)
                parameter.PageSize = 30;
            if (parameter.PageIndex == 0)
                parameter.PageIndex = 1;
            return this.GetAll().GetPageList<TEntity>(parameter);
        }

        public async Task<bool> Insert(TEntity entity)
        {
            entity.CreationTime = DateTime.Now;
            entity.LastModifiedTime = DateTime.Now;
            _context.Set<TEntity>().Attach(entity);
            _context.Entry<TEntity>(entity).State = System.Data.Entity.EntityState.Added;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
