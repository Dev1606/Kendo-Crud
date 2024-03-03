using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MVC.Controllers
{

    [Route("[controller]")]
    public class UserApiController : Controller
    {
        private readonly ILogger<UserApiController> _logger;

        public UserApiController(ILogger<UserApiController> logger)
        {
            _logger = logger;
        }


        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Login")]

        public IActionResult Login()
        {
            return View();
        }


        [Route("Register")]
        public IActionResult Register()
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