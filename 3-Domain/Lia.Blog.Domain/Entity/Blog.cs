using Lia.Blog.Utils.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Entity
{
    [Table("Blog")]
    public class BlogInfo : EntityBase<string>//: IAggregateRoot<string>
    {
        public BlogInfo()
        {
            Comments = new HashSet<Comment>();
            //Id = CombHelper.NewComb().ToString();
        }

        //public string Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        [Required]
        public string Title { get; set;}

        /// <summary>
        /// 文章内容
        /// </summary>
        [MaxLength(400000)]
        [Required]
        public string Body { get; set; }

        /// <summary>
        /// 分类Id
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 作者Id
        /// </summary>
        public string AuthorId { get; set; }


        [StringLength(50)]
        public string Url { get; set; }
        
        /// <summary>
        /// 是否为转发文章
        /// </summary>
        public bool IsForwarding { get; set; }


        public virtual Category Category { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
