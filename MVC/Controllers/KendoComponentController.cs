using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using WebApi.Repositories;
using WebApi.Repositories.API_Repositories;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class KendoComponentController : Controller
    {
        private readonly ILogger<KendoComponentController> _logger;
        private readonly IEmpInterface _empRepo;
        private readonly IWebHostEnvironment _environment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KendoComponentController(ILogger<KendoComponentController> logger,IEmpInterface empRepo, IWebHostEnvironment environment, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _empRepo = empRepo;
            _environment = environment;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index(){
            return View("Register");
            //return View();
        }

        public IActionResult AdminIndex()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register(){
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserModel user)
        {
              Console.WriteLine("Details @ kendocomponentcontroller"+user.c_uemail+user.c_password+user.c_uname);
            var status = _userrepo.RegistrationDetail(user);
            Console.WriteLine(status);
            if(status){
              return Json(new {success = true, message = "Registration Successful"});
            }else{
                return Json(new {success = false, message = "Registration Fail"});
            }
        }
        [HttpGet]
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel user)
        {
            Console.WriteLine("Details @ kendocomponentcontroller"+user.c_uemail+user.c_password);
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetInt32("userid") == null)
            {
                if (_userrepo.Login(user))
                {
                    if (session.GetInt32("isRole") == 0)
                    {
                        // User
                        return Json(new {success = true, message = "Login Successful",controller="KendoGrid", action= "UserKendoMVC" });
                    }
                    else if (session.GetInt32("isRole") == 1)
                    {
                        // Admin
                        return Json(new {success = true, message = "Login Successful",controller="KendoGrid", action= "AdminKendoMVC" });
                    }
                    else
                    {
                        // login.ErrorMessage = "Invalid email or password";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }
        public IActionResult KendoAPI()
        {
            return View();
        }
        public IActionResult KendoMVC()
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

            Console.WriteLine("@Kendocomponentcontroller"+emp);
            Console.WriteLine("Emp details @ controller"+ emp.c_empid+emp.c_empname+ emp.c_empgender+emp.c_dob+emp.c_shift+ emp.c_department+emp.c_empimage);
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
            Console.WriteLine("Delete called"+id);
            _empRepo.DeleteEmp(id);
            return Json(new{success=true,message="Employee deleted"});
        }
         static string file="";
        [HttpPost]
       public IActionResult UploadPhoto(EmpModel emp)
        {
            if (emp.Image!= null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + emp.Image.FileName;
                string filepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploadsimg",uniqueFileName);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {

                    emp.Image.CopyTo(stream);
                }

                file = uniqueFileName;
            }

            return Json("Image Uploaded");
        }

        #region UserSide

        public IActionResult UserKendoMVC()
        {
            return View();
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
            // emp.c_empimage = file;
            var empData = _empRepo.UserAddEmpData(emp);
            return Json(empData);
        }
        [HttpGet]
        public string[] GetDepartment()
        {
            return _empRepo.GetDepartment();
        }
        static string file = "";

        [HttpPost]
        public IActionResult UploadPhoto(EmpModel emp)
        {
            if (emp.Image != null)
            {
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + emp.Image.FileName;
                string filepath = Path.Combine(_environment.WebRootPath, "uploadsimg", uniqueFileName);

                using (var stream = new FileStream(filepath, FileMode.Create))
                {

                    emp.Image.CopyTo(stream);
                }

                // file = uniqueFileName;
            }

            return Json("Image Uploaded");
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        #endregion
    }
}