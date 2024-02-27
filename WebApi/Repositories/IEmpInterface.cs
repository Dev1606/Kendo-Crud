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
        List<EmpModel> GetEmpData();
        string[] GetDepartment();
        EmpModel GetEmpDetail(int id);
        bool DeleteEmp(int id);
        bool UpdateEmp(EmpModel emp);
        #endregion

        #region User Repo Methods

        String[] GetDepartment();
        List<EmpModel> UserGetEmpData();
        bool UserAddEmpData(EmpModel employee);

        #endregion 
    }
}