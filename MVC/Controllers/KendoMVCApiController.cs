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

        public KendoMVCApiController(ILogger<KendoMVCApiController> logger)
        {
            _logger = logger;
        }

        //[Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AdminApiKendoComp()
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