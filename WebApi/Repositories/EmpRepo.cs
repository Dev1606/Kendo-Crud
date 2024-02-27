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

        public List<EmpModel> GetEmpData()
        {
            var EmployeeList = new List<EmpModel>();

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

                            var employee = new EmpModel
                            {
                                c_empid = (int)reader["c_empid"],
                                c_empname = (string)reader["c_empname"],
                                c_empgender = (string)reader["c_empgender"],
                                c_dob = (DateTime)reader["c_dob"],
                                c_shift = (string[])reader["c_shift"],
                                c_department = (string)reader["c_department"],
                                c_empimage = (string)reader["c_empimage"]
                            };
                            EmployeeList.Add(employee);
                        }

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("###### Get Employee Data error: " + e);
                }
                finally
                {
                    con.Close();
                }
            }

            return EmployeeList;
        }

        public string[] GetDepartment()
        {
            List<string> Department = new List<string>();
            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    con.Open();
                    using (NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_departmentid, c_department FROM mvc_master_project.t_department;", con))
                    {
                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Department.Add((string)reader["c_department"]);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("#### Get Department data error" + e);

                }
                finally
                {
                    con.Close();
                }
            }
            return Department.ToArray();
        }

        public EmpModel GetEmpDetail(int id)
        {
            var emp = new EmpModel();
            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    var qry = "SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_department, c_empimage FROM mvc_master_project.t_emp WHERE c_empid = @id";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {

                            emp = new EmpModel
                            {
                                c_empid = (int)reader["c_empid"],
                                c_empname = (string)reader["c_empname"],
                                c_empgender = (string)reader["c_empgender"],
                                c_dob = (DateTime)reader["c_dob"],
                                c_shift = (string[])reader["c_shift"],
                                c_department = (string)reader["c_department"],
                                c_empimage = (string)reader["c_empimage"]
                            };
                        }

                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("###### Get Employee Detail error: " + e);
                }
                finally
                {
                    con.Close();
                }
            }
            return emp;
        }

        public bool DeleteEmp(int id)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    var qry = "DELETE FROM mvc_master_project.t_emp WHERE c_empid=@id;";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("#### Delete Employee Data Error : " + e);
                }
                finally
                {
                    con.Close();
                }
            }
            return true;
        }

        public bool UpdateEmp(EmpModel emp)
        {
            using (NpgsqlConnection con = new NpgsqlConnection(_ConnectionString))
            {
                try
                {
                    var qry = "UPDATE mvc_master_project.t_emp SET c_empname=@c_empname, c_empgender=@c_empgender, c_dob=@c_dob, c_shift=@c_shift, c_department=@c_department, c_empimage=@c_empimage WHERE c_empid=@c_empid;";
                    using (NpgsqlCommand cmd = new NpgsqlCommand(qry, con))
                    {
                        cmd.Parameters.AddWithValue("@c_empid", emp.c_empid);
                        cmd.Parameters.AddWithValue("@c_empname", emp.c_empname);
                        cmd.Parameters.AddWithValue("@c_empgender", emp.c_empgender);
                        cmd.Parameters.AddWithValue("@c_dob", emp.c_dob);
                        cmd.Parameters.AddWithValue("@c_shift", emp.c_shift);
                        cmd.Parameters.AddWithValue("@c_department", emp.c_department);
                        cmd.Parameters.AddWithValue("@c_empimage", emp.c_empimage);
                        Console.WriteLine("Emp details @ repo"+ emp.c_empid+emp.c_empname+ emp.c_empgender+emp.c_dob+emp.c_shift+ emp.c_department+emp.c_empimage);

                        // string c_shift = string.Join(",", emp.c_shift);
                        // cmd.Parameters.AddWithValue("@c_shift", emp.c_shift);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("####  Update Employee Data Error : " + e);
                }
                finally
                {
                    con.Close();
                }
            }
            return true;
        }

        #endregion

        #region User Repo Methods
        #endregion
    }
}