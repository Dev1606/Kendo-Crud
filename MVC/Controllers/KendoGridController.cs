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
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KendoGridController(ILogger<KendoGridController> logger, IEmpInterface empRepo, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _empRepo = empRepo;
            _hostingEnvironment = environment;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult UserKendoMVC()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("userid") != null)
            {
                if (session.GetInt32("isRole") == 0)
                {
                    return View();
                }else{
                    return RedirectToAction("AdminKendoMVC", "KendoGrid");
                }
            }else{
                return RedirectToAction("Login", "KendoComponent");
            }
        }
        public IActionResult AdminKendoMVC()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("userid") != null)
            {
                if (session.GetInt32("isRole") == 1)
                {
                    return View();
                }else{
                    return RedirectToAction("UserKendoMVC", "KendoGrid");
                }
            }else{
                return RedirectToAction("Login", "KendoComponent");
            }
        }

        [Produces("application/json")]
        [HttpGet]
        public IActionResult UserGetEmpData()
        {
            var empData = _empRepo.UserGetEmpData();
            return Json(empData);
        }

        public IActionResult UserAddEmpData(EmpModel emp)
        {
            emp.c_empimage = file;
            var empData = _empRepo.UserAddEmpData(emp);
            return Json(empData);
        }
        [HttpGet]
        public string[] GetDepartment()
        {
            return _empRepo.GetDepartment();
        }
        static string file = "";


        #region Admin

        [Produces("application/json")]
        [HttpGet]
        public IActionResult AdminGetEmpData()
        {
            var empData = _empRepo.GetEmpData();
            return Json(empData);
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
            Console.WriteLine(emp);
            Console.WriteLine("DOB" + emp.c_dob);
            Console.WriteLine("Image" + emp.c_empimage + "IF" + emp.Image);

            //Code For File Upload:
            if (emp.Image != null && emp.Image.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploadsimg");
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
            emp.c_empimage = file;
            _empRepo.UpdateEmp(emp);
            return Json(new { success = true, message = "Employee updated" });
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
            Console.WriteLine("Delete called" + id);
            _empRepo.DeleteEmp(id);
            return Json(new { success = true, message = "Employee updated" });
        }

        [HttpPost]
        public IActionResult UploadPhoto(EmpModel emp)
        {
            if (emp.Image != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + emp.Image.FileName;
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploadsimg", uniqueFileName);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {

                    emp.Image.CopyTo(stream);
                }

                file = uniqueFileName;
                // emp.c_empimage=file;
                // _empRepo.UpdateEmp(emp);
            }
            return Json("Image Uploaded");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        #endregion
    }
}