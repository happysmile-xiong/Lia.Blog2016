using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lia.Blog.Domain.Dto
{
    public class LoginInfo
    {
        private string _key = "loginInfo";
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string NickName { get; set; }

        public string Key { get { return _key; } }
    }
}
