using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserInterface _userInterface;
        public UserApiController(IUserInterface userInterface){
            _userInterface = userInterface;
        }
    }
}