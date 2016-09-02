using System;

namespace Lia.Blog.Domain
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }

        DateTime CreationTime { get; set; }

        DateTime LastModifiedTime { get; set; }
    }
}
