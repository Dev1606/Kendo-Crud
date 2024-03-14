using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class KendoMVCApiController : Controller
    {
        private readonly ILogger<KendoMVCApiController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KendoMVCApiController(ILogger<KendoMVCApiController> logger,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        //[Route("Index")]
        public IActionResult Index()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                if (session.GetInt32("role") == 1)
                {
                    return View();
                }else{
                    return RedirectToAction("UserApiKendoGrid", "KendoMVCApi");
                }
            }else{
                return RedirectToAction("Login", "KendoComponent");
            }
            // return View();
        }

        //Added this cide for api call in user kendo grid:
        public IActionResult UserApiKendoGrid()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                if (session.GetInt32("role") == 0)
                {
                    return View();
                }else{
                    return RedirectToAction("Index", "KendoMVCApi");
                }
            }else{
                return RedirectToAction("Login", "KendoComponent");
            }
            // return View();
        }

        public IActionResult AdminApiKendoComp()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                if (session.GetInt32("role") == 1)
                {
                    return View();
                }else{
                    return RedirectToAction("UserApiKendoComp", "KendoMVCApi");
                }
            }else{
                return RedirectToAction("Login", "KendoComponent");
            }
        }

        public IActionResult UserApiKendoComp()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                if (session.GetInt32("role") == 0)
                {
                    return View();
                }else{
                    return RedirectToAction("AdminApiKendoComp", "KendoMVCApi");
                }
            }else{
                return RedirectToAction("Login", "KendoComponent");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}