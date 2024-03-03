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

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult KendoAPI()
        {
            return View();
        }
        public IActionResult KendoMVC()
        {
            return View();
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
    }
}