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

        public List<EmpModel> GetEmpData(){
            var EmployeeList = new List<EmpModel>();

            using(NpgsqlConnection con = new NpgsqlConnection(_ConnectionString)){
                try{
                    var qry = "SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_department, c_empimage FROM mvc_master_project.t_emp;";
                    using(NpgsqlCommand cmd = new NpgsqlCommand(qry,con)){
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while(reader.Read()){

                            var employee = new EmpModel{
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

                }catch(Exception e){
                    Console.WriteLine("######nGet Employee Data error: "+e);
                }finally{
                    con.Close();
                }
            }

            return EmployeeList;
        }

        public string[] GetDepartment(){
            var Department = new List<string>();
            using(NpgsqlConnection con = new NpgsqlConnection(_ConnectionString)){
                try{
                    con.Open();
                    using(NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_departmentid, c_department mvc_master_project.t_department;",con)){
                        var reader = cmd.ExecuteReader();
                        while(reader.Read()){
                            Department.Add((string) reader["c_department"]);
                        }
                    }
                }catch(Exception e){
                    Console.WriteLine("#### Get Department data helper error ##### "+e);

                }finally{
                    con.Close();
                }
            }
            return Department.ToArray();
        }

        public EmpModel GetEmpDetail(int id){
            var emp = new EmpModel();
            using(NpgsqlConnection con = new NpgsqlConnection(_ConnectionString)){
                try{
                    var qry = "SELECT c_empid, c_empname, c_empgender, c_dob, c_shift, c_department, c_empimage FROM mvc_master_project.t_emp WHERE c_empid = @id";
                    using(NpgsqlCommand cmd = new NpgsqlCommand(qry,con)){
                        cmd.Parameters.AddWithValue("@id",id);
                        con.Open();
                        var reader = cmd.ExecuteReader();

                        while(reader.Read()){

                            emp = new EmpModel{
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

                }catch(Exception e){
                    Console.WriteLine("######nGet Specific Student Helper error: "+e);
                }finally{
                    con.Close();
                }
            }
            return emp;
        }

        public bool DeleteEmp(int id){
            return true;
        }

        public bool UpdateEmp(EmpModel emp){
            return true;
        }
        
        #endregion

        #region User Repo Methods
        #endregion 
    }
}