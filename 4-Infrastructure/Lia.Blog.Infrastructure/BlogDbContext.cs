using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using Lia.Blog.Infrastructure.Interfaces;
using Lia.Blog.Domain.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Lia.Blog.Infrastructure.Migrations;

namespace Lia.Blog.Infrastructure
{
    public class BlogDbContext: IdentityDbContext<User>,IDbContext
    {
        public BlogDbContext() : base("name=BlogConn")
        {
            //每次重新启动总是初始化数据库到最新版本（连接字符串为“BlogConn”）
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<BlogDbContext, Configuration>("BlogConn"));
        }

        public static BlogDbContext Create()
        {
            return new BlogDbContext();
        }

        //#region Implemention of IUnitOfWork
        ///// <summary>
        ///// 获取或设置 是否开启事务提交
        ///// </summary>
        //public bool TransactionEnabled { get; set; }

        //public virtual int ExecuteSqlCommand(Tran transactionalBehavior, string sql, params object[] parameters)
        //{
        //    var behavior = transactionalBehavior == Tran.DoNotEnsureTransction ?
        //        System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction :
        //        System.Data.Entity.TransactionalBehavior.EnsureTransaction;
        //    return Database.ExecuteSqlCommand(behavior, sql, parameters);
        //}

        //public virtual IEnumerable<TElement> SqlQuery<TElement>(string sql,params object[] parameters)
        //{
        //    return Database.SqlQuery<TElement>(sql, parameters);
        //}

        //public virtual IEnumerable SqlQuery(Type elementType,string sql,params object[] parameters)
        //{
        //    return Database.SqlQuery(elementType, sql, parameters);
        //}

        ///// <summary>
        ///// 提交当前单元操作的更改
        ///// </summary>
        ///// <returns>操作影响的行数</returns>
        //public override int SaveChanges()
        //{
        //    return base.SaveChanges();
        //}
        //#endregion


        public virtual DbSet<BlogInfo> Blogs { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        //public virtual DbSet<Role> Roles { get; set; }
        //public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Category和Blog 一对多
            modelBuilder.Entity<Category>().HasMany(c => c.BlogInfos).WithRequired(b => b.Category).HasForeignKey(b => b.CategoryId).WillCascadeOnDelete(false);

            //Blog和Comment 一对多
            modelBuilder.Entity<BlogInfo>().HasMany(b => b.Comments).WithRequired(c => c.BlogInfo).HasForeignKey(c => c.BlogId).WillCascadeOnDelete(false);

            //User和Blog 一对多
            modelBuilder.Entity<User>().HasMany(u => u.BlogInfos).WithRequired(b => b.User).HasForeignKey(b => b.AuthorId).WillCascadeOnDelete(false);

            //User和Comment 一对多
            modelBuilder.Entity<User>().HasMany(u => u.Comments).WithOptional(c => c.User).HasForeignKey(c => c.PostId);

            //User和Role 多对多
            //modelBuilder.Entity<User>().HasMany(u => u.Roles).WithMany(r => r.Users).Map(act => act.ToTable("UserRoles").MapLeftKey("UserId").MapRightKey("RoleId"));

            base.OnModelCreating(modelBuilder);
        }
    }

    public class IdentityDbInit:NullDatabaseInitializer<BlogDbContext>
    {

    }
}
