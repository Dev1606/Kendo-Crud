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

        public ApiAjaxController(IEmpInterface empAPIInterface,ILogger<ApiAjaxController> logger)
        {
            _empAPIInterface = empAPIInterface;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}