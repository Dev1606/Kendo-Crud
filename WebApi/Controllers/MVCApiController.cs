using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        [Route("GetTokenData")]
        public IActionResult GetTokenData(string usertoken)
        {
            // string token = Request.Headers["Authorization"].Substring("Bearer ".Length);
            string token = usertoken;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            var claims = jwtSecurityToken.Claims;

            string userId = claims.Single(c => c.Type == "Userid").Value;
            string userName = claims.Single(c => c.Type == "UserName").Value;
            // string userName = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string email = claims.Single(c => c.Type == "Email").Value;

            // Use the retrieved claims in your controller logic
            return Ok(new { userId, userName, email });
        }

        #region Admin API Calls

        [HttpGet]
        [Route("GetEmpData")]
        [Authorize]
        // [Authorize]
        public IActionResult GetEmpData()
        {
            var emplist = _empAPIInterface.GetEmpData();
            return Ok(emplist);
        }

        [HttpGet]
        [Route("GetDropDepartment")]
        [Authorize]
        // [Authorize]
        public string[] GetDepartment()
        {
            return _empAPIInterface.GetDepartment();
        }

        [HttpGet]
        [Route("GetEmpDetail")]
        [Authorize]
        public IActionResult GetEmpDetail(int id)
        {
            var emp = _empAPIInterface.GetEmpDetail(id);
            return Ok(emp);
        }

        [HttpPut]
        [Route("UpdateEmpData")]
        [Authorize]
        public IActionResult UpdateEmp([FromForm] EmpApiModel emp, IFormFile? Image)
        {
            if (Image == null)
            {
                var data = _empAPIInterface.GetEmpDetail(emp.c_empid);
                emp.c_empimage = data.c_empimage;
            }
            else
            {
                if (string.IsNullOrEmpty(Image.FileName))
                {
                    return BadRequest("Image file name is null or empty.");
                }
                string filename = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                var filePath = Path.Combine(@"../MVC/wwwroot/", "uploadsimg", filename);
                if (string.IsNullOrEmpty(filePath))
                {
                    return BadRequest("File path is null or empty.");
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(stream);
                }

                emp.c_empimage = filename;
            }

            _empAPIInterface.UpdateEmp(emp);
            return Ok(new { success = true, message = "Student updated successfully" });
        }


        [HttpDelete]
        [Route("DeleteEmpData")]
        [Authorize]
        public IActionResult DeleteEmp(int id)
        {
            _empAPIInterface.DeleteEmp(id);
            return Ok(new { success = true, message = "Student deleted successfully" });
        }

        #endregion

        #region User API Calls

        // UserGetEmpData
        [HttpGet]
        [Route("UserGetEmpData")]
        [Authorize]
        public IActionResult UserGetEmpData()
        {
            var emplist = _empAPIInterface.UserGetEmpData();
            return Ok(emplist);
        }

        // UserAddEmpData
        [HttpPost]
        [Route("UserAddEmpData")]
        // [Authorize]
        public IActionResult UserAddEmpData([FromForm] EmpApiModel emp, IFormFile file)
        {
            Console.WriteLine(file);
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
            _empAPIInterface.UserAddEmpData(emp, imageUrl);
            return Ok(new { success = true, message = "Employee Added !!!!!" });
        }

        #endregion
    }
}