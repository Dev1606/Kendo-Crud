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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private NpgsqlConnection _conn;
        public UserRepo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _ConnectionString = configuration.GetConnectionString("ConStr");
            _httpContextAccessor = httpContextAccessor;
            _conn = new NpgsqlConnection(_ConnectionString);
        }
        public bool RegistrationDetail(UserModel user)
        {
            int rowseffected = 0;
            try
            {
                _conn.Open();
                using (var cmd = new NpgsqlCommand("Insert INTO mvc_master_project.t_user(c_uname,c_uemail,c_password) values(@username,@email,@password)", _conn))
                {
                    cmd.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.c_uname;
                    cmd.Parameters.AddWithValue("@email", user.c_uemail);
                    cmd.Parameters.AddWithValue("@password", user.c_password);
                    rowseffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
              Console.WriteLine("Error occured at Register" + e);
            }
            
            if(rowseffected>0){
                return true;
            }else{
                return false;
            }

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