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
    public class KendoGridController : Controller
    {
        private readonly ILogger<KendoGridController> _logger;
         private readonly IEmpInterface _empRepo;
          private readonly IWebHostEnvironment _environment;
          private readonly IHttpContextAccessor _httpContextAccessor;

        public KendoGridController(ILogger<KendoGridController> logger,IEmpInterface empRepo, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _empRepo = empRepo;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult AdminKendoAPI()
        {
            return View();
        }
        public IActionResult AdminKendoMVC()
        {
            return View();
        }
        #region Admin
        
        [Produces("application/json")]
        [HttpGet]
        public IActionResult AdminGetEmpData()
        {
            var empData = _empRepo.GetEmpData();
            return Json(empData);
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
            return Json(empDetail);
        }

        [HttpGet]
        public IActionResult AdminUpdateEmpData(int id)
        {
            ViewBag.Departments = _empRepo.GetDepartment();
            var empUpdate = _empRepo.GetEmpDetail(id);
            return Json(empUpdate);
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
            return Json(new{success=true,message="Employee updated"});
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
            return Json(new{success=true,message="Employee updated"});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        #endregion
    }
}