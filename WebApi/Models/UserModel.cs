using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserModel
    {
        public int c_uid {get; set;} = 0;
        [Required(ErrorMessage ="Please  enter your username")]
        public string c_uname {get; set;} = string.Empty;
        [Required(ErrorMessage ="Please provide an email")]
        [RegularExpression(@"^\w+([-+.']\w+)@\w+([-.]\w+)\.\w+([-.]\w+)*$", ErrorMessage = "Please enter a valid email address.")]
        public string c_uemail {get; set;} = string.Empty;
        [Required(ErrorMessage ="Please provide a password")]
        [RegularExpression(@"^(?=.[a-z])(?=.[A-Z])(?=.\d)(?=.[!@#$%^&()-_=+])[A-Za-z\d!@#$%^&()-_=+]{6,}$",ErrorMessage ="Password must be atleast 6 characteras long,atleast one lowercase,uppercase,digits and special character")]
        [DataType(DataType.Password)]
        public string c_password {get; set;} = string.Empty;
        [Required(ErrorMessage ="Please confirm your password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string c_confirmpassword {get; set;} = string.Empty;

        // 0 -> User and 1 -> Admin
        public int c_role {get; set;} = 0;
    }
}