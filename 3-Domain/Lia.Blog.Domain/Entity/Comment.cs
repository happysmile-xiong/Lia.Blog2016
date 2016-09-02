using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Entity
{
    public class Comment : EntityBase<string>//: IAggregateRoot<string>
    {
        //public string Id { get; set; }

        [StringLength(500)]
        public string Body { get; set; }

        public string BlogId { get; set; }

        public string PostId { get; set; }

        public virtual BlogInfo BlogInfo { get; set; }

        public virtual User User { get; set; }
    }
}
