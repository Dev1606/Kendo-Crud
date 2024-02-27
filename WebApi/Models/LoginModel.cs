using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter Email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email Pattern Doesn't Match")]
        public string c_uemail {get; set;} = string.Empty;

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please Enter Password")]
        public string c_password {get; set;} = string.Empty;
    }
}