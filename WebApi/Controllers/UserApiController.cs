using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories.API_Repositories;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserAPIInterface _userAPIInterface;
        public UserApiController(IUserAPIInterface userAPIInterface){
            _userAPIInterface = userAPIInterface;
        }
    }
}