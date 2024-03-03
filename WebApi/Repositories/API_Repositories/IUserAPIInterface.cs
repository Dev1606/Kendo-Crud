using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repositories.API_Repositories
{
    public interface IUserAPIInterface
    {
        UserModel Login(LoginModel user);
        bool RegistrationDetail(UserModel user);
    }
}