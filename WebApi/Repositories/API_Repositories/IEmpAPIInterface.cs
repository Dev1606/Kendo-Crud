using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories.API_Repositories
{
    public interface IEmpAPIInterface
    {
        #region User Repo Methods
        // String[] GetDepartment();
        List<EmpModel> UserGetEmpData();
        bool UserAddEmpData(EmpModel employee);

        #endregion 
    }
}