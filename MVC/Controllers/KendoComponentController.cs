using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{
    // [Route("[controller]")]
    public class KendoComponentController : Controller
    {
        private readonly ILogger<KendoComponentController> _logger;

        public KendoComponentController(ILogger<KendoComponentController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Register");
        }
        public IActionResult Register(){
            return View();
        }
        public IActionResult Login(){
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}