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
        private readonly string? _conn;

        private NpgsqlConnection connection;

        public UserRepo(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            _conn = configuration.GetConnectionString("ConStr");
            connection = new NpgsqlConnection(_conn);
        }

        public UserModel Login(LoginModel login)
        {
            UserModel user = new UserModel();
            try{
                using (var cmd = new NpgsqlCommand("Select * from mvc_master_project.t_user  Where c_uemail=@uemail and c_password=@password",connection))
                {
                    cmd.Parameters.AddWithValue("@uemail", login.c_uemail);
                    cmd.Parameters.AddWithValue("@password", login.c_password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user.c_uid = (int)reader["c_uid"];
                            user.c_uname = (string)reader["c_uname"];
                            user.c_uemail = (string)reader["c_uemail"];
                            user.c_password = (string)reader["c_password"];
                            user.c_role = (int)reader["c_role"];  
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
                connection.Close();
            }
            return user;
        }

    }
}