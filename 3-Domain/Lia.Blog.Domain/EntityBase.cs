using Lia.Blog.Utils.Data;
using Lia.Blog.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain
{
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// 初始化一个<see cref="EntityBase{TKey}"/>类型的新实例
        /// </summary>
        protected EntityBase()
        {
            if (typeof(TKey) == typeof(Guid) || typeof(TKey) == typeof(string))
            {
                Id = CombHelper.NewComb().CastTo<TKey>();
            }
            LastModifiedTime = DateTime.Now;
        }

        public TKey Id { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastModifiedTime { get; set; }
    }
}
