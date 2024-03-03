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
            return Ok(new{success=true,message="Student updated successfully"});
        }

        [HttpDelete]
        [Route("DeleteEmpData")]
        public IActionResult DeleteEmp(int id)
        {
            _empAPIInterface.DeleteEmp(id);
            return Ok(new{success=true,message="Student deleted successfully"});
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

        public IActionResult UserAddEmpData([FromForm]EmpApiModel emp,IFormFile file)
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
            //emp.c_empimage = imageUrl;

            // var shift = Request.Form["c_shift"].ToList();
            // emp.c_shift = string.Join(", ", shift);
            // HttpContext.Session.SetInt32("userid", emp.c_userid.GetValueOrDefault());

            _empAPIInterface.UserAddEmpData(emp,imageUrl);

        public IActionResult UserAddEmpData([FromForm] EmpModel emp)
        {
            //Code For File Upload:
            if (emp.Image != null && emp.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine("../MVC/wwwroot", "uploadsimg");
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