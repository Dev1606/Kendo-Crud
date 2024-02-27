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

        public MVCViewController(ILogger<MVCViewController> logger, IEmpInterface empRepo, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _empRepo = empRepo;
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
        public IActionResult AdminGetEmpData()
        {
            //Validate using Session
            return View();
        }
        public IActionResult AdminUpdateEmpData()
        {
            //Validate using Session
            return View();
        }

        #endregion

        #region User Methods
        #endregion 

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}