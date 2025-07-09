using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceProjectBLL.Dto.AccountDto
{
    public class AuthModel
    {
        public string Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public List<string> Errors { get; set; }
        public string Token { get; set; }
        public DateTime ExpairationDate { get; set; }
    }
}
