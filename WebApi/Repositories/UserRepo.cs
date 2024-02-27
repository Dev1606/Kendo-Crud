using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Npgsql;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class UserRepo : IUserInterface
    {
        private readonly string? _ConnectionString;
        private readonly NpgsqlConnection _conn;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepo(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _ConnectionString = configuration.GetConnectionString("ConStr");
            _conn = new NpgsqlConnection(_ConnectionString);
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Login(LoginModel login)
        {
            bool isUserAuthenticated = false;
            try
            {
                _conn.Open();
                using (var cmd = new NpgsqlCommand("Select * from mvc_master_project.t_user  Where c_uemail=@uemail and c_password=@password",_conn))
                {
                    cmd.Parameters.AddWithValue("@uemail", login.c_uemail);
                    cmd.Parameters.AddWithValue("@password", login.c_password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isUserAuthenticated = true;
                            var session = _httpContextAccessor.HttpContext.Session;
                            session.SetInt32("userid", (int)reader["c_uid"]);
                            session.SetString("username", reader["c_uname"].ToString());

                            // Login with Role
                            int userRoleId = (int)reader["c_role"];

                            if (userRoleId == 0)
                            {
                                session.SetInt32("isRole", 0);
                            }
                            else if (userRoleId == 1)
                            {
                                session.SetInt32("isRole", 1);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error In Login"+ex);
            }
            finally
            {
                _conn.Close();
            }
            return isUserAuthenticated;
        }

    }
}