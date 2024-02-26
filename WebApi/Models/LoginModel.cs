using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class LoginModel
    {
        public string c_uemail {get; set;} = string.Empty;
        public string c_password {get; set;} = string.Empty;
    }
}