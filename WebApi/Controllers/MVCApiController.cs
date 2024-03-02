using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.ApiModel;
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

        [HttpGet]
        [Route("GetEmpData")]
        public IActionResult GetEmpData()
        {
            var emplist = _empAPIInterface.GetEmpData();
            return Ok(emplist);
        }  

        [HttpGet]
        [Route("GetDropDepartment")]
        public string[] GetDepartment()
        {
            return _empAPIInterface.GetDepartment();
        }

        [HttpGet]
        [Route("GetEmpDetail")]
        public IActionResult GetEmpDetail(int id)
        {
            var emp = _empAPIInterface.GetEmpDetail(id);
            return Ok(emp);
        }

        [HttpPut]
        [Route("UpdateEmpData")]
        public IActionResult UpdateEmp(EmpModel emp)
        {
            _empAPIInterface.UpdateEmp(emp);
            return Ok("Employee Updated Succcessfully");
        }

        [HttpDelete]
        [Route("DeleteEmpData")]
        public IActionResult DeleteEmp(int id)
        {
            _empAPIInterface.DeleteEmp(id);
            return Ok("Employee Deleted Succcessfully");
        }

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
        public IActionResult UserAddEmpData([FromForm] EmpApiModel emp,IFormFile file)
        {
            //Code For File Upload:
            var folderPath = @"..\MVC\wwwroot\uploadsimg";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);
            var fileName = Path.GetFileName(file.FileName);

            if (System.IO.File.Exists(filePath))
            {
                fileName = Guid.NewGuid().ToString() + "_" + fileName;
                filePath = Path.Combine(folderPath, fileName);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            var imageUrl = Path.Combine("/uploadsimg", fileName);
            //emp.c_empimage = imageUrl;

            // var shift = Request.Form["c_shift"].ToList();
            // emp.c_shift = string.Join(", ", shift);
            // HttpContext.Session.SetInt32("userid", emp.c_userid.GetValueOrDefault());

            _empAPIInterface.UserAddEmpData(emp,imageUrl);
            return Ok("Employee Data Added Successfully!");
        }

        #endregion
    }
}