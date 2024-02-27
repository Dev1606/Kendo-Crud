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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserInterface _userrepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(ILogger<UserController> logger,IUserInterface userrepo,IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userrepo = userrepo;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            var status = _userrepo.RegistrationDetail(user);
            if(status){
                return View("Login");
            }else{
                return RedirectToAction("Register","User");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel user)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            if (session.GetInt32("userid") == null)
            {
                if (_userrepo.Login(user))
                {
                    if (session.GetInt32("isRole") == 0)
                    {
                        // User
                        return RedirectToAction("Index", "MVCView");
                    }
                    else if (session.GetInt32("isRole") == 1)
                    {
                        // Admin
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // login.ErrorMessage = "Invalid email or password";
                        return View();
                    }
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
            }
            else
            {
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult Logout()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.Clear();
            return RedirectToAction("Login","User");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}