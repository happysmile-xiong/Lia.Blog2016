using Lia.Blog.Utils.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Entity
{
    public class Category : EntityBase<string>//: IAggregateRoot<string>
    {
        public Category()
        {
            BlogInfos = new HashSet<BlogInfo>();
            //Id = CombHelper.NewComb().ToString();
        }

        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; }

        public ICollection<BlogInfo> BlogInfos { get; set; }
    }
}
