using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Dto
{
    public class UserViewDto
    {
        public string UserId { get; set; }
        
        public string UserName { get; set; }
        
        public string NickName { get; set; }

        public string Password { get; set; }
        
        public string Email { get; set; }

        public bool IsRemember { get; set; }
    }

    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsRemember { get; set; }
    }
}
