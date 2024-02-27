using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Repositories.API_Repositories;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class MVCAjaxController : Controller
    {
        private readonly ILogger<MVCAjaxController> _logger;
        private readonly IEmpAPIInterface _empAPIRepo;

        public MVCAjaxController(ILogger<MVCAjaxController> logger, IEmpAPIInterface empAPIRepo)
        {
            _logger = logger;
            _empAPIRepo = empAPIRepo;
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