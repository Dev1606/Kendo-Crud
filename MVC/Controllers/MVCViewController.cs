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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IWebHostEnvironment _environment;

        public MVCViewController(ILogger<MVCViewController> logger,IEmpInterface empRepo, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _empRepo = empRepo;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        /*
            Specify the method type on all the controllers
            use _empRepo for helper class usage
            Write the methods in their respecitve regions
        */
        public IActionResult Index()
        {
            //Validate using Session
            string username = HttpContext.Session.GetString("username");
            if(username == null)
            {
                return RedirectToAction("Login","User");
            }
            return View();
        }

        #region Admin Methods

        [HttpGet]
        public IActionResult AdminGetEmpData()
        {
            var empData = _empRepo.GetEmpData();
            return View(empData);
        }

        [HttpGet]
        public string[] GetDepartment()
        {
            return _empRepo.GetDepartment();
        }

        [HttpGet]
        public IActionResult GetEmpDetail(int id)
        {
            var empDetail = _empRepo.GetEmpDetail(id);
            return View(empDetail);
        }

        [HttpGet]
        public IActionResult AdminUpdateEmpData(int id)
        {
            ViewBag.Departments = _empRepo.GetDepartment();
            var empUpdate = _empRepo.GetEmpDetail(id);
            return View(empUpdate);
        }

        [HttpPost]
        public IActionResult AdminUpdateEmpData(EmpModel emp)
        {

            //Code For File Upload:
            if (emp.Image != null && emp.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploadsimg");
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
            
            _empRepo.UpdateEmp(emp);
            return RedirectToAction("AdminGetEmpData");
        }

        [HttpGet]
        public IActionResult AdminDeleteEmp(int id)
        {
            var empDelete = _empRepo.GetEmpDetail(id);
            return View(empDelete);
        }

        [HttpPost]
        public IActionResult AdminDeleteEmpConfirm(int id)
        {
            _empRepo.DeleteEmp(id);
            return RedirectToAction("AdminGetEmpData");
        }

        #endregion

        #region User Methods
        // Get
        [HttpGet]
        public IActionResult UserGetEmpData()
        {
            var Employees = _empRepo.UserGetEmpData();
            return View(Employees);
        }

        //department dropdown
        // [HttpGet]
        // public string[] GetDepartment()
        // {
        //     return _empRepo.GetDepartment();
        // }

        // Add
        [HttpGet]
        public IActionResult UserAddEmpData()
        {
            ViewBag.Departments = _empRepo.GetDepartment();
            return View();
        }
        [HttpPost]
        public IActionResult UserAddEmpData(EmpModel employee)
        {
            //Code For File Upload:
            if (employee.Image != null && employee.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploadsimg");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + employee.Image.FileName;
                //var uniqueFileName =  item.Image.FileName; //To Get Only File Name
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    employee.Image.CopyTo(stream);
                }

                // Save The File Path To Our DB Table In c_image Field:
                employee.c_empimage = uniqueFileName;
            }

            _empRepo.UserAddEmpData(employee);
            return RedirectToAction("UserGetEmpData");
        }

        #endregion 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}