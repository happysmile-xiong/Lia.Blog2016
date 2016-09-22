using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Model
{
    public class PageParameter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int RecordCount { get; set; }
        public string OrderBy { get; set; }
        public bool IsAsc { get; set; }
        public int PageCount
        {
            get
            {
                return RecordCount % PageSize > 0 ? (RecordCount / PageSize + 1) : RecordCount / PageSize;
            }
        }
    }

    public class BlogParameter : PageParameter
    {
        public string AuthorId { get; set; }
    }
}
