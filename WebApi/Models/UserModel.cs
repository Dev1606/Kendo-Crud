using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UserModel
    {
        public int c_uid { get; set; } = 0;

        [Required(ErrorMessage = "UserName is required")]
        public string? c_uname { get; set; } = string.Empty;
        //email
        [Required(ErrorMessage = "Please provide an email")]
         [RegularExpression(@"^[a-zA-Z0-9.!#$%&'+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)$",ErrorMessage ="Please enter a valid email")]
        public string? c_uemail { get; set; } = string.Empty;

        //Password
         [RegularExpression(@"(?!^[0-9]$)(?!^[a-zA-Z]$)^([a-zA-Z0-9]{6,15})$", ErrorMessage = "At least one digit, one alphabetic character, no special characters, and 6-15 characters in length.")]
        // [DataType(DataType.Password)]
         [Required(ErrorMessage = "Password can't be blank!")]
       
        public string? c_password { get; set; } = string.Empty;
        //confirm password
        [Compare("c_password",ErrorMessage ="'Password' and 'confirm password' must match")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password is required")]
        public string? c_confirmpassword { get; set; } = string.Empty;

        // 0 -> User and 1 -> Admin
        public int c_role { get; set; } = 0;
    }
}