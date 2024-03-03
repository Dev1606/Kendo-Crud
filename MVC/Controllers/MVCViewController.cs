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
            var session = _httpContextAccessor.HttpContext.Session;
            //Validate using Session
            string username = session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (session.GetInt32("isRole") == 0)
                {
                    return RedirectToAction("UserGetEmpData", "MVCView");
                }
                else
                {
                    return RedirectToAction("AdminGetEmpData", "MVCView");
                }
            }
        }

        #region Admin Methods

        [HttpGet]
        public IActionResult AdminGetEmpData()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            //Validate using Session
            string username = session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (session.GetInt32("isRole") == 1)
                {
                    var empData = _empRepo.GetEmpData();
                    return View(empData);
                }
                else
                {
                    return RedirectToAction("UserGetEmpData");
                }
            }

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
            var session = _httpContextAccessor.HttpContext.Session;
            //Validate using Session
            string username = session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (session.GetInt32("isRole") == 1)
                {
                    ViewBag.Departments = _empRepo.GetDepartment();
                    var empUpdate = _empRepo.GetEmpDetail(id);
                    return View(empUpdate);
                }
                else
                {
                    return RedirectToAction("UserGetEmpData");
                }
            }
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
            var session = _httpContextAccessor.HttpContext.Session;
            //Validate using Session
            string username = session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (session.GetInt32("isRole") == 1)
                {
                    var empDelete = _empRepo.GetEmpDetail(id);
                    return View(empDelete);
                }
                else
                {
                    return RedirectToAction("UserGetEmpData");
                }
            }
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
            var session = _httpContextAccessor.HttpContext.Session;
            //Validate using Session
            string username = session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (session.GetInt32("isRole") == 0)
                {
                    var Employees = _empRepo.UserGetEmpData();
                    return View(Employees);
                }
                else
                {
                    return RedirectToAction("AdminGetEmpData");
                }
            }

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
            var session = _httpContextAccessor.HttpContext.Session;
            //Validate using Session
            string username = session.GetString("username");
            if (username == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                if (session.GetInt32("isRole") == 0)
                {
                    ViewBag.Departments = _empRepo.GetDepartment();
                    return View();
                }
                else
                {
                    return RedirectToAction("AdminGetEmpData");
                }
            }

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