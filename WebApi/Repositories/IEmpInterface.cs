using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories
{
    public interface IEmpInterface
    {
        #region Admin Repo Methods
        #endregion

        #region User Repo Methods
        List<EmpModel> UserGetEmpData();
        bool UserAddEmpData(EmpModel employee);

        #endregion 
    }
}