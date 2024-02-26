using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class UserRepo : IUserInterface
    {
        private readonly string? conn;
         private readonly IHttpContextAccessor _httpContextAccessor;
        private NpgsqlConnection _conn;
        public UserRepo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            conn = configuration.GetConnectionString("ConStr");
            _httpContextAccessor = httpContextAccessor;
            _conn = new NpgsqlConnection(conn);
        }
        public void RegistrationDetail(UserModel user)
        {
            try
            {
                _conn.Open();
                using (var cmd = new NpgsqlCommand("Insert INTO mvc_master_project.t_user(c_uname,c_uemail,c_password) values(@username,@email,@password)", _conn))
                {
                    cmd.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.c_uname;
                    cmd.Parameters.AddWithValue("@email", user.c_uemail);
                    cmd.Parameters.AddWithValue("@password", user.c_password);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occured at Register" + e);
            }
            finally
            {
                _conn.Close();
            }
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString("username", user.c_uname);
            session.SetInt32("userid", user.c_uid);
        }
    }
}