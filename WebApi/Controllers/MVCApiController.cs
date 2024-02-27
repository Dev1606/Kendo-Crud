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
    public class MVCApiController : ControllerBase
    {
        private readonly IEmpAPIInterface _empAPIInterface;
        public MVCApiController(IEmpAPIInterface empAPIInterface){
            _empAPIInterface = empAPIInterface;
        }

        #region Admin API Calls
        #endregion

        #region User API Calls
        #endregion
    }
}