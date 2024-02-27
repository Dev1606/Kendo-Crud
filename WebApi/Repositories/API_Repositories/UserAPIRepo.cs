using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using WebApi.Models;

namespace WebApi.Repositories.API_Repositories
{
    public class UserAPIRepo : IUserAPIInterface
    {
        private readonly string? _ConnectionString;
        private NpgsqlConnection _conn;
        public UserAPIRepo(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("ConStr");
            _conn = new NpgsqlConnection(_ConnectionString);
        }

        public UserModel Login(LoginModel user)
        {
            UserModel userDetails = new UserModel();
            try
            {
                _conn.Open();
                using (var cmd = new NpgsqlCommand("Select * from mvc_master_project.t_user Where c_uemail=@email and c_password=@password",_conn))
                {
                    cmd.Parameters.AddWithValue("@email", user.c_uemail);
                    cmd.Parameters.AddWithValue("@password", user.c_password);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userDetails.c_uid = (int)reader["c_uid"];
                            userDetails.c_uname = (string)reader["c_uname"];
                            userDetails.c_uemail = (string)reader["c_uemail"];
                            userDetails.c_password = (string)reader["c_password"];
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error at Login" + e);
            }
            finally
            {
                _conn.Close();
            }
            return userDetails;
        }

        // public bool RegistrationDetail(UserModel user)
        // {
        //     int rowseffected = 0;
        //     try
        //     {
        //         _conn.Open();
        //         using (var cmd = new NpgsqlCommand("Insert INTO mvc_master_project.t_user(c_uname,c_uemail,c_password) values(@username,@email,@password)", _conn))
        //         {
        //             cmd.Parameters.Add("@username", NpgsqlTypes.NpgsqlDbType.Varchar).Value = user.c_uname;
        //             cmd.Parameters.AddWithValue("@email", user.c_uemail);
        //             cmd.Parameters.AddWithValue("@password", user.c_password);
        //             rowseffected = cmd.ExecuteNonQuery();
        //         }
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine("Error occured at Register" + e);
        //     }
        //     finally{
        //         _conn.Close();
        //     }

        //     if (rowseffected > 0)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }

        // }

        public bool RegistrationDetail(UserModel user)
        {
            try
            {
                using NpgsqlCommand cmd = new NpgsqlCommand();

                cmd.Connection = _conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "INSERT INTO mvc_master_project.t_user (c_uname,c_uemail,c_password) VALUES(@c_uname,@c_uemail,@c_password);";
                cmd.Parameters.AddWithValue("c_uname", user.c_uname ??(object)DBNull.Value);
                cmd.Parameters.AddWithValue("c_uemail", user.c_uemail ??(object)DBNull.Value);
                cmd.Parameters.AddWithValue("c_password", user.c_password ??(object)DBNull.Value);
                _conn.Open();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            finally{
                _conn.Close();
            }
        }
    }
}