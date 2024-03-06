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
    public class ApiAjaxController : Controller
    {
        private readonly ILogger<ApiAjaxController> _logger;
        private readonly IEmpInterface _empAPIInterface;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiAjaxController(ILogger<ApiAjaxController> logger, IEmpInterface empAPIInterface, IHttpContextAccessor httpContextAccessor)
        {
            _empAPIInterface = empAPIInterface;
            _logger = logger;
            empAPIInterface = _empAPIInterface;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                if (session.GetInt32("role") == 1)
                {
                    return View();
                }else{
                    return RedirectToAction("UserIndex");
                }
            }else{
                return RedirectToAction("Login","UserApi");
            }

        }

        public IActionResult UserIndex()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetString("username") != null)
            {
                if (session.GetInt32("role") == 0)
                {
                    return View();
                }else{
                    return RedirectToAction("Index");
                }
            }else{
                return RedirectToAction("Login","UserApi");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}