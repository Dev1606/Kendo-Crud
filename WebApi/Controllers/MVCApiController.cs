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
    public class MVCApiController : ControllerBase
    {
        private readonly IEmpInterface _empInterface;
        public MVCApiController(IEmpInterface empInterface){
            _empInterface = empInterface;
        }

        #region Admin API Calls
        #endregion

        #region User API Calls
        #endregion
    }
}