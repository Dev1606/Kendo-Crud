using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class MVCViewController : Controller
    {
        private readonly ILogger<MVCViewController> _logger;
        private readonly IEmpInterface _empRepo;

        public MVCViewController(ILogger<MVCViewController> logger, IEmpInterface empRepo)
        {
            _logger = logger;
            _empRepo = empRepo;
        }

        /*
            Specify the method type on all the controllers
            use _empRepo for helper class usage
            Write the methods in their respecitve regions
        */
        public IActionResult Index()
        {
            //Validate using Session 
            return View();
        }

        #region Admin Methods
        #endregion

        #region User Methods
        // Get
        [HttpPost]
        public IActionResult UserGetEmpData()
        {
            var Employees = _empRepo.UserGetEmpData();
            return Json(Employees);
        }

        // Add
        [HttpPost]
        public IActionResult UserAddEmpData(EmpModel employee)
        {
            var status = _empRepo.UserAddEmpData(employee);
            if (status)
            {
                return Json(new { Success = true, message = "Employee Added Successfully !!!!" });
            }
            else
            {
                return Json(new { Success = false, message = "There was some Error" });
            }
        }


        #endregion 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}