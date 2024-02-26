using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class EmpModel
    {
        public int c_empid {get; set;} = 0;
        public string c_empname {get; set;} = string.Empty;
        public string c_empgender {get; set;} = string.Empty;
        public DateTime c_dob {get; set;} 
        public string[] c_shift {get; set;} = new string[0];
        public string c_department {get; set;} = string.Empty;
        public string c_empimage {get; set;} = string.Empty;
        public IFormFile? c_image;
    }
}