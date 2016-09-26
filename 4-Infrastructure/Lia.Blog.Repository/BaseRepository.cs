using Lia.Blog.Domain;
using Lia.Blog.Domain.IRepository;
using Lia.Blog.Domain.Model;
using Lia.Blog.Infrastructure;
using Lia.Blog.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Repository
{
    public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity,TKey> 
        where TEntity : class,IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public readonly DbSet<TEntity> _entities;
        public readonly IDbContext _context;

        public BaseRepository(IDbContext dbContext)
        {
            _context = dbContext;
            _entities = dbContext.Set<TEntity>();
        }

        public TEntity GetById(TKey id)
        {
            if (id == null || string.IsNullOrEmpty(id.ToString()))
                return null;
            return this.GetList(t => t.Id.Equals(id)).FirstOrDefault();
        }

        public IQueryable<TEntity> GetList(Expression<Func<TEntity, bool>> strWhere = null, bool isAsNoTracking = true)
        {
            var list = _entities.AsQueryable();
            if (strWhere != null)
                list.Where(strWhere);
            if (isAsNoTracking)
                list = _entities.AsNoTracking().AsQueryable();
            
            return list;
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
            return this.GetList().GetPageList<TEntity>(parameter);
        }

        public async Task<bool> Save(TEntity entity, bool isAdd = false)
        {
            entity.LastModifiedTime = DateTime.Now;
            if(isAdd)
            {
                entity.CreationTime = DateTime.Now;
                _entities.Add(entity);
            }
            else
            {
                _context.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Insert(TEntity entity)
        {
            entity.CreationTime = DateTime.Now;
            entity.LastModifiedTime = DateTime.Now;
            _entities.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(TEntity entity)
        {
            entity.LastModifiedTime = DateTime.Now;
            _context.Entry<TEntity>(entity).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
