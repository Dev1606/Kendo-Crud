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
        public MVCApiController(IEmpAPIInterface empAPIInterface){
            _empAPIInterface = empAPIInterface;
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
        #endregion
    }
}