using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Models.ApiModel;

namespace WebApi.Repositories.API_Repositories
{
    public interface IEmpAPIInterface
    {

        #region User Repo Methods
        // String[] GetDepartment();
        List<EmpModel> UserGetEmpData();
        bool UserAddEmpData(EmpModel employee);
        bool UserAddEmpData(EmpApiModel employee,string c_empimage);

        //Added this code for kendo api call: 
        bool UserAddKendoEmpData(EmpApiModel employee);
        #endregion 
          

        #region Admin Repo Methods
        List<EmpModel> GetEmpData();
        string[] GetDepartment();
        EmpModel GetEmpDetail(int id);
        bool DeleteEmp(int id);
        bool UpdateEmp(EmpApiModel emp);
        #endregion 

    }
}