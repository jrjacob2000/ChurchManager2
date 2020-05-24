using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChurchManagerApi.Models
{
    public class RegisterUserModel : LoginModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsAccountant { get; set; }
        public bool IsEncoder { get; set; }
    }
}
