using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserModel
    {
        public int c_uid {get; set;} = 0;
        public string c_uname {get; set;} = string.Empty;
        public string c_uemail {get; set;} = string.Empty;
        public string c_password {get; set;} = string.Empty;
        public string c_confirmpassword {get; set;} = string.Empty;

        // 0 -> User and 1 -> Admin
        public int c_role {get; set;} = 0;
    }
}