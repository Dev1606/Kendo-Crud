using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using WebApi.Models;

namespace WebApi.Repositories.API_Repositories
{
    public class EmpAPIRepo : IEmpAPIInterface
    {
        private readonly string? _ConnectionString;
        public EmpAPIRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConStr");
        }

        #region User Repo Methods

        // //Get department In Dropdwn
        // public string[] GetDepartment()
        // {
        //     var department = new List<string>();
        //     using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
        //     {
        //         try
        //         {
        //             con.Open();
        //             using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_department FROM mvc_master_project.t_department;", con))
        //             {
        //                 var reader = cmd.ExecuteReader();
        //                 while (reader.Read())
        //                 {
        //                     department.Add((string)reader["c_department"]);
        //                 }
        //             }
        //         }
        //         catch (Exception e)
        //         {
        //             Console.WriteLine("#### Get Department helper error ##### " + e);
        //         }
        //         finally
        //         {
        //             con.Close();
        //         }
        //     }
        //     return department.ToArray();
        // }

        // UserGetEmpData
        public List<EmpModel> UserGetEmpData()
        {
            var Employees = new List<EmpModel>();

            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    var qry = "SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_department, c_empimage FROM mvc_master_project.t_emp;";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(qry, con))
                    {
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            var Employee = new EmpModel
                            {
                                c_empid = (int)reader["c_empid"],
                                c_empname = (string)reader["c_empname"],
                                c_empgender = (string)reader["c_empgender"],
                                c_dob = (DateTime)reader["c_dob"],
                                c_shift = (string[])reader["c_shift"],
                                c_department = (string)reader["c_department"],
                                c_empimage = (string)reader["c_empimage"]
                            };
                            Employees.Add(Employee);
                        }

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("------> Get Employee Helper error: " + e);
                }
                finally
                {
                    con.Close();
                }
            }
            return Employees;
        }

        // UserAddEmp Data
        public bool UserAddEmpData(EmpModel employee)
        {
            int rowseffected = 0;
            //Adding Employee
            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    var qry = "INSERT INTO mvc_master_project.t_emp(c_empname, c_empgender, c_dob, c_shift, c_department, c_empimage) VALUES (@c_empname, @c_empgender, @c_dob, @c_shift, @c_department, @c_empimage);";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@c_empname", employee.c_empname);
                        cmd.Parameters.AddWithValue("@c_empgender", employee.c_empgender);
                        cmd.Parameters.AddWithValue("@c_dob", employee.c_dob);
                        string shift = string.Join(",", employee.c_shift);
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