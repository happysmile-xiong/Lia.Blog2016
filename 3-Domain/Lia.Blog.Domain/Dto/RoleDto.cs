using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Dto
{
    public class RoleDto : IdentityRole
    {
        public RoleDto() : base() { }

        public RoleDto(string name) : base(name) { }
    }
}
