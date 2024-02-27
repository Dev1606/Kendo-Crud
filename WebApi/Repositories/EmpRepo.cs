using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class EmpRepo : IEmpInterface
    {
        private readonly string? _ConnectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmpRepo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _ConnectionString = configuration.GetConnectionString("ConStr");
            _httpContextAccessor = httpContextAccessor;
        }

        /*
            User **** conn **** as connection object.
            Everthing must be enclosed with in try catch block with proper exception
            _ConnectionString variable has the connection string.
        */

        #region Admin Repo Methods
        #endregion

        #region User Repo Methods

        public bool UserAddEmpData(EmpModel employee)
        {
            int rowseffected = 0;
            //Adding Employee
            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    var qry = "INSERT INTO mvc_master_project.t_emp(c_empid, c_empname, c_empgender, c_dob, c_shift, c_department, c_empimage) VALUES (@c_empid, @c_empname, @c_empgender, @c_dob, @c_shift, @c_department, @c_empimage);";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@c_empid", employee.c_empid);
                        cmd.Parameters.AddWithValue("@c_empname", employee.c_empname);
                        cmd.Parameters.AddWithValue("@c_empgender", employee.c_empgender);
                        cmd.Parameters.AddWithValue("@c_dob", employee.c_dob);
                        cmd.Parameters.AddWithValue("@c_shift", employee.c_shift);
                        cmd.Parameters.AddWithValue("@c_department", employee.c_department);
                        cmd.Parameters.AddWithValue("@c_empimage", employee.c_empimage);

                        con.Open();
                        rowseffected = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("-------> Add Employee Helper : " + e);
                }
                finally
                {
                    con.Close();
                }
            }
            if (rowseffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion 
    }
}