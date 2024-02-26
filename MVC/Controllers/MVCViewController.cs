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
    public class MVCViewController : Controller
    {
        private readonly ILogger<MVCViewController> _logger;

        public MVCViewController(ILogger<MVCViewController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //Validate using Session 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}