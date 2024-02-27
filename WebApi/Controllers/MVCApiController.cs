using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Repositories.API_Repositories;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MVCApiController : ControllerBase
    {
        private readonly IEmpAPIInterface _empAPIInterface;

        private readonly IWebHostEnvironment _environment;
        public MVCApiController(IEmpAPIInterface empAPIInterface, IWebHostEnvironment environment)
        {
            _empAPIInterface = empAPIInterface;
            _environment = environment;
        }

        #region Admin API Calls
        #endregion

        #region User API Calls

        // UserGetEmpData
        [HttpGet]
        [Route("UserGetEmpData")]
        public IActionResult UserGetEmpData()
        {
            var emplist = _empAPIInterface.UserGetEmpData();
            return Ok(emplist);
        }

        // UserAddEmpData
        [HttpPost]
        [Route("UserAddEmpData")]
        public IActionResult UserAddEmpData([FromForm] EmpModel emp)
        {
            //Code For File Upload:
            if (emp.Image != null && emp.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine("D:/Priyanshi_Intern/Master_Project/Kendo-Crud/MVC/wwwroot", "uploadsimg");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + emp.Image.FileName;
                //var uniqueFileName =  item.Image.FileName; //To Get Only File Name
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    emp.Image.CopyTo(stream);
                }

                // Save The File Path To Our DB Table In c_image Field:
                emp.c_empimage = uniqueFileName;
            }
            _empAPIInterface.UserAddEmpData(emp);
            return Ok("Employee Data Added Successfully!");
        }

        #endregion
    }
}